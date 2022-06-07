using Ocelot.DependencyInjection;
using Ocelot.Middleware;

IConfiguration configuration = new ConfigurationBuilder()
                            .AddJsonFile("ocelot.json")
                            .Build();

var builder = WebApplication.CreateBuilder(args);

// hungnt: add Ocelot
builder.Configuration.AddJsonFile("ocelot.json");
builder.Services.AddOcelot(configuration);

var app = builder.Build();

app.UseOcelot();

app.Run();

