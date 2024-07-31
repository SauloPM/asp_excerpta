using System;
using System.Net;
using System.Web;
using System.Web.UI;
using Excerpta.App_Code;
using System.Data.OleDb;
using System.Web.UI.WebControls;

namespace Excerpta
{
    public partial class Florilegios : Page
    {
        private static readonly string CadenaConexion = "Provider = Microsoft.ACE.OLEDB.12.0; Data Source = " + @"c:\inetpub\excerpta\excerpta.accdb;";

        protected void Page_Load(object sender, EventArgs e)
        {
            // Sesión expirada o usuario no registrado
            Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
            if (Session["usuario"] == null)
                Response.Redirect("~/Default.aspx");

            // Administrador
            if (Session["usuario"].ToString() != "gregorio.rodriguez@ulpgc.es")
                Response.Redirect("~/Default.aspx");

            // Eliminamos las notificaciones residuales
            Notificacion.Visible = false;

            if (Page.IsPostBack) return;

            // Ítems visibles del navbar
            Master.navbarLogin_Visible          = false;
            Master.navbarRegistro_Visible       = false;
            Master.navbarLogout_Visible         = true;
            Master.navbarBienvenida_Visible     = true;
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
        ╔═════════════════════╗
        ║     FLORILEGIOS     ║
        ╚═════════════════════╝
        */

        // Recuento
        protected void DataFlorilegios_Selected(object sender, SqlDataSourceStatusEventArgs e)
        {
            RecuentoFlorilegios.Text = e.AffectedRows.ToString() + " florilegios";
        }

        // Seleccionar
        protected void TablaFlorilegios_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Select")
                return;

            try
            {
                // Posición de la fila seleccionada
                int i = Convert.ToInt32(e.CommandArgument);

                // Hemos seleccionado la misma fila que antes
                if (TablaFlorilegios.Rows[i].CssClass == "active")
                {
                    VaciarFormularioFlorilegios();

                    return;
                }

                VaciarFormularioFlorilegios();

                // Resaltamos la fila
                TablaFlorilegios.Rows[i].CssClass = TablaFlorilegios.Rows[i].CssClass = "active";

                // Rellenamos el formulario
                TituloFlorilegio.Text = WebUtility.HtmlDecode(TablaFlorilegios.Rows[i].Cells[0].Text).Trim();
                InputFlorilegio .Text = WebUtility.HtmlDecode(TablaFlorilegios.Rows[i].Cells[0].Text).Trim();

                FlorilegioSeleccionado.Text = InputFlorilegio.Text;

                // Activamos los botones
                BotonAbrirFlorilegio   .Enabled = true;
                BotonEliminarFlorilegio.Enabled = true;

                // Refrescamos
                DataRegistros.SelectCommand = "SELECT * FROM Descripciones WHERE Florilegio = '" + FlorilegioSeleccionado.Text + "' ORDER BY Autor, Obra";

                DataRegistros .DataBind();
                TablaRegistros.DataBind();

                UpdateTablaRegistros.Update();
            }
            catch (Exception Excepcion)
            {
                VaciarFormularioFlorilegios();

                new Log(Session["usuario"].ToString(), "EXCEPCIÓN EN SELECCIONAR FLORILEGIO", Excepcion.Message).RegistrarActividad();

                new Popup(Notificacion, "No se ha podido cargar el elemento seleccionado");
            }
        }

        // Crear / Modificar
        protected void ButtonGuardarFlorilegio_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            try
            {
                // Crear
                if (TituloFlorilegio.Text == "")
                {
                    new Florilegio().Crear(InputFlorilegio.Text.Trim());

                    new Log(Session["usuario"].ToString(), "HA CREADO EL FLORILEGIO '" + InputFlorilegio.Text + "'").RegistrarActividad();
                }

                // Editar
                else
                {
                    new Florilegio().Modificar(TituloFlorilegio.Text.Trim(), InputFlorilegio.Text.Trim());

                    new Log(Session["usuario"].ToString(), "HA MODIFICADO EL TÍTULO DEL FLORILEGIO DE '" + TituloFlorilegio.Text + "' a '" + InputFlorilegio.Text + "'").RegistrarActividad();
                }

                VaciarFormularioFlorilegios();

                new Popup(Notificacion, "Operación completada", false);
            }
            catch (Exception Excepcion)
            {
                new Log(Session["usuario"].ToString(), "EXCEPCIÓN EN GUARDAR EL FLORILEGIO '" + InputFlorilegio.Text + "'", Excepcion.Message).RegistrarActividad();

                new Popup(Notificacion);
            }
        }

        // Eliminar
        protected void ButtonEliminarFlorilegio_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            try
            {
                new Florilegio().Eliminar(TituloFlorilegio.Text.Trim());

                VaciarFormularioFlorilegios();

                new Log(Session["usuario"].ToString(), "HA ELIMINADO EL FLORILEGIO '" + TituloFlorilegio.Text + "'").RegistrarActividad();

                new Popup(Notificacion, "Operación completada", false);
            }
            catch (Exception Excepcion)
            {
                new Log(Session["usuario"].ToString(), "EXCEPCIÓN EN ELIMINAR EL FLORILEGIO '" + InputFlorilegio.Text + "'", Excepcion.Message).RegistrarActividad();

                new Popup(Notificacion);
            }
        }

        // Abrir
        protected void BotonAbrirFlorilegio_Click(object sender, EventArgs e)
        {
            PanelFlorilegios.Visible = false;
            PanelRegistros  .Visible = true;

            UpdatePanelRegistros.Update();
        }

        /*
        ╔═══════════════════╗
        ║     REGISTROS     ║
        ╚═══════════════════╝
        */

        // Recuento
        protected void DataRegistros_Selected(object sender, SqlDataSourceStatusEventArgs e)
        {
            RecuentoRegistros.Text = e.AffectedRows.ToString() + " registros";
        }

        // Seleccionar
        protected void TablaRegistros_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Select")
                return;

            try
            {
                // Posición de la fila seleccionada
                int i = Convert.ToInt32(e.CommandArgument);

                // Hemos seleccionado la misma fila que antes
                if (TablaRegistros.Rows[i].CssClass == "active")
                {
                    VaciarFormularioRegistros();

                    return;
                }

                VaciarFormularioRegistros();

                // Resaltamos la fila
                TablaRegistros.Rows[i].CssClass = TablaRegistros.Rows[i].CssClass = "active";

                // Rellenamos el formulario
                InputAutor.Text = WebUtility.HtmlDecode(TablaRegistros.Rows[i].Cells[1].Text).Trim();
                InputObra .Text = WebUtility.HtmlDecode(TablaRegistros.Rows[i].Cells[2].Text).Trim();
                InputLibro.Text = WebUtility.HtmlDecode(TablaRegistros.Rows[i].Cells[3].Text).Trim();
                InputPoema.Text = WebUtility.HtmlDecode(TablaRegistros.Rows[i].Cells[4].Text).Trim();

                RegistroSeleccionado.Text = WebUtility.HtmlDecode(TablaRegistros.Rows[i].Cells[0].Text).Trim();

                // Activamos los botones
                BotonEliminarRegistro.Enabled = true;

                UpdateTablaRegistros.Update();
            }
            catch (Exception Excepcion)
            {
                VaciarFormularioFlorilegios();

                new Log(Session["usuario"].ToString(), "EXCEPCIÓN EN SELECCIONAR REGISTRO DE FLORILEGIO", Excepcion.Message).RegistrarActividad();

                new Popup(Notificacion, "No se ha podido cargar el elemento seleccionado");
            }
        }

        // Crear / Modificar
        protected void ButtonGuardarRegistro_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            try
            {
                // Crear
                if (RegistroSeleccionado.Text == "")
                {
                    new Estructura().Crear(FlorilegioSeleccionado.Text, InputAutor.Text.Trim(), InputObra .Text.Trim(), InputLibro.Text, InputPoema.Text);

                    new Log(Session["usuario"].ToString(), "HA CREADO UNA ESTRUCTURA PARA EL FLORILEGIO '" + FlorilegioSeleccionado.Text + "'").RegistrarActividad();
                }

                // Modificar
                else
                {
                    new Estructura().Modificar(RegistroSeleccionado.Text.Trim(), FlorilegioSeleccionado.Text.Trim(), InputAutor.Text.Trim(), InputObra.Text.Trim(), InputLibro.Text, InputPoema.Text);

                    new Log(Session["usuario"].ToString(), "HA MODIFICADO UNA ESTRUCTURA PARA EL FLORILEGIO '" + FlorilegioSeleccionado.Text + "'").RegistrarActividad();
                }

                VaciarFormularioRegistros();

                DataRegistros.SelectCommand = "SELECT * FROM Descripciones WHERE Florilegio = '" + FlorilegioSeleccionado.Text + "' ORDER BY Autor, Obra";

                DataRegistros .DataBind();
                TablaRegistros.DataBind();

                UpdateTablaRegistros.Update();

                new Popup(Notificacion, "Operación completada", false);
            }
            catch (Exception Excepcion)
            {
                new Log(Session["usuario"].ToString(), "EXCEPCIÓN EN GUARDAR UNa ESTRUCTURA DEL FLORILEGIO '" + FlorilegioSeleccionado.Text + "'", Excepcion.Message).RegistrarActividad();

                new Popup(Notificacion);
            }
        }

        // Eliminar
        protected void ButtonEliminarRegistro_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            try
            {
                new Estructura().Eliminar(RegistroSeleccionado.Text.Trim());

                VaciarFormularioRegistros();

                DataRegistros.SelectCommand = "SELECT * FROM Descripciones WHERE Florilegio = '" + FlorilegioSeleccionado.Text + "' ORDER BY Autor, Obra";

                DataRegistros .DataBind();
                TablaRegistros.DataBind();

                UpdateTablaRegistros.Update();

                new Log(Session["usuario"].ToString(), "HA ELIMINADO UNA ESTRUCTURA").RegistrarActividad();

                new Popup(Notificacion, "Operación completada", false);
            }
            catch (Exception Excepcion)
            {
                new Log(Session["usuario"].ToString(), "EXCEPCIÓN EN ELIMINAR ESTRUCTURA", Excepcion.Message).RegistrarActividad();

                new Popup(Notificacion);
            }
        }

        // Volver
        protected void ButtonVolver_Click(object sender, EventArgs e)
        {
            PanelFlorilegios.Visible = true;
            PanelRegistros  .Visible = false;

            VaciarFormularioFlorilegios();

            UpdatePanelRegistros.Update();
        }

        /*
        ╔══════════════════════╗
        ║     VALIDACIONES     ║
        ╚══════════════════════╝
        */

        // Florilegio repetido
        protected void ValidarFlorilegioRepetido(object source, ServerValidateEventArgs args)
        {
            try
            {
                if (new Florilegio().ExisteFlorilegio(InputFlorilegio.Text.Trim()))
                {
                    args.IsValid = false;

                    new Popup(Notificacion, "Ya existe un florilegio con ese nombre");
                }
            }
            catch(Exception Excepcion)
            {
                args.IsValid = false;

                new Log(Session["usuario"].ToString(), "EXCEPCIÓN EN VALIDACIÓN", Excepcion.Message).RegistrarActividad();
            }
        }

        // Florilegio vacío
        protected void ValidarFlorilegioVacio(object source, ServerValidateEventArgs args)
        {
            try
            {
                if (new Florilegio().ContieneExtractos(InputFlorilegio.Text.Trim()))
                {
                    args.IsValid = false;

                    new Popup(Notificacion, "Hay extractos asociados a ese florilegio");
                }

                if (new Florilegio().ContieneRegistros(InputFlorilegio.Text.Trim()))
                {
                    args.IsValid = false;

                    new Popup(Notificacion, "Hay registros asociados a ese florilegio");
                }
            }
            catch(Exception Excepcion)
            {
                args.IsValid = false;

                new Log(Session["usuario"].ToString(), "EXCEPCIÓN EN VALIDACIÓN", Excepcion.Message).RegistrarActividad();
            }
        }

        // Estructura repetida
        protected void ValidarEstructuraRepetida(object source, ServerValidateEventArgs args)
        {
            try
            {
                if (new Estructura().ExisteEstructura(FlorilegioSeleccionado.Text.Trim(), InputAutor.Text.Trim(), InputObra.Text.Trim()))
                {
                    args.IsValid = false;

                    new Popup(Notificacion, "Ya existe un registro con esas propiedades");
                }
            }
            catch (Exception Excepcion)
            {
                args.IsValid = false;

                new Log(Session["usuario"].ToString(), "EXCEPCIÓN EN VALIDACIÓN", Excepcion.Message).RegistrarActividad();
            }
        }

        // Estructura vacía
        protected void ValidarEstructuraVacia(object source, ServerValidateEventArgs args)
        {
            try
            {
                if (new Estructura().ContieneExtractos(FlorilegioSeleccionado.Text.Trim(), InputAutor.Text.Trim(), InputObra.Text.Trim()))
                {
                    args.IsValid = false;

                    new Popup(Notificacion, "Hay extractos asociados a ese registro");
                }
            }
            catch(Exception Excepcion)
            {
                args.IsValid = false;

                new Log(Session["usuario"].ToString(), "EXCEPCIÓN EN VALIDACIÓN", Excepcion.Message).RegistrarActividad();
            }
        }

        /*
        ╔══════════════════╗
        ║     AUXILIAR     ║
        ╚══════════════════╝
        */

        private void VaciarFormularioFlorilegios()
        {
            // Vaciamos el formulario
            TituloFlorilegio.Text = "";
            InputFlorilegio .Text = "";

            // Desactivamos algunos botones
            BotonAbrirFlorilegio   .Enabled = false;
            BotonEliminarFlorilegio.Enabled = false;

            // Desactivamos todas las filas
            foreach (GridViewRow Row in TablaFlorilegios.Rows)
                Row.CssClass = "";

            // Refrescamos
            DataFlorilegios .DataBind();
            TablaFlorilegios.DataBind();

            DataRegistros.SelectCommand = "";

            DataRegistros .DataBind();
            TablaRegistros.DataBind();
        }

        private void VaciarFormularioRegistros()
        {
            // Vaciamos el formulario
            InputAutor.Text = "";
            InputObra .Text = "";
            InputLibro.Text = "";
            InputPoema.Text = "";
            RegistroSeleccionado.Text = "";

            // Desactivamos el botón "Eliminar"
            BotonEliminarRegistro.Enabled = false;

            // Desactivamos todas las filas
            foreach (GridViewRow Row in TablaRegistros.Rows)
                Row.CssClass = "";
        }
    }
}