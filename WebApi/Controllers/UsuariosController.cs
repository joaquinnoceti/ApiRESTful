using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebApi.DTOs;
using WebApi.Servicios;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/cuentas")]
    public class UsuariosController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IConfiguration configuration;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly HashService hashService;
        private readonly IDataProtector dataProtector;

        public UsuariosController(UserManager<IdentityUser> userManager,
            IConfiguration configuration, SignInManager<IdentityUser> signInManager,
            IDataProtectionProvider dataProtectionProvider, HashService hashService)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.signInManager = signInManager;
            this.hashService = hashService;
            dataProtector = dataProtectionProvider.CreateProtector("valorSecreto");
        }

        [HttpGet("hash/{textoPlano}")]
        public ActionResult RealizarHash(string textoPlano)
        {
            var resultado1= hashService.Hash(textoPlano);
            var resultado2= hashService.Hash(textoPlano);
            return Ok(new {
                TextoPlano = textoPlano,
                    Hash1 = resultado1,
                    Hash2 = resultado2
            });

        }

        [HttpGet("encriptado")]
        [AllowAnonymous]
        public ActionResult Encriptar()
        {
            var textoPlano = "test1551";
            var textoEncriptado = dataProtector.Protect(textoPlano);
            var textoDesencriptado = dataProtector.Unprotect(textoEncriptado);

            return Ok(new
            {
                textoPlano = textoPlano,
                textoEncriptado = textoEncriptado,
                textoDesencriptado = textoDesencriptado
            });
        }
        [HttpGet("encriptadoPorTiempo")]
        [AllowAnonymous]
        public ActionResult EncriptarPorTiempo()
        {
            var protectorPorTiempo = dataProtector.ToTimeLimitedDataProtector();

            var textoPlano = "test1551";
            var textoEncriptado = protectorPorTiempo.Protect(textoPlano, lifetime: TimeSpan.FromSeconds(5));
            Thread.Sleep(TimeSpan.FromSeconds(6));
            var textoDesencriptado = protectorPorTiempo.Unprotect(textoEncriptado);

            return Ok(new
            {
                textoPlano = textoPlano,
                textoEncriptado = textoEncriptado,
                textoDesencriptado = textoDesencriptado
            });
        }

        [HttpPost("registrar")]
        public async Task<ActionResult<RespuestaAutenticacion>> Registrar(CredencialesUsuario credencialesUsuario)
        {
            var user = new IdentityUser { UserName = credencialesUsuario.Email, Email = credencialesUsuario.Email };

            var resultado = await userManager.CreateAsync(user, credencialesUsuario.Password);

            if (!resultado.Succeeded)
            {
                return BadRequest(resultado.Errors);
            }
            else
            {
                return await ConstruirToken(credencialesUsuario);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<RespuestaAutenticacion>> Login(CredencialesUsuario credencialesUsuario)
        {
            var resultado = await signInManager.PasswordSignInAsync(credencialesUsuario.Email, credencialesUsuario.Password,
                isPersistent: false, lockoutOnFailure: false);

            if (!resultado.Succeeded)
            {
                return BadRequest("Login Incorrecto");
            }
            else
            {
                return await ConstruirToken(credencialesUsuario);
            }

        }

        [HttpGet("RenovarToken")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<RespuestaAutenticacion>> RenovarToken()
        {
            var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
            var email = emailClaim.Value;
            var credencialesUsuario = new CredencialesUsuario()
            {
                Email = email
            };
            return await ConstruirToken(credencialesUsuario);
        }


        private async Task<RespuestaAutenticacion> ConstruirToken(CredencialesUsuario credencialesUsuario)
        {
            var claims = new List<Claim>()
            {
                new Claim("email", credencialesUsuario.Email),
            };

            var usuario = await userManager.FindByEmailAsync(credencialesUsuario.Email);
            var claimsDB = await userManager.GetClaimsAsync(usuario);
            claims.AddRange(claimsDB);

            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["llavejwt"]));
            var creds = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);

            var expiracion = DateTime.UtcNow.AddDays(1);

            var securityToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expiracion, signingCredentials: creds);

            return new RespuestaAutenticacion()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
                Expiracion = expiracion
            };

        }
        [HttpPost("DarAdministrador")]
        public async Task<ActionResult> DarAdministrador(EditarAdminDTO editarAdminDTO)
        {
            var user = await userManager.FindByEmailAsync(editarAdminDTO.Email);
            await userManager.AddClaimAsync(user, new Claim("esAdmin", "1"));
            return NoContent();
        }
        [HttpPost("RemoverAdministrador")]
        public async Task<ActionResult> RemoverAdministrador(EditarAdminDTO editarAdminDTO)
        {
            var user = await userManager.FindByEmailAsync(editarAdminDTO.Email);
            await userManager.RemoveClaimAsync(user, new Claim("esAdmin", "1"));
            return NoContent();
        }

    }
}
