
using ChatApp.Api.ActionFilter;
using ChatApp.Api.Middlewares;
using ChatApp.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using ChatApp.Api.Hubs;

namespace ChatApp.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddInfrastructure(builder.Configuration);

            builder.Services.AddControllers(options =>
                {
                    options.Filters.Add<ValidationFilterAttribute>();
                })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });
            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                // Tắt cơ chế tự động của [ApiController] để filter có thể chạy
                options.SuppressModelStateInvalidFilter = true;
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            var app = builder.Build();


            //Custom Middleware
            app.UseMiddleware<ExceptionMiddleware>();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
            //    app.UseSwagger();
            //    app.UseSwaggerUI();
            //}

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseAuthentication(); 
            app.UseAuthorization();


            app.MapControllers();
            app.MapHub<ChatHub>("/chatHub");

            app.UseCors("CorsPolicy");

            app.Run();
        }
    }
}
