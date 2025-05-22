using API_Computos_Publica.Data.DbContexts;
using API_Computos_Publica.Data.Inicializador;
using API_Computos_Publica.Data.Repository.Clases;
using API_Computos_Publica.Data.Repository.Interfacez;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NLog;
using NLog.Web;
using System.Threading.RateLimiting;

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");
try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    builder.Host.UseNLog();
    builder.Services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
    builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), sqlServerOptions => sqlServerOptions.UseCompatibilityLevel(110)));

    builder.Services.AddTransient<I_Inicializador, Inicializador>();
    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

    builder.Services.AddHttpContextAccessor();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "API_Computos_Publica", Version = "v1" });
    });
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("CorsPolicy", builder =>
        {
            builder.AllowAnyOrigin()
                 .AllowAnyHeader()
                 .AllowAnyMethod()
                 .SetIsOriginAllowed(origin => true);
        });
    });
    builder.Services.AddRateLimiter(options =>
    {
        options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
        options.AddPolicy("fixed", httpcontext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpcontext.Connection.RemoteIpAddress?.ToString(),
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 10,
                Window = TimeSpan.FromSeconds(60)
            }));
    });

    builder.Services.AddOutputCache(opciones =>
    {
        opciones.AddPolicy("consultas", builder => builder.Expire(TimeSpan.FromMinutes(0)).Tag("consultas"));
    });

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    app.UseRateLimiter();
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("CorsPolicy");
    app.UseStaticFiles();
    app.UseHttpsRedirection();
    app.MapControllers();
    app.UseOutputCache();
    app.Run();
}
catch (Exception ex)
{

    string type = ex.GetType().Name;
    if (type.Equals("StopTheHostException", StringComparison.Ordinal))
    {
        throw;
    }
    // NLog: catch setup errors
    logger.Error(ex, "Stopped program because of exception");
    logger.Error($"{ex.Message} <=> {ex.StackTrace} <=> {ex.Source}");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}
