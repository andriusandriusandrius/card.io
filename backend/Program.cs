using backend.Data;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
Env.Load();
builder.Services.AddDbContext<CardioContext>(options => options.UseNpgsql(Environment.GetEnvironmentVariable("DB_CONNECTION")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.Run();

