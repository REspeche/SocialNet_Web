using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using Web.CommonApi;
using Web.Filters;

namespace Web.App_Start
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.Filters.Add(new UnhandledExceptionFilter());

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "ApiById",
                routeTemplate: "api/{action}/{id}",
                defaults: new { controller = "ApiDefault", id = RouteParameter.Optional },
                constraints: new { id = @"^[0-9]+$" }
            );

            config.Routes.MapHttpRoute(
                name: "ApiByName",
                routeTemplate: "api/{action}/{name}",
                defaults: null,
                constraints: new { controller = "ApiDefault", name = @"^[a-z,0-9]+$" }
            );

            config.Routes.MapHttpRoute(
                name: "ApiByAction",
                routeTemplate: "api/{action}",
                defaults: new { controller = "ApiDefault", action = "GetStatus" }
            );

            // Remove the XML formatter
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            // Compression
            config.MessageHandlers.Insert(0, new CompressionHandler());
            var httpClient = HttpClientFactory.Create(new DecompressionHandler());
            httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
        }
    }
}