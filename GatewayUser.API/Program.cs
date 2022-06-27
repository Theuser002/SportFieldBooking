using Ocelot.DependencyInjection;
using Ocelot.Middleware;

IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("ocelot.json").Build();   

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOcelot(configuration);
builder.Services.AddSwaggerForOcelot(configuration);

var app = builder.Build();

app.UseSwaggerForOcelotUI( opt =>
{
    opt.PathToSwaggerGenerator = "/swagger/docs";
}).UseOcelot().Wait();

app.Run();
