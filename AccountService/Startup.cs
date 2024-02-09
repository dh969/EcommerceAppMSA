using AccountService.Entities;
using AccountService.Identity;
using GlobalExceptionHandler;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProductsService;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AccountService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddConsulConfig(Configuration);
            services.AddControllersWithViews();
            services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = "IdentityServer.Cookie";
                config.LoginPath = "/Account/Login";
            });
            services.AddDbContext<IndentityDbContext>(options =>
      options.UseInMemoryDatabase("IdentityDatabase"));
            
                    
            services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<IndentityDbContext>().AddDefaultTokenProviders().AddSignInManager<SignInManager<AppUser>>();
            services.AddIdentityServer(x => { x.IssuerUri = "http://localhost:5002"; }).AddInMemoryApiResources(Config.GetApis()).AddAspNetIdentity<AppUser>().
                AddInMemoryClients(Config.GetClients()).AddInMemoryApiScopes(Config.Scopes).AddDeveloperSigningCredential().AddProfileService<IdentityProfileService>();
            //IdentityExtensions.AddIdentityServices(services);
           
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AccountService", Version = "v1" });
            });
            //services.AddDbContext<IndentityDbContext>(x => { x.UseSqlServer(Configuration.GetConnectionString("IdentityConnection")); });
          
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AccountService v1"));
            }
            app.UseConsul(Configuration);
            app.UseSerilogRequestLogging();
            app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
         
           // app.UseHttpsRedirection();
            app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            app.UseStaticFiles();
            app.UseRouting();
            app.UseIdentityServer();
            app.UseCookiePolicy(new CookiePolicyOptions()
            {
                MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.Lax
            });
            //app.UseAuthentication();
            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
            using var scope = app.ApplicationServices.CreateScope();//in .net 6 or above use just Services
            var services = scope.ServiceProvider;
           
            var identityContext = services.GetRequiredService<IndentityDbContext>();
            var userManager = services.GetRequiredService<UserManager<AppUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var logger = services.GetRequiredService<ILogger<Program>>();
            try
            {

               //await  identityContext.Database.MigrateAsync();
                //await StoreContextSeed.SeedAsync(context);
                await IdentitySeed.SeedUsersAsync(userManager, roleManager);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "an error has occured");
            }
        }
    }
}
