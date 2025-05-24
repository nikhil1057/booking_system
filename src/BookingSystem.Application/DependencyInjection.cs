using Microsoft.Extensions.DependencyInjection;

namespace BookingSystem.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Register MediatR handlers, validators, AutoMapper profiles, etc.

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

            services.AddAutoMapper(typeof(DependencyInjection).Assembly);

            // Register FluentValidation validators here if you have any, e.g.
            // services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

            return services;
        }
    }
}
