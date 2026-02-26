using Acceloka.Behaviors;
using Acceloka.Entities;
using Acceloka.Middleware;
using Acceloka.Services;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Reflection;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File(
        path: "logs/Log-.txt",
        rollingInterval: RollingInterval.Day,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
    ).CreateLogger();

try
{
    Log.Information("Starting Web Host");

    var builder = WebApplication.CreateBuilder(args);
    var configuration = builder.Configuration;

    builder.Host.UseSerilog();

    builder.Services.AddControllers();
    builder.Services.AddOpenApi();

    builder.Services.AddEntityFrameworkSqlServer();
    builder.Services.AddDbContext<AccelokaContext>(options =>
    {
        var conString = configuration.GetConnectionString("DefaultConnection");
        options.UseSqlServer(conString);

    });

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowReactApp",
            policy =>
            {
                policy.WithOrigins("http://localhost:3000") // URL React kamu
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });
    });

    builder.Services.AddTransient<ServiceTicket>();
    builder.Services.AddTransient<ServiceBookedTicket>();

    builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

    builder.Services.AddMediatR(cfg =>
    {
        cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
    });

    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
    builder.Services.AddProblemDetails();

    var app = builder.Build();

    app.UseCors("AllowReactApp");

    app.UseExceptionHandler(); 
    app.UseSerilogRequestLogging(); 

    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
    }

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}