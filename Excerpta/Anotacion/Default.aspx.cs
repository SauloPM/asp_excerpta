using System;
using System.Web.UI;
using Excerpta.App_Code;

namespace Excerpta
{
    public partial class Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Ítems visibles del navbar
            Master.navbarLogin_Visible          = true;
            Master.navbarRegistro_Visible       = true;
            Master.navbarLogout_Visible         = false;
            Master.navbarBienvenida_Visible     = false;
            Master.navbarExtractos_Visible      = false;
            Master.navbarAdministracion_Visible = false;

            // Ítem activo del navbar
            Master.navbarLogin_Active          = "active";
            Master.navbarRegistro_Active       = "";
            Master.navbarBienvenida_Active     = "";
            Master.navbarExtractos_Active      = "";
            Master.navbarAdministracion_Active = "";

            // Vaciamos todas las variables de sesión
            Session.RemoveAll();
        }

        /*
        ╔═══════════════╗
        ║     LOGIN     ║
        ╚═══════════════╝
        */

        // Procedimiento que permite al usuario iniciar sesión
        protected void ButtonLogin_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            try
            {
                if (!new Usuario().ComprobarDatos(InputEmail.Text.Trim(), InputClave.Text.Trim()))
                {
                    new Popup(Notificacion, "Datos de acceso incorrectos");

                    return;
                }

                Session["usuario"] = InputEmail.Text.Trim();

                new Log(Session["usuario"].ToString(), "LOGIN").RegistrarActividad();

                // Vaciamos el formulario
                InputEmail.Text = "";
                InputClave.Text = "";

                Response.Redirect("~/Bienvenida.aspx", false);
            }
            catch (Exception Excepcion)
            {
                new Log(Session["usuario"].ToString(), "EXCEPCIÓN EN LOGIN", Excepcion.Message).RegistrarActividad();

                new Popup(Notificacion, "Se ha producido un error");

                Session.RemoveAll();
            }
        }
    }
}