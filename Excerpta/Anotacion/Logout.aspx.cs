using System;
using System.Web.UI;
using Excerpta.App_Code;

namespace Excerpta
{
    public partial class Logout : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            new Log(Session["usuario"].ToString(), "LOGOUT").RegistrarActividad();

            Session.RemoveAll();

            Response.Redirect("~/Default.aspx");
        }
    }
}