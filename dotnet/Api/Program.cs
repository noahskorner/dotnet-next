using Api;
using Api.Configuration;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json");
var corsConfig = builder.Configuration.GetSection(CorsConfiguration.Cors).Get<CorsConfiguration>();
var sqlServerConfig = builder.Configuration.GetSection(SqlServerConfiguration.SqlServer).Get<SqlServerConfiguration>();

// Add services to the container.
builder.Services.AddConfiguration(builder.Configuration);
builder.Services.AddCors(corsConfig);
builder.Services.AddSqlServer(sqlServerConfig);
builder.Services.AddServices();


// Configure the HTTP request pipeline.
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseCors(corsConfig.PolicyName);
app.UseAuthorization();
app.UseUnknownExceptionHandler();
app.UseFluentValidationExceptionHandler();
app.MapControllers();
app.Run();
