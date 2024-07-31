using System;
using System.Web;
using System.Web.UI;
using System.Net.Mail;
using Excerpta.App_Code;

namespace Excerpta
{
    public partial class Registro : Page
    {
        // Cadena de conexión
        private static readonly string CadenaConexion = "Provider = Microsoft.ACE.OLEDB.12.0; Data Source = " + @"c:\inetpub\excerpta\excerpta.accdb;";

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
            Master.navbarLogin_Active          = "";
            Master.navbarRegistro_Active       = "active";
            Master.navbarBienvenida_Active     = "";
            Master.navbarExtractos_Active      = "";
            Master.navbarAdministracion_Active = "";

            // Vaciamos todas las variables de sesión
            Session.RemoveAll();
        }

        /*
        ╔══════════════════╗
        ║     REGISTRO     ║
        ╚══════════════════╝
        */

        // Registrarse
        protected void ButtonRegistro_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            try
            {
                if (new Usuario().ExisteUsuario(InputEmail.Text.Trim()))
                {
                    new Popup(Notificacion, "Ya existe un usuario con ese email");

                    return;
                }

                new Usuario().Crear(InputNombre.Text.Trim(), InputApellidos.Text.Trim(), InputCentro.Text.Trim(), InputTelefono.Text.Trim(), InputEmail.Text.Trim(), InputClave.Text.Trim());

                new Email("Se ha registrado un nuevo usuario", "Se ha registrado un nuevo ususario que queda pendiente de ser dado de alta.");

                VaciarFormulario();

                new Log(InputEmail.Text.Trim(), "REGISTRO").RegistrarActividad();

                new Popup(Notificacion, "Registro completado. Queda pendiente de alta", false);
            }
            catch (Exception Excepcion)
            {
                new Log(InputEmail.Text.Trim(), "EXCEPCIÓN EN REGISTRO", Excepcion.Message).RegistrarActividad();

                new Popup(Notificacion);
            }
        }

        /*
        ╔══════════════════════╗
        ║     VALIDACIONES     ║
        ╚══════════════════════╝
        */

        // Formato email
        protected void ValidarEmail(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            try
            {
                new MailAddress(args.Value);
            }
            catch (FormatException)
            {
                args.IsValid = false;

                new Popup(Notificacion, "El email contiene caracteres inválidos");
            }
        }

        /*
        ╔══════════════════╗
        ║     AUXILIAR     ║
        ╚══════════════════╝
        */

        private void VaciarFormulario()
        {
            InputNombre   .Text = "";
            InputApellidos.Text = "";
            InputCentro   .Text = "";
            InputTelefono .Text = "";
            InputEmail    .Text = "";
            InputClave    .Text = "";
        }
    }
}