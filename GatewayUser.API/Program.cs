using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.Text;

IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("ocelot.json").Build();   

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();

// Add an authentication provider key as the id for an authentication scheme (here we add an authentication id as "Bearer" first, reserve it for an authentication scheme that we may add later)
var authenticationProviderKey = "Bearer";

builder.Services.AddAuthentication().AddJwtBearer(authenticationProviderKey, x =>
{
});

builder.Services.AddOcelot(configuration);

// Add the authentication method "appAuth" to the authentication key (id) "Bearer"
builder.Services.AddSwaggerForOcelot(configuration, (o) =>
{
    o.AddAuthenticationProviderKeyMapping(authenticationProviderKey, "appAuth");
});

var app = builder.Build();

app.UseSwaggerForOcelotUI( opt =>
{
    opt.PathToSwaggerGenerator = "/swagger/docs";
    //opt.DownstreamSwaggerHeaders = new[]
    //{
    //    new KeyValuePair<string, string>("Auth-Key", "AuthValue"),
    //};
}).UseOcelot().Wait();

/*
    I'm having ans authentication problem. After using the login method of swagger, only the endpoints that do not require authentication are authenticated. Maybe this will help: https://www.youtube.com/watch?v=hlUGZ6Hmv6s
 */

app.Run();
