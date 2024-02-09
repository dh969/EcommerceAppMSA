using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityConfiguration.IdentityConfig
{
    public static class IdentityExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {
            services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = "IdentityServer.Cookie";
                config.LoginPath = "/Account/Login";
            });
            services.AddIdentity<AppUser, IdentityRole>().
                AddEntityFrameworkStores<IndentityDbContext>().
                AddDefaultTokenProviders().
                AddSignInManager<SignInManager<AppUser>>();
            services.AddIdentityServer().AddInMemoryApiResources(Config.GetApis()).
                AddAspNetIdentity<AppUser>().
                AddInMemoryClients(Config.GetClients()).
                AddInMemoryApiScopes(Config.Scopes).
                AddDeveloperSigningCredential().
                AddProfileService<IdentityProfileService>();

            return services;
        }
    }
}
