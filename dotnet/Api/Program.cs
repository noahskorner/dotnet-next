using Api.Extensions;
using Serilog;
using Services.Extensions;

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
var builder = WebApplication.CreateBuilder(args);

// Add configuration files
builder.Configuration.AddJsonFile("appsettings.json");
builder.Configuration.AddJsonFile($"appsettings.{environment}.json", true);

// Add logging
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();
builder.Host.UseSerilog();

// Add services to the container.
builder.Services.RegisterServices(builder.Configuration);
builder.Services.RegisterApi(builder.Configuration);

// Configure the HTTP request pipeline.
var app = builder.Build();
app.UseSerilogRequestLogging();
app.BuildApi();
app.Run();
