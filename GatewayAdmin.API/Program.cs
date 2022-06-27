using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Microsoft.OpenApi.Models;

IConfiguration configuration = new ConfigurationBuilder()
                            .AddJsonFile("ocelot.json")
                            .Build();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddMvc();
//builder.Services.AddSwaggerGen(c =>
//{
//    c.SwaggerDoc("v1", new OpenApiInfo { Title = "SportFieldBookingAPI", Version = "v1" });
//});

//builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// hungnt: add Ocelot
//builder.Configuration.AddJsonFile("ocelot.json");
builder.Services.AddOcelot(configuration);

//// hungnt: add Swagger for Ocelot
builder.Services.AddSwaggerForOcelot(configuration);

var app = builder.Build();

app.UseSwaggerForOcelotUI(opt =>
{
    opt.PathToSwaggerGenerator = "/swagger/docs";
}).UseOcelot().Wait();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseAuthorization();

//app.MapControllers();

app.Run();
