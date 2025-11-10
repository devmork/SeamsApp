using SeamsApp.Data.Repositories;
using SeamsApp.Interfaces.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IStudentsRepository, StudentsRepository>();
builder.Services.AddOutputCache();
builder.Services.AddCors();
// Configure the HTTP request pipeline.

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();
app.MapControllers();

app.Run();
