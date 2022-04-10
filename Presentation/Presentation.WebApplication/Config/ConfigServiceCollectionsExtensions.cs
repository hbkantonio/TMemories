using Core.Business.Auth;
using Core.Business.Services;
using Core.Contracts.Data;
using Core.Contracts.Repositories;
using Core.Contracts.Service;
using Infrastructure.Data;
using Infrastructure.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Transversal.Helpers;

namespace Presentation.WebApplication.Config
{
    public static class ConfigServiceCollectionsExtensions
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services)
        {
            //
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddTransient<JwtAuthManager>();
            services.AddTransient<EmailSenderHelper>();

            // Repositories
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ICountryRepository, CountryRepository>();

            // services
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<ICatalogService, CatalogService>();
            return services;
        }

    }
}
