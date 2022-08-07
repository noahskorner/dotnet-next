using Api;

var builder = WebApplication.CreateBuilder(args);
builder.AddConfiguration();

// Add services to the container.
builder.AddCors();
builder.AddSqlServer();
builder.Services.AddServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseCors("Default");
app.UseAuthorization();
app.MapControllers();
app.Run();
