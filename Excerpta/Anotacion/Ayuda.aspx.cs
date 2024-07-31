using System;
using System.Web.UI;

namespace Excerpta
{
    public partial class Ayuda : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Sesión expirada o usuario no registrado
            Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
            if (Session["usuario"] == null)
                Response.Redirect("~/Default.aspx");

            // Ítems visibles del navbar
            Master.navbarLogin_Visible          = false;
            Master.navbarRegistro_Visible       = false;
            Master.navbarLogout_Visible         = true;
            Master.navbarBienvenida_Visible     = true;
            Master.navbarExtractos_Visible      = true;
            Master.navbarAdministracion_Visible = (Session["usuario"].ToString() == "gregorio.rodriguez@ulpgc.es") ? true : false;

            // Ítem activo del navbar
            Master.navbarLogin_Active          = "";
            Master.navbarRegistro_Active       = "";
            Master.navbarBienvenida_Active     = "";
            Master.navbarExtractos_Active      = "active";
            Master.navbarAdministracion_Active = "";
        }
    }
}