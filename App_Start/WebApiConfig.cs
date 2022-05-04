using System.Web.Http;

namespace DemoTest
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Конфигурация и службы веб-API

            // Маршруты веб-API
            config.EnableCors();
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/getstat/{id}",
                defaults: new { controller = "UCapi", id = RouteParameter.Optional }
            );
        }
    }
}
