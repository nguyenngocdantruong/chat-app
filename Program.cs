using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.OpenApi.Models;
using ChatApp.Infrastructure.Data;
using ChatApp.Domain.Interfaces;
using ChatApp.Infrastructure.Repositories;
using ChatApp.Application.Services;
using ChatApp.Application.Interfaces;
using ChatApp.Presentation.Middlewares;
using ChatApp.Infrastructure;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Configure Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ChatApp API",
        Version = "v1",
        Description = "Backend API for Flutter Chat App"
    });
});

builder.Services.AddInfrastructure(builder.Configuration);


builder.Services.AddControllers();

var app = builder.Build();
// Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
