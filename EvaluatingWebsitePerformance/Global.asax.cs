using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using EvaluatingWebsitePerformance.App_Start;
using EvaluatingWebsitePerformance.Bootstrap;

namespace EvaluatingWebsitePerformance
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AutoMapperEntityDtoConfig.RegisterMapping();
            AutoMapperConfig.RegisterMapping();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
