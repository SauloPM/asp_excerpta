using System;
using System.Web.UI;
using Excerpta.App_Code;
using System.Web.UI.WebControls;
using System.Net;

namespace Excerpta
{
    public partial class Bibliografia : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Sesión expirada o usuario no registrado
            Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
            if (Session["usuario"] == null)
                Response.Redirect("~/Default.aspx");

            // Administrador
            if (Session["usuario"].ToString() != "gregorio.rodriguez@ulpgc.es")
                Response.Redirect("~/Default.aspx");

            Notificacion.Visible = false;

            if (Page.IsPostBack) return;

            // Ítems visibles del navbar
            Master.navbarLogin_Visible          = false;
            Master.navbarRegistro_Visible       = false;
            Master.navbarLogout_Visible         = true;
            Master.navbarExtractos_Visible      = true;
            Master.navbarAdministracion_Visible = true;

            // Ítem activo del navbar
            Master.navbarLogin_Active          = "";
            Master.navbarRegistro_Active       = "";
            Master.navbarBienvenida_Active     = "";
            Master.navbarExtractos_Active      = "";
            Master.navbarAdministracion_Active = "active";
        }

        /*
        ╔══════════════════════╗
        ║     BIBLIOGRAFÍA     ║
        ╚══════════════════════╝
        */

        // Recuento
        protected void DataBiliografia_Selected(object sender, SqlDataSourceStatusEventArgs e)
        {
            Recuento.Text = e.AffectedRows.ToString() + " fuente(s) bibliográfica(s)";
        }

        // Seleccionar
        protected void TablaBibliografia_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Select")
                return;

            try
            {
                // Posición de la fila seleccionada
                int i = Convert.ToInt32(e.CommandArgument);

                // Hemos seleccionado la misma fila que antes
                if (TablaBibliografia.Rows[i].CssClass == "active")
                {
                    VaciarFormulario();

                    return;
                }

                VaciarFormulario();

                // Resaltamos la fila
                TablaBibliografia.Rows[i].CssClass = TablaBibliografia.Rows[i].CssClass = "active";

                // Rellenamos el formulario
                InputID          .Text = WebUtility.HtmlDecode(TablaBibliografia.Rows[i].Cells[0].Text).Trim();
                InputBibliografia.Text = WebUtility.HtmlDecode(TablaBibliografia.Rows[i].Cells[1].Text).Trim();

                // Activamos los botones
                ButtonEliminar.Enabled = true;

                // Refrescamos
                UpdateTabla.Update();
            }
            catch (Exception Excepcion)
            {
                VaciarFormulario();

                new Log(Session["usuario"].ToString(), "EXCEPCIÓN EN SELECCIONAR USUARIO", Excepcion.Message).RegistrarActividad();

                new Popup(Notificacion, "No se ha podido cargar el elemento seleccionado");
            }
        }

        // Guardar
        protected void BotonGuardarBibliografia_Click(object sender, EventArgs e)
        {
            try
            {
                // Crear fuente bibliográfica
                if (InputID.Text == "")
                {
                    new FuenteBibliografica().Crear(InputBibliografia.Text.Trim());

                    new Log(Session["usuario"].ToString(), "CREAR FUENTE BIBLIOGRÁFICA '").RegistrarActividad();
                }

                // Editar fuente bibliográfica
                else
                {
                    new FuenteBibliografica().Modificar(InputID.Text.Trim(), InputBibliografia.Text.Trim());

                    new Log(Session["usuario"].ToString(), "MODIFICAR FUENTE BIBLIOGRÁFICA").RegistrarActividad();
                }

                VaciarFormulario();

                new Popup(Notificacion, "Operación completada", false);
            }
            catch (Exception Excepcion)
            {
                new Log(Session["usuario"].ToString(), "EXCEPCIÓN EN GUARDAR FUENTE BIBLIOGRÁFICA", Excepcion.Message).RegistrarActividad();

                new Popup(Notificacion);
            }
        }

        // Eliminar
        protected void BotonEliminarBibliografia_Click(object sender, EventArgs e)
        {
            try
            {
                new FuenteBibliografica().Eliminar(InputID.Text.Trim());

                VaciarFormulario();

                new Log(Session["usuario"].ToString(), "HA ELIMINADO UNA FUENTE BIBLIOGRÁFICA").RegistrarActividad();

                new Popup(Notificacion, "Operación completada", false);
            }
            catch (Exception Excepcion)
            {
                new Log(Session["usuario"].ToString(), "EXCEPCIÓN EN ELIMINAR UNA FUENTE BIBLIOGRÁFICA", Excepcion.Message).RegistrarActividad();

                new Popup(Notificacion);
            }
        }

        /*
        ╔══════════════════╗
        ║     AUXILIAR     ║
        ╚══════════════════╝
        */

        private void VaciarFormulario()
        {
            // Vaciamos el formulario
            InputID.Text           = "";
            InputBibliografia.Text = "";

            // Desactivamos el botón de eliminar
            ButtonEliminar.Enabled = false;

            // Desactivamos todas las filas
            foreach (GridViewRow Row in TablaBibliografia.Rows)
                Row.CssClass = "";

            // Refrescamos
            DataBibliografia .DataBind();
            TablaBibliografia.DataBind();

            UpdateTabla.Update();
        }
    }
}