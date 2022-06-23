using Api.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json");

// Add services to the container.
var connectionString = builder.Configuration["SqlServer:ConnectionString"];
var poolSize = builder.Configuration.GetValue("SqlServer:PoolSize", defaultValue: 128);
var enableSensitiveDataLogging = true;

builder.Services.AddDbContextPool<ApiContext>(options => options
    .UseSqlServer(connectionString, x => x.EnableRetryOnFailure())
    .EnableSensitiveDataLogging(enableSensitiveDataLogging)
    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking), poolSize);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
