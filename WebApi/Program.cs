using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using WebApi;

var builder = WebApplication.CreateBuilder(args);

var startup = new Startup(builder.Configuration);

startup.ConfiugureServices(builder.Services);

var app = builder.Build();

var ServicioLogger = (ILogger<Startup>)app.Services.GetService(typeof(ILogger<Startup>));   
startup.Configure(app,app.Environment, ServicioLogger); 

app.Run();
