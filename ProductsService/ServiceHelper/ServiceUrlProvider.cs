using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;

namespace ProductsService.ServiceHelper
{
    public class ServiceUrlProvider
    {
        public ServiceUrlProvider()
        {
        }

        public static Dictionary<string, ServiceInfoExtension> serviceDetails { get; private set; }

        private static void UrlInitializer()
        {
            string ConsulUrl = "http://localhost:8500/v1/agent/services";
            var http = new HttpClient();
            var result = http.GetStringAsync(ConsulUrl).Result;
            serviceDetails = JsonConvert.DeserializeObject<Dictionary<string, ServiceInfoExtension>>(result);
        }

        public static string GetAccountService()
        {
            UrlInitializer();
            var port = serviceDetails["AccountService"].Port;
            var address = serviceDetails["AccountService"].Address;
            return "http://" + address + ":" + port + "/";
        }

        public static string GetProductService()
        {
            UrlInitializer();
            var port = serviceDetails["ProductsService"].Port;
            var address = serviceDetails["ProductsService"].Address;
            return "http://" + address + ":" + port + "/";
        }

        public static string GetCartService()
        {
            UrlInitializer();
            var port = serviceDetails["CartAndOrderService"].Port;
            var address = serviceDetails["CartAndOrderService"].Address;
            return "http://" + address + ":" + port + "/";
        }
    }
}
