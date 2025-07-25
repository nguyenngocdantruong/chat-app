using ChatApp.Domain.Interfaces;
using ChatApp.Infrastructure.Data;
using ChatApp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Register DbContext
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Register mapper

            // Register repositories
            services.AddScoped<IUserRepository, UserRepository>();

            //Register UnitOfWork

            //Register Services
            return services;
        }
    }
}
