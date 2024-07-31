using System;
using System.Web;
using System.Linq;
using System.Web.Security;
using System.Web.Routing;
using System.Web.SessionState;
using System.Web.Optimization;
using System.Collections.Generic;

namespace Busqueda
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Código que se ejecuta al iniciar la aplicación
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        void Application_Error(object sender, EventArgs e)
        {
            Response.Redirect("~/Default.aspx");
        }

    }
}