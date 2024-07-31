using System;
using System.Web;

namespace Excerpta
{
	public class Global : HttpApplication
	{

		void Application_Start(object sender, EventArgs e)
		{
			// Código que se ejecuta al iniciarse la aplicación

		}

		void Application_End(object sender, EventArgs e)
		{
			//  Código que se ejecuta cuando se cierra la aplicación

		}

		void Application_Error(object sender, EventArgs e)
		{
            Response.Redirect("~/Default.aspx");
        }

		void Session_Start(object sender, EventArgs e)
		{
			// Código que se ejecuta cuando se inicia una nueva sesión

		}

		void Session_End(object sender, EventArgs e)
		{
        }

    }
}
