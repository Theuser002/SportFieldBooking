using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;

IConfiguration configuration = new ConfigurationBuilder()
                            .AddJsonFile("ocelot.json")
                            .Build();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// hungnt: add Ocelot
builder.Services.AddOcelot(configuration);

// Add an authentication provider key as the id for an authentication method (here we add an authentication id as "Bearer" first, reserve it for an authentication method that we may add later)
var authenticationProviderKey = "Bearer";
builder.Services.AddAuthentication().AddJwtBearer(authenticationProviderKey, x =>
{
});

// Add the authentication method "appAuth" to the authentication key (id) "Bearer"
builder.Services.AddSwaggerForOcelot(configuration, (o) =>
{
    o.AddAuthenticationProviderKeyMapping("Bearer", "appAuth");
});

var app = builder.Build();

app.UseSwaggerForOcelotUI(opt =>
{
    opt.PathToSwaggerGenerator = "/swagger/docs";
}).UseOcelot().Wait();

app.Run();
