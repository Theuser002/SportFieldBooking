using Quartz;
using Quartz.Impl;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SportFieldBooking.Data;
using SportFieldBooking.Helper.Exceptions;
using SportFieldBooking.Helper.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;

#region add_configuraions
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    // Fix "SchemaId already used for ... error (error can be seen in log)"
    options.CustomSchemaIds(type => type.ToString());

    // Add JWT on Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Bearer authentication with JWT token",
        Type = SecuritySchemeType.Http
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        },
    });
});

// Add system configuration
IConfiguration configuration = builder.Configuration;

// Add Serilog
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .Enrich.FromLogContext()
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

// Dependencies Injection
builder.Services.AddScoped<SportFieldBooking.Biz.IRepositoryWrapper, SportFieldBooking.Biz.RepositoryWrapper>();

// DB Config
string dbProvider = configuration["DatabaseOptions:Provider"];
// For Entity Framework
switch (dbProvider.ToLower())
{
    case "sqlserver":
        builder.Services.AddDbContext<DomainDbContext, SQLServerDbContext>();
        break;
    default:
        throw new Exception($"Wrong provider's name or unsupported provider: {dbProvider}");
}

// Adding Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
// Adding Jwt Bearer
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,

        ValidAudience = configuration["JWT:ValidAudience"],
        ValidIssuer = configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

#endregion

# region main
try
{
    Console.WriteLine("TEST");
    logger.Information($"[MyLog]: Migrating process");
    Console.WriteLine(DateTime.Now.Date);
    using (var scope = app.Services.CreateScope())
    {
        using (var context = scope.ServiceProvider.GetRequiredService<DomainDbContext>())
        {
            context.Database.Migrate();
        }
    }

    DeactivateBookingsScheduler.StartAsync().GetAwaiter().GetResult();
}
catch (Exception e)
{
    logger.Error($"[MyLog]: Error in migrating process", e);
    throw;
}

app.Run();
#endregion