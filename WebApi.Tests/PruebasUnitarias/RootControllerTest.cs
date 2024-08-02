using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApi.Controllers.v1;
using WebApi.Tests.Mocks;

namespace WebApi.Tests.PruebasUnitarias
{
    [TestClass]
    public class RootControllerTest
    {
        [TestMethod]
        public async Task Admin_4Links()
        {
            //PREP
            var autotizationService = new AuthorizationServiceSuccessMock();
            var rootController = new RootController(autotizationService);
            rootController.Url = new UrlHelperMock();

            //EJECUCION
            var result = await rootController.Get();

            //VERIFICACION
            Assert.AreEqual(4, result.Value.Count());
        }


        [TestMethod]
        public async Task Admin_4Links_Moq()
        {
            //PREP
            var mockAuthorizationService = new Mock<IAuthorizationService>();
            mockAuthorizationService.Setup(x => x.AuthorizeAsync(
                It.IsAny<ClaimsPrincipal>(),
                It.IsAny<object>(),
                It.IsAny<IEnumerable<IAuthorizationRequirement>>()
                )).Returns(Task.FromResult(AuthorizationResult.Failed()));

            mockAuthorizationService.Setup(x => x.AuthorizeAsync(
                It.IsAny<ClaimsPrincipal>(),
                It.IsAny<object>(),
                It.IsAny<string>()
                )).Returns(Task.FromResult(AuthorizationResult.Failed()));

            var mockUrlHelper = new Mock<IUrlHelper>();
            mockUrlHelper.Setup(x => x.Link(
                It.IsAny<string>(),
                It.IsAny<object>()
                )).Returns(string.Empty);

            var rootController = new RootController(mockAuthorizationService.Object);
            rootController.Url = mockUrlHelper.Object;

            //EJECUCION
            var result = await rootController.Get();

            //VERIFICACION
            Assert.AreEqual(4, result.Value.Count());
        }

    }
}
