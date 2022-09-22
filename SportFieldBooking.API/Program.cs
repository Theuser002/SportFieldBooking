using Microsoft.EntityFrameworkCore;
using Serilog;
using SportFieldBooking.Data;
using SportFieldBooking.Helper.Exceptions;
using SportFieldBooking.Helper.Tasks;
using SportFieldBooking.API.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;

#region ConfigureProject
//------------------------------ Before app is built ------------------------------
var builder = WebApplication.CreateBuilder(args);

// Instantiate system configuration
#region InstantiateSystemConfiguration
IConfiguration configuration = builder.Configuration;
#endregion

// Add Controllers
#region AddControllers
builder.Services.AddControllers();
#endregion

// Add Swaggers
#region AddSwaggers
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddMvc();

#region Uncomment this
builder.Services.AddSwaggerGen(
    options =>
    {
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

        options.SwaggerDoc("v1", new OpenApiInfo { Title = "SportFieldBookingAPI", Version = "v1" });
    }
);
#endregion

#endregion

// Add Serilog
#region AddSerilog
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .Enrich.FromLogContext()
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);
#endregion

// Dependencies Injection
#region DI
builder.Services.AddScoped<SportFieldBooking.Biz.IRepositoryWrapper, SportFieldBooking.Biz.RepositoryWrapper>();
#endregion

// DB Config
#region DBConfig
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
#endregion

#region Uncomment this
// Add Auth
#region AddAuth(JWT)
// Add Authentication
//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
//}).AddJwtBearer(
//    options =>
//    {
//        options.SaveToken = true;
//        options.RequireHttpsMetadata = false;
//        options.TokenValidationParameters = new TokenValidationParameters()
//        {
//            ValidateIssuer = true,
//            ValidateAudience = true,
//            ValidateLifetime = true,
//            ValidateIssuerSigningKey = true,
//            ClockSkew = TimeSpan.Zero,

//            ValidAudience = configuration["JWT:ValidAudience"],
//            ValidIssuer = configuration["JWT:ValidIssuer"],
//            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
//        };
//    }
//);
//Add Authorization
builder.Services.AddAuthorization();
#endregion
#endregion

// Add Quartz - Cron job scheduler to automatically change status of expired bookings
#region AddQuartz - Uncomment this
//builder.Services.AddQuartz(q =>
//{
//    q.UseMicrosoftDependencyInjectionJobFactory();

//    var jobKey = new JobKey("deactivateBookingJob");
//    q.AddJob<DeactivateBookingsJob>(jobKey, j => j.WithDescription("Deactivate bookings job..."));
//    q.AddTrigger(t => t.WithIdentity("deactivateBookingSimpleTrigger").ForJob(jobKey).StartNow().WithSimpleSchedule(x => x.WithInterval(TimeSpan.FromSeconds(10)).RepeatForever()).WithDescription("Deactivate bookings trigger..."));
//});

//builder.Services.AddQuartzHostedService(
//    q => q.WaitForJobsToComplete = true
//);
#endregion

//------------------------------ App is being built ------------------------------
var app = builder.Build();

//------------------------------ After app is built ------------------------------

// Configure the HTTP request pipeline.
#region useSwagger
#region Comment this
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
#endregion

#region uncomment this
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("v1/swagger.json", "SportFieldBookingAPI v1");
});
app.MapControllers();
#endregion

#endregion

#region UseAuth - Uncomment this
app.UseAuthentication();
app.UseAuthorization();
#endregion

#endregion

# region main
try
{
    Console.WriteLine("TEST");
    Console.WriteLine(DateTime.Now);
    Console.WriteLine(DateTime.Now.Date);
    logger.Information($"[MyLog]: Migration process");
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
    logger.Error($"[MyLog]: Error in migration process", e);
    throw;
}

app.Run();
#endregion