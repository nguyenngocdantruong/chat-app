using ChatApp.Application.DTOs.Request;
using ChatApp.Application.DTOs.Response;
using ChatApp.Application.Interfaces;
using ChatApp.Application.Interfaces.ExternalService;
using ChatApp.Application.Interfaces.Mapper;
using ChatApp.Application.Interfaces.Services;
using ChatApp.Application.Mapper;
using ChatApp.Application.Services;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Interfaces;
using ChatApp.Infrastructure.Configurations;
using ChatApp.Infrastructure.Data;
using ChatApp.Infrastructure.ExternalServices.CacheService;
using ChatApp.Infrastructure.ExternalServices.FileStorage;
using ChatApp.Infrastructure.ExternalServices.MailService;
using ChatApp.Infrastructure.ExternalServices.TokenService;
using ChatApp.Infrastructure.Repositories;
using ChatApp.Shared.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Authentication;
using System.Text;
using ChatApp.Application.Interfaces.Authentication;
using ChatApp.Infrastructure.ExternalServices.Authentication;

namespace ChatApp.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            //Register configuration settings
            services.Configure<JwtSetting>(configuration.GetSection("JwtSettings"));
            
            // Register JwtSetting as singleton with resolved configuration
            services.AddSingleton<ITokenSetting>(provider =>
            {
                var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSetting>();
                if (jwtSettings == null)
                {
                    throw new InvalidOperationException("JWT settings not found in configuration!");
                }
                return jwtSettings;
            });

            //Register JWT authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSetting>();
                var key = Encoding.UTF8.GetBytes(jwtSettings.SecretKey);
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
                options.Events = new JwtBearerEvents()
                {
                    OnChallenge = context =>
                    {
                        // Bỏ qua response mặc định
                        context.HandleResponse();

                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.ContentType = "application/json";

                        var response = new
                        {
                            StatusCode = StatusCodes.Status401Unauthorized,
                            Message = "Not authentication or invalid token",
                            IsSuccess = false
                        };

                        return context.Response.WriteAsJsonAsync(response);
                    },
                    OnForbidden = context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        context.Response.ContentType = "application/json";

                        var response = new
                        {
                            StatusCode = StatusCodes.Status403Forbidden,
                            Message = "Forbidden",
                            IsSuccess = false
                        };

                        return context.Response.WriteAsJsonAsync(response);
                    }
                };
            }); 

            //Add memory cache
            services.AddMemoryCache();
            services.AddSingleton<ICacheService<string>, CacheService>();

            //Add HttpContextAccessor & Current User Service
            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUserService, HttpContextService>();

            //Add File Service
            services.AddScoped<IFileService, LocalFileService>();

            // Register DbContext
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            //services.AddScoped<DbContext, AppDbContext>();

            // Register mapper
            services.AddScoped<IRefreshTokenMapper, RefreshTokenMapper>();
            services.AddScoped<IUserMapper, UserMapper>();

            // Register repositories
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IAuditLogRepository, AuditLogRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            //Register UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //Register Services
            //services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));
            services.AddScoped<IAuditLogService, AuditLogService>();
            services.AddSingleton<ITokenService, JwtTokenService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IMailService, MailConsoleService>();
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();


            //Register SignalR
            services.AddSignalR(options =>
            {

            });

            return services;
        }
    }
}
