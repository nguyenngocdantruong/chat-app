using System.Runtime.CompilerServices;
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
using ChatApp.Application.AuthorizationHandler;
using ChatApp.Application.Interfaces.Authentication;
using ChatApp.Application.Interfaces.Authorization;
using ChatApp.Infrastructure.Decorators.Authorization;
using ChatApp.Infrastructure.Decorators.Logging;
using ChatApp.Infrastructure.ExternalServices.Authentication;

namespace ChatApp.Infrastructure
{
    public static class DependencyInjection
    {
        private static void AddAppContext(this IServiceCollection services, IConfiguration configuration)
        {
            // Register AppDbContext with SQL Server
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            //Register UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
        private static void AddAuth(this IServiceCollection services, IConfiguration configuration)
        {
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
        }

        private static void AddCacheService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMemoryCache();
            services.AddSingleton<ICacheService<string>, CacheService>();
            services.AddSingleton<IPresenceService, PresenceService>();
        }

        private static void AddSecurityServices(this IServiceCollection services, IConfiguration configuration)
        {
            //Add HttpContextAccessor & Current User Service
            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUserService, HttpContextService>();
            // Register authentication service
            services.AddScoped<IAuthService, AuthService>();
            services.AddSingleton<ITokenService, JwtTokenService>();
            // Register authorization handlers
            services.AddScoped<IAuthorizationHandler<Attachment>, AuthorizationAttachmentHandler>();
            services.AddScoped<IAuthorizationHandler<Conversation>, AuthorizationConversationHandler>();
            services.AddScoped<IAuthorizationHandler<Friend>, AuthorizationFriendHandler>();
            services.AddScoped<IAuthorizationHandler<Message>, AuthorizationMessageHandler>();
            services.AddScoped<IAuthorizationHandler<RefreshToken>, AuthorizationRefreshTokenHandler>();
            services.AddScoped<IAuthorizationHandler<User>, AuthorizationUserHandler>();
        }

        private static void AddMappers(this IServiceCollection services)
        {
            // Register mapper
            services.AddScoped<IAttachmentMapper, AttachmentMapper>();
            services.AddScoped<IConversationMapper, ConversationMapper>();
            services.AddScoped<IFcmTokenMapper, FcmTokenMapper>();
            services.AddScoped<IFriendMapper, FriendMapper>();
            services.AddScoped<IMessageMapper, MessageMapper>();
            services.AddScoped<IRefreshTokenMapper, RefreshTokenMapper>();
            services.AddScoped<IUserMapper, UserMapper>();
        }

        private static void AddRepositories(this IServiceCollection services)
        {
            // Register repositories
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IAttachmentRepository, AttachmentRepository>();
            services.AddScoped<IAuditLogRepository, AuditLogRepository>();
            services.AddScoped<IConversationMemberRepository, ConversationMemberRepository>();
            services.AddScoped<IConversationRepository, ConversationRepository>();
            services.AddScoped<IFcmTokenRepository, FcmTokenRepository>();
            services.AddScoped<IFriendRepository, FriendRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
        }

        private static void AddServices(this IServiceCollection services)
        {
            //Register Services
            services.AddScoped<IAuditLogService, AuditLogService>();
            services.AddScoped<IConversationService, ConversationService>();
            //Add File Service
            services.AddScoped<IFileService, LocalFileService>();
            services.AddScoped<IFriendService, FriendService>();
            services.AddScoped<IMailService, MailConsoleService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();
            services.AddScoped<IUserService, UserService>();
        }

        private static void AddDecorators(this IServiceCollection services)
        {
            //Register logging decorators
            services.Decorate<IAuthService, LoggingAuthServiceDecorator>();
            services.Decorate<IConversationService, LoggingConversationServiceDecorator>();
            services.Decorate<IFileService, LoggingFileServiceDecorator>();
            services.Decorate<IFriendService, LoggingFriendServiceDecorator>();
            services.Decorate<IMailService, LoggingMailServiceDecorator>();
            services.Decorate<IMessageService, LoggingMessageServiceDecorator>();
            services.Decorate<IRefreshTokenService, LoggingRefreshTokenServiceDecorator>();
            services.Decorate<IUserService, LoggingUserServiceDecorator>();

            //Register authorization decorators
            services.Decorate<IAuthService, AuthorizationAuthServiceDecorator>();
            services.Decorate<IConversationService, AuthorizationConversationServiceDecorator>();
            services.Decorate<IFileService, AuthorizationFileServiceDecorator>();
            services.Decorate<IFriendService, AuthorizationFriendServiceDecorator>();
            services.Decorate<IMessageService, AuthorizationMessageServiceDecorator>();
            services.Decorate<IRefreshTokenService, AuthorizationRefreshTokenServiceDecorator>();
            services.Decorate<IUserService, AuthorizationUserServiceDecorator>();
        }

        private static void AddRealtimeService(this IServiceCollection services)
        {
            //Register SignalR
            services.AddSignalR(options =>
            {

            });
        }

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAppContext(configuration);
            services.AddAuth(configuration);
            services.AddCacheService(configuration);
            services.AddMappers();
            services.AddRepositories();
            services.AddServices();
            services.AddSecurityServices(configuration);
            services.AddDecorators();
            services.AddRealtimeService();
            return services;
        }
    }
}
