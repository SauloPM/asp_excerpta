using System;
using System.Web.UI;
using System.Net.Mail;
using Excerpta.App_Code;
using System.Web.UI.WebControls;

namespace Excerpta
{
    public partial class Cuenta : Page
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
        ╔═══════════════════════╗
        ║     CAMBIAR CLAVE     ║
        ╚═══════════════════════╝
        */

        protected void ButtonCambiarClave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsValid)
                    return;

                if (!new Usuario().ComprobarDatos(InputEmailCambiarClave.Text.Trim(), InputClaveActualCambiarClave.Text.Trim()))
                {
                    new Popup(Notificacion, "Datos de acceso incorrectos");

                    return;
                }

                new Usuario().CambiarClave(InputEmailCambiarClave.Text.Trim(), InputClaveNueva1CambiarClave.Text.Trim());

                // Vaciamos el formulario
                InputEmailCambiarClave.Text       = "";
                InputClaveActualCambiarClave.Text = "";
                InputClaveNueva1CambiarClave.Text = "";
                InputClaveNueva2CambiarClave.Text = "";

                new Log(InputEmailCambiarClave.Text, "CAMBIAR CLAVE").RegistrarActividad();

                new Popup(Notificacion, "Operación completada", false);
            }
            catch (Exception Excepcion)
            {
                new Log(InputEmailCambiarClave.Text.Trim(), "EXCEPCIÓN EN CAMBIAR CLAVE", Excepcion.Message).RegistrarActividad();

                new Popup(Notificacion);
            }
            finally
            {
                UpdateNotificacion.Update();
            }
        }

        /*
        ╔════════════════════════╗
        ║     RECORDAR CLAVE     ║
        ╚════════════════════════╝
        */

        protected void ButtonRecordarClave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsValid)
                    return;

                string Clave = new Usuario().ObtenerClave(InputEmailRecordarClave.Text);

                if (Clave == "")
                {
                    new Popup(Notificacion, "No existe ningún usuario con ese email");

                    return;
                }

                // Notificamos al usuario
                new Email("Recuperación de clave de acceso", "Su clave de acceso del panel de administraci&oacute; del Proyecto Excerpta es '" + Clave + "'", InputEmailRecordarClave.Text).EnviarEmail();

                // Vaciamos el formulario
                InputEmailRecordarClave.Text = "";

                new Log(InputEmailRecordarClave.Text, "RECORDAR CLAVE").RegistrarActividad();

                new Popup(Notificacion, "Se ha enviado un email con su clave de acceso", false);
            }
            catch (Exception Excepcion)
            {
                new Log(InputEmailRecordarClave.Text, "EXCEPCIÓN EN RECORDAR CLAVE", Excepcion.Message).RegistrarActividad();

                new Popup(Notificacion);
            }
            finally
            {
                UpdateNotificacion.Update();
            }
        }

        /*
        ╔═════════════════════════╗
        ║     ELIMINAR CUENTA     ║
        ╚═════════════════════════╝
        */

        protected void ButtonEliminarCuenta_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsValid)
                    return;

                if (!new Usuario().ComprobarDatos(InputEmailEliminarCuenta.Text.Trim(), InputClaveEliminarCuenta.Text.Trim()))
                {
                    new Popup(Notificacion, "Datos de acceso incorrectos");

                    return;
                }

                new Usuario().Eliminar(InputEmailEliminarCuenta.Text.Trim());

                // Vaciamos el formulario
                InputEmailEliminarCuenta.Text = "";
                InputClaveEliminarCuenta.Text = "";

                new Log(InputEmailEliminarCuenta.Text, "ELIMINAR CUENTA").RegistrarActividad();

                new Popup(Notificacion, "Operación completada", false);
            }
            catch (Exception Excepcion)
            {
                new Log(InputEmailEliminarCuenta.Text, "EXCEPCIÓN EN ELIMINAR CUENTA", Excepcion.Message).RegistrarActividad();

                new Popup(Notificacion);
            }
            finally
            {
                UpdateNotificacion.Update();
            }
        }

        /*
        ╔══════════════════════╗
        ║     VALIDACIONES     ║
        ╚══════════════════════╝
        */

        // Formato email
        protected void ValidarEmail(object source, ServerValidateEventArgs args)
        {
            try
            {
                MailAddress m = new MailAddress(args.Value);

                args.IsValid = true;
            }
            catch (FormatException)
            {
                args.IsValid = false;

                new Popup(Notificacion, "El email contiene caracteres inválidos");
            }
        }

        // La clave actual debe ser distinta de la nueva
        protected void ValidarClavesDistintas(object source, ServerValidateEventArgs args)
        {
            if (InputClaveActualCambiarClave.Text.Trim() == InputClaveNueva1CambiarClave.Text.Trim())
            {
                args.IsValid = false;

                new Popup(Notificacion, "La clave de acceso actual no puede ser igual que la nueva");
            }
        }

        // Las dos claves nuevas deben coincidir
        protected void ValidarClavesIguales(object source, ServerValidateEventArgs args)
        {
            if (InputClaveNueva1CambiarClave.Text != InputClaveNueva2CambiarClave.Text)
            {
                args.IsValid = false;

                new Popup(Notificacion, "Las claves de acceso nuevas deben coincidir");
            }
        }

        // El administrador no puede eliminar su cuenta
        protected void ValidarAdministrador(object source, ServerValidateEventArgs args)
        {
            if (InputEmailEliminarCuenta.Text == "gregorio.rodriguez@ulpgc.es")
            {
                args.IsValid = false;

                new Popup(Notificacion, "El administrador no puede eliminar su cuenta");
            }
        }
    }
}