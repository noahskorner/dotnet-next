using Api.Extensions;
using Domain.Extensions;

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
var builder = WebApplication.CreateBuilder(args);

// Add configuration files
builder.Configuration.AddJsonFile("appsettings.json");
builder.Configuration.AddJsonFile($"appsettings.{environment}.json");

// Add services to the container.
builder.Services.RegisterDomain(builder.Configuration);
builder.Services.RegisterApi(builder.Configuration);

// Configure the HTTP request pipeline.
var app = builder.Build();
app.BuildApi();
app.Run();
