
using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ProductsService
{
    public static class ServiceRegistryExtensions
    {
        public static IServiceCollection AddConsulConfig(this IServiceCollection services, IConfiguration config)
        {
            services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
            {
                consulConfig.Address = new System.Uri(config["Configuration:ConsulAddress"]);

            }));
            return services;
        }
        public static IApplicationBuilder UseConsul(this IApplicationBuilder app, IConfiguration config)
        {
            var consulClient = app.ApplicationServices.GetRequiredService<IConsulClient>();
            var logger = app.ApplicationServices.GetRequiredService<ILoggerFactory>().CreateLogger("AppExtensions");
            var lifetime = app.ApplicationServices.GetRequiredService<IApplicationLifetime>();
            var registration = new AgentServiceRegistration()
            {
                ID = config["Configuration:ServiceName"],
                Name = config["Configuration:ServiceName"],
                Address = config["Configuration:ServiceHost"],
                Port = int.Parse(config["Configuration:ServicePort"])
            };
            logger.LogInformation("Registering with consul");
            consulClient.Agent.ServiceDeregister(registration.ID).ConfigureAwait(true);
            consulClient.Agent.ServiceRegister(registration).ConfigureAwait(true);
            lifetime.ApplicationStopping.Register(() => { logger.LogInformation("Unregistering from consul"); });
            return app;
        }
    }
}
