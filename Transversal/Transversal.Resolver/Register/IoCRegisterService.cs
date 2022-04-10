using Core.Business.Auth;
using Core.Business.Services;
using Core.Contracts.Service;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transversal.Resolver.Register
{
    public static class IoCRegisterService
    {
        public static IServiceCollection AddRegistration(IServiceCollection services)
        {
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<JwtAuthManager>();
            return services;
        }
    }
}
