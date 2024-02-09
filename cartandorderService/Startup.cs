using cartandorderService.Data;
using cartandorderService.Entities;
using cartandorderService.Repository;
using GlobalExceptionHandler;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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

namespace cartandorderService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public async void ConfigureServices(IServiceCollection services)
        {
            services.AddConsulConfig(Configuration);
            services.AddControllers().AddNewtonsoftJson();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "cartandorderService", Version = "v1" });
            });
             var url =ServiceUrlProvider.GetAccountService();
            services.AddDbContext<StoreContext>(options =>
          options.UseInMemoryDatabase("CartAndOrderDatabase"));
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddAuthentication("Bearer").AddJwtBearer("Bearer", config =>
            {

                config.Authority = url;

                //config.TokenValidationParameters = new TokenValidationParameters

                //{

                //    ValidateAudience = false

                //};
                config.Audience = "ApiTwo";
                config.RequireHttpsMetadata = false;
            });
            services.AddHttpClient();
            services.AddAuthorization();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseConsul(Configuration);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "cartandorderService v1"));
            }

            app.UseSerilogRequestLogging();
            app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

            app.UseHttpsRedirection();
            app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            using var scope = app.ApplicationServices.CreateScope();//in .net 6 or above use just Services
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<StoreContext>();

            var logger = services.GetRequiredService<ILogger<Program>>();
            try
            {

                await CartSeed.SeedAsync(context);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "an error has occured");
            }
        }
    }
}
