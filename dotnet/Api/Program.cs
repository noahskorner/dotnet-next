using Api.Data;
using Api.Services.PasswordManager;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json");

// Add services to the container.
var policyName = builder.Configuration["Cors:PolicyName"];
var origins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: policyName, policy =>
    {
        policy.WithOrigins(origins);
    });
});

var connectionString = builder.Configuration["SqlServer:ConnectionString"];
var poolSize = builder.Configuration.GetValue("SqlServer:PoolSize", defaultValue: 128);
var enableSensitiveDataLogging = true;
builder.Services.AddDbContextPool<ApiContext>(options => options
    .UseSqlServer(connectionString, x => x.EnableRetryOnFailure())
    .EnableSensitiveDataLogging(enableSensitiveDataLogging)
    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking), poolSize);

builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IPasswordManager, PasswordManager>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(policyName);

app.UseAuthorization();

app.MapControllers();

app.Run();
