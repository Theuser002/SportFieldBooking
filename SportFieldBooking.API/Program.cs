using Microsoft.EntityFrameworkCore;
using Serilog;
using SportFieldBooking.Data;
using SportFieldBooking.Helper.Exceptions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Add Serilog
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Automapper
builder.Services.AddScoped<SportFieldBooking.Biz.IRepositoryWrapper, SportFieldBooking.Biz.RepositoryWrapper>();

// Add system configuration
IConfiguration configuration = builder.Configuration;

// DB Config
string dbProvider = configuration["DatabaseOptions:Provider"];

switch (dbProvider)
{
    case "SQLServer":
        builder.Services.AddDbContext<DomainDbContext, SQLServerDbContext>();
        break;
    default:
        throw new Exception($"Wrong provider's name or unsupported provider: {dbProvider}");
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

try
{
    Console.WriteLine("TEST");
    logger.Information($"[MyLog]: Migrating process");
    using (var scope = app.Services.CreateScope())
    {
        using (var context = scope.ServiceProvider.GetRequiredService<DomainDbContext>())
        {
            context.Database.Migrate();
        }
    }
}
catch (Exception e)
{
    logger.Error($"[MyLog]: Error in migrating process", e);
    throw;
}

app.Run();
