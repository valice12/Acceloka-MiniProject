using Acceloka.Entities;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Acceloka.Services;

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
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog();

    var configuration = builder.Configuration;

    builder.Services.AddTransient<AccelokaContext>();
    builder.Services.AddTransient<ServiceTicket>();
    builder.Services.AddTransient<ServiceBookedTicket>();
    builder.Services.AddControllers();
    builder.Services.AddOpenApi();

    builder.Services.AddDbContextPool<AccelokaContext>(options =>
    {
        var conString = configuration.GetConnectionString("DefaultConnection");
        options.UseSqlServer(conString);
    });

    var app = builder.Build();

    app.UseSerilogRequestLogging();

    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
    }

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