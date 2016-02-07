using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace PhotoAlbum
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError() as HttpException;
            Server.ClearError();
            var urlHelper = new UrlHelper(Request.RequestContext);

            switch (exception.ErrorCode)
            {
                case 404:
                    Response.Redirect(urlHelper.Action("NotFound", "Error"));
                    break;
                default:
                    Response.Redirect(urlHelper.Action("Error", "Error"));
                    break;
            }
        }
    }
}