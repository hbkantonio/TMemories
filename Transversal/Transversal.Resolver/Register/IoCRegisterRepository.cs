using Core.Contracts.Repositories;
using Infrastructure.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transversal.Resolver.Register
{
    public static class IoCRegisterRepository
    {
        public static IServiceCollection AddRegistration(IServiceCollection services)
        {
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ICountryRepository, CountryRepository>();

            return services;
        }
    }
}
