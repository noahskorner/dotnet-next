using Api.Configuration;
using Api.Extensions;

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
var builder = WebApplication.CreateBuilder(args);
var corsConfig = builder.Configuration.GetSection(CorsConfiguration.Cors).Get<CorsConfiguration>();
builder.Configuration.AddJsonFile("appsettings.json");
builder.Configuration.AddJsonFile($"appsettings.{environment}.json");

// Add services to the container.
builder.Services
    .AddConfiguration(builder.Configuration)
    .AddCors(builder.Configuration)
    .AddSqlServer(builder.Configuration)
    .AddMiddleware()
    .AddServices();

// Configure the HTTP request pipeline.
var app = builder.Build();
app.RunMigrations();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseCors(corsConfig.PolicyName);
app.UseExceptionHandler();
app.MapControllers();
app.Run();
