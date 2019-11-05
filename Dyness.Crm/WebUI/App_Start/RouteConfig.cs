using System.Web.Mvc;
using System.Web.Routing;

namespace WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapMvcAttributeRoutes();

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
              name: "Default",
              url: "{controller}/{action}/{id}",
              defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });

            routes.MapRoute(
                name: "404-PageNotFound",
                url: "{*url}",
                defaults: new { controller = "Error", action = "NotFound" });

            routes.MapRoute(
                name: "YetkisizIslem",
                url: "{*url}",
                defaults: new { controller = "Error", action = "YetkisizIslem" });

            routes.MapRoute(
               name: "Hata",
               url: "{*url}",
               defaults: new { controller = "Error", action = "Hata" });
        }
    }
}
