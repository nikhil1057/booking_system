using BookingSystem.Core.Interfaces;
using BookingSystem.Infrastructure.Data;
using BookingSystem.Infrastructure.Repositories;
using BookingSystem.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookingSystem.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("SqlServerConnection")
                                     ?? "Server=localhost;Database=BookingSystem;Trusted_Connection=True;"));

            // Register repositories
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IMemberRepository, MemberRepository>();
            services.AddScoped<IInventoryRepository, InventoryRepository>();
            services.AddScoped<IBookingRepository, BookingRepository>();

            // Register services
            services.AddScoped<IFileImporter, FileImporter>();

            return services;
        }

    }
}
