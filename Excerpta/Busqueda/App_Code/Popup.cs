using System.Web.UI.WebControls;

namespace Busqueda.App_Code
{
    public class Popup
    {
        // Atributos
        Label Icono        = new Label();
        Label Mensaje      = new Label();
        Panel Notificacion = new Panel();

        /*
        ╔═════════════════╗
        ║     MÉTODOS     ║
        ╚═════════════════╝
        */

        // Constructor
        public Popup (Panel Notificacion, string Mensaje = "Se ha producido un error", bool error = true)
        {
            this.Mensaje.Text = Mensaje;
            this.Notificacion = Notificacion;

            if (error)
            {
                Icono.Text  = "<span class='icon'><i class='fa fa-fw fa-times' aria-hidden='true'></i></span>";

                Notificacion.CssClass = "notificacion error";
            }
            else
            {
                Icono.Text  = "<span class='icon'><i class='fa fa-fw fa-check' aria-hidden='true'></i></span>";

                Notificacion.CssClass = "notificacion success";
            }

            this.Notificacion.Controls.Clear();

            this.Notificacion.Controls.Add(Icono);
            this.Notificacion.Controls.Add(this.Mensaje);

            Notificacion.Visible = true;
        }
    }
}