using System;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Drawing;
using System.Data.OleDb;
using Excerpta.App_Code;
using System.IO.Compression;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Web.UI.DataVisualization.Charting;

namespace Excerpta
{
    public partial class Extractos : Page
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

            // Ocultamos las notificaciones residuales
            Notificacion.Visible = false;

            if (Page.IsPostBack) return;

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
            Master.navbarExtractos_Active      = "";
            Master.navbarAdministracion_Active = "active";

            GenerarFiltro();

            GenerarGrafico();
        }

        /*
        ╔════════════════╗
        ║     FILTRO     ║
        ╚════════════════╝
        */

        protected void DropFiltroUsuarios_SelectedIndexChanged(object sender, EventArgs e)
        {
            SincronizarFiltroFlorilegios();
        }

        protected void DropFiltroFlorilegios_SelectedIndexChanged(object sender, EventArgs e)
        {
            SincronizarFiltroAutores();
        }

        protected void DropFiltroAutores_SelectedIndexChanged(object sender, EventArgs e)
        {
            SincronizarFiltroObras();
        }

        protected void DropFiltroObras_SelectedIndexChanged(object sender, EventArgs e)
        {
            ActualizarTablaExtractos();
        }

        protected void DropFiltroEstados_SelectedIndexChanged(object sender, EventArgs e)
        {
            ActualizarTablaExtractos();
        }

        protected void InputID_TextChanged(object sender, EventArgs e)
        {
            ActualizarTablaExtractos();
        }

        /*
        ╔═══════════════════╗
        ║     EXTRACTOS     ║
        ╚═══════════════════╝
        */

        // Recuento
        protected void DataExtractos_Selected(object sender, SqlDataSourceStatusEventArgs e)
        {
            RecuentoExtractos.Text = e.AffectedRows.ToString() + " extracto(s)";

            // Mostramos el paginador solo cuando sea necesario
            TablaExtractos.AllowPaging = e.AffectedRows > 500 ? true : false;
            TablaExtractos.CssClass    = e.AffectedRows > 500 ? "margin-bottom-50" : ""; // Dejamos espacio para el paginador
        }

        // Formato
        protected void TablaExtractos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                try
                {
                    // Representamos el estado del alta con iconos
                    e.Row.Cells[0].Text = e.Row.Cells[0].Text == "True" ? "<i class='fa fa-fw fa-check' aria-hidden='true'></i>" : "<i class='fa fa-fw fa-times' aria-hidden='true'></i>";

                    // Acortamos los campos demasiado largos
                    if (e.Row.Cells[ 7].Text.Length > 50) e.Row.Cells[ 7].Text = e.Row.Cells[ 7].Text.Substring(0, 50) + "...";
                    if (e.Row.Cells[ 8].Text.Length > 50) e.Row.Cells[ 8].Text = e.Row.Cells[ 8].Text.Substring(0, 50) + "...";
                    if (e.Row.Cells[11].Text.Length > 50) e.Row.Cells[11].Text = e.Row.Cells[11].Text.Substring(0, 50) + "...";
                    if (e.Row.Cells[12].Text.Length > 50) e.Row.Cells[12].Text = e.Row.Cells[12].Text.Substring(0, 50) + "...";
                }
                catch
                {

                }
            }
        }

        // Paginador / Seleccionar
        protected void TablaExtractos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            // Paginador
            if (e.CommandName != "Select")
            {
                ActualizarTablaExtractos();

                return;
            }

            // Seleccionar
            try
            {
                // Posición de la fila seleccionada
                int i = Convert.ToInt32(e.CommandArgument);

                // Hemos seleccionado la misma fila que antes
                if (TablaExtractos.Rows[i].CssClass == "active")
                {
                    TablaExtractos.Rows[i].CssClass = "";

                    ExtractosSeleccionados.Text = ExtractosSeleccionados.Text.Replace(TablaExtractos.Rows[i].Cells[1].Text + ",", "");

                    UpdateTabla.Update();

                    // Desactivamos los botones si no hubiera ninguna fila seleccionada
                    if (ExtractosSeleccionados.Text.Trim() == "")
                    {
                        ButtonPublicar   .Enabled = false;
                        ButtonDespublicar.Enabled = false;
                    }

                    return;
                }

                // Resaltamos la fila
                TablaExtractos.Rows[i].CssClass = TablaExtractos.Rows[i].CssClass = "active";

                // Obtenemos el ID del extracto seleccionado
                ExtractosSeleccionados.Text += TablaExtractos.Rows[i].Cells[1].Text.Trim() + ",";

                // Desbloqueamos los botones
                ButtonPublicar   .Enabled = true;
                ButtonDespublicar.Enabled = true;

                UpdateTabla.Update();
            }
            catch (Exception Excepcion)
            {
                VaciarFormulario();

                new Log(Session["usuario"].ToString(), "EXCEPCIÓN EN SELECCIONAR EXTRACTO", Excepcion.Message).RegistrarActividad();

                new Popup(Notificacion, "No se ha podido cargar el elemento seleccionado");
            }
        }

        // Seleccionar todos los extractos de la tabla (pendiente)
        protected void ButtonMarcarTodos_Click(object sender, EventArgs e)
        {
            try
            {
                // Reseteamos para evitar repeticiones si hacemos clic varias veces en "Marcar todo"
                ExtractosSeleccionados.Text = "";

                // Activamos todas las filas y completamos el formulario
                foreach (GridViewRow Row in TablaExtractos.Rows)
                {
                    Row.CssClass = "active";

                    ExtractosSeleccionados.Text += Row.Cells[1].Text.Trim() + ",";
                }

                // Refrescamos
                UpdateTabla.Update();

                // Activamos o desactivamos los botones
                ButtonPublicar   .Enabled = ExtractosSeleccionados.Text.Trim() != "" ? true : false;
                ButtonDespublicar.Enabled = ExtractosSeleccionados.Text.Trim() != "" ? true : false;
            }
            catch (Exception Excepcion)
            {
                VaciarFormulario();

                new Log(Session["usuario"].ToString(), "EXCEPCIÓN EN SELECCIONAR EXTRACTO", Excepcion.Message).RegistrarActividad();

                new Popup(Notificacion, "No se ha podido cargar el elemento seleccionado");
            }
        }

        // Deseleccionar todos los extractos de la tabla
        protected void ButtonDesmarcarTodos_Click(object sender, EventArgs e)
        {
            VaciarFormulario();
        }

        // Publicar extracto
        protected void ButtonPublicarExtracto_Click(object sender, EventArgs e)
        {
            try
            {
                new Extracto().Publicar(ExtractosSeleccionados.Text.Trim());

                ActualizarTablaExtractos();

                // Guardamos la fecha de esta, la última actualización (esta fecha será utilizado por la app de búsqueda)
                File.WriteAllText(@"C:\inetpub\ExcerptaLastUpdate\fecha-ultima-actualizacion.txt", DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString());

                new Log("gregorio.rodriguez@ulpgc.es", "HA PUBLICADO UNO O VARIOS EXTRACTOS").RegistrarActividad();

                new Popup(Notificacion, "Operación completada", false);
            }
            catch (Exception Excepcion)
            {
                new Log(Session["usuario"].ToString(), "EXCEPCIÓN EN PUBLICAR EXTRACTO", Excepcion.Message).RegistrarActividad();

                new Popup(Notificacion, Excepcion.Message);
            }
        }

        // Despublicar extracto
        protected void ButtonDespublicarExtracto_Click(object sender, EventArgs e)
        {
            try
            {
                new Extracto().Despublicar(ExtractosSeleccionados.Text.Trim());

                ActualizarTablaExtractos();

                new Log("gregorio.rodriguez@ulpgc.es", "HA DESPUBLICADO UNO O VARIOS EXTRACTOS").RegistrarActividad();

                new Popup(Notificacion, "Operación completada", false);
            }
            catch (Exception Excepcion)
            {
                new Log(Session["usuario"].ToString(), "EXCEPCIÓN EN DESPUBLICAR EXTRACTO", Excepcion.Message).RegistrarActividad();

                new Popup(Notificacion, Excepcion.Message);
            }
        }

        /*
        ╔═════════════════╗
        ║     BACKUPS     ║
        ╚═════════════════╝
        */

        // Descargar backup
        protected void BotonDescargarBackup_Click(object sender, EventArgs e)
        {
            try
            {
                new Backup().Descargar();

                // Comprimimos el subdirectorio en un zip
                ZipFile.CreateFromDirectory(HttpContext.Current.Server.MapPath("~/Backup") + "/florilegios", HttpContext.Current.Server.MapPath("~/Backup") + "/backup.zip");

                // Activamos el enlace de descarga
                LinkDescargarBackup.Attributes.Add("href", "~/Backup/backup.zip");
                LinkDescargarBackup.Attributes.Add("class", "boton");

                new Log(Session["usuario"].ToString(), "DESCARGAR BACKUP").RegistrarActividad();

                new Popup(Notificacion, "Operación completada", false);
            }
            catch (Exception Excepcion)
            {
                new Log(Session["usuario"].ToString(), "EXCEPCIÓN EN DESCARGAR BACKUP", Excepcion.Message).RegistrarActividad();

                new Popup(Notificacion);

                return;
            }
        }

        // Subir backup
        protected void ButtonImportarXML_Click(object sender, EventArgs e)
        {
            try
            {
                // ¿Se ha subido algo?
                if (!BotonOcultoBackup.HasFile)
                {
                    BotonImportarBackup.Enabled = false;

                    new Popup(Notificacion, "Debe subir un fichero");

                    return;
                }

                // ¿La extensión es .xml?
                if (BotonOcultoBackup.FileName.Substring(BotonOcultoBackup.FileName.Length - 4) != ".xml")
                {
                    BotonImportarBackup.Enabled = false;

                    new Popup(Notificacion, "Debe subir un fichero XML");

                    return;
                }

                new Backup().Subir(BotonOcultoBackup);

                ActualizarTablaExtractos();

                new Popup(Notificacion, "Operación completada", false);
            }
            catch (Exception Excepcion)
            {
                new Log(Session["usuario"].ToString(), "EXCEPCIÓN EN SUBIR BACKUP", Excepcion.Message).RegistrarActividad();

                new Popup(Notificacion);
            }
        }

        /*
        ╔══════════════════╗
        ║     AUXILIAR     ║
        ╚══════════════════╝
        */

        private void SincronizarFiltroFlorilegios()
        {
            try
            {
                DropFiltroFlorilegios.Items.Clear();

                DropFiltroFlorilegios.Items.Add(new ListItem { Text = "Todos los florilegios", Value = "%", Selected = true });

                DataFiltroFlorilegios.SelectCommand = "SELECT DISTINCT Florilegio FROM Extractos WHERE Usuario LIKE '" + DropFiltroUsuarios.SelectedValue + "' ORDER BY Florilegio";

                DataFiltroFlorilegios.DataBind();
                DropFiltroFlorilegios.DataBind();

                SincronizarFiltroAutores();
            }
            catch (Exception Excepcion)
            {
                new Log(Session["usuario"].ToString(), "EXCEPCIÓN EN FILTRO", Excepcion.Message).RegistrarActividad();

                new Popup(Notificacion);
            }
        }

        private void SincronizarFiltroAutores()
        {
            try
            {
                DropFiltroAutores.Items.Clear();

                DropFiltroAutores.Items.Add(new ListItem { Text = "Todos los autores", Value = "%", Selected = true });

                DataFiltroAutores.SelectCommand = "SELECT DISTINCT Autor FROM Extractos WHERE Usuario LIKE '" + DropFiltroUsuarios.SelectedValue + "' AND Florilegio LIKE '" + DropFiltroFlorilegios.SelectedValue + "' AND Autor IS NOT NULL ORDER BY Autor";

                DataFiltroAutores.DataBind();
                DropFiltroAutores.DataBind();

                SincronizarFiltroObras();
            }
            catch (Exception Excepcion)
            {
                new Log(Session["usuario"].ToString(), "EXCEPCIÓN EN FILTRO", Excepcion.Message).RegistrarActividad();

                new Popup(Notificacion);
            }
        }

        private void SincronizarFiltroObras()
        {
            try
            {
                DropFiltroObras.Items.Clear();

                DropFiltroObras.Items.Add(new ListItem { Text = "Todas las obras", Value = "%", Selected = true });

                DataFiltroObras.SelectCommand = "SELECT DISTINCT Obra FROM Extractos WHERE Usuario LIKE '" + DropFiltroUsuarios.SelectedValue + "' AND Florilegio LIKE '" + DropFiltroFlorilegios.SelectedValue + "' AND Autor LIKE '" + DropFiltroAutores.SelectedValue + "' AND Obra IS NOT NULL ORDER BY Obra";

                DataFiltroObras.DataBind();
                DropFiltroObras.DataBind();

                ActualizarTablaExtractos();
            }
            catch (Exception Excepcion)
            {
                new Log(Session["usuario"].ToString(), "EXCEPCIÓN EN FILTRO", Excepcion.Message).RegistrarActividad();

                new Popup(Notificacion);
            }
        }

        protected void ActualizarTablaExtractos()
        {
            try
            {
                string FragmentoID     = "";

                foreach (string ID in InputID.Text.Trim().Replace(" ", "").Split(','))
                {
                    if (ID == "")
                        continue;

                    FragmentoID += "ID = " + ID + " OR ";
                }

                FragmentoID = FragmentoID == "" ? "" : "AND ( " + FragmentoID.Substring(0, FragmentoID.Length - 4) + " ) "; // Retiramos el último " OR " de la sentencia SQL

                DataExtractos.SelectCommand  = "SELECT ID, Alta, Usuario, Florilegio, Autor, Obra, Libro, Poema, [Capítulo], [Subcapítulo], VersoInicial, VersoFinal, Extracto, TLL, [Vernácula], [Página] FROM Extractos WHERE Usuario LIKE '" + DropFiltroUsuarios.SelectedValue + "' AND Florilegio LIKE '" + DropFiltroFlorilegios.SelectedValue + "' AND Autor LIKE '" + DropFiltroAutores.SelectedValue + "' AND Obra LIKE '" + DropFiltroObras.SelectedValue + "' AND Alta LIKE '" + DropFiltroEstados.SelectedValue + "' " + FragmentoID + "ORDER BY Florilegio, Autor, Obra, Libro, Poema, [Capítulo], [Subcapítulo]";

                DataExtractos .DataBind();
                TablaExtractos.DataBind();

                UpdateTabla   .Update();
                UpdateRecuento.Update();

                GenerarPDF();

                VaciarFormulario();
            }
            catch (Exception Excepcion)
            {
                new Log(Session["usuario"].ToString(), "EXCEPCIÓN EN FILTRO", Excepcion.Message).RegistrarActividad();

                new Popup(Notificacion);
            }
        }

        private void GenerarPDF()
        {
            try
            {
                new FicheroPDF().Generar(Session["usuario"].ToString(), DataExtractos.SelectCommand, RecuentoExtractos.Text.Trim());

                // Activamos el enlace de descarga
                BotonPDF.Attributes.Add("href", @"~/PDF/" + Session["usuario"] + @"/extractos.pdf");

                new Log(Session["usuario"].ToString(), "GENERAR PDF DE EXTRACTOS").RegistrarActividad();
            }
            catch (Exception Excepcion)
            {
                new Log(Session["usuario"].ToString(), "EXCEPCIÓN EN GENERAR PDF DE EXTRACTOS", Excepcion.Message).RegistrarActividad();

                new Popup(Notificacion, "No se ha podido generar el PDF de extractos");
            }
        }

        protected void GenerarFiltro()
        {
            try
            {
                DropFiltroFlorilegios.Items.Clear();
                DropFiltroAutores    .Items.Clear();
                DropFiltroObras      .Items.Clear();

                DropFiltroFlorilegios.Items.Add(new ListItem { Text = "Todos los florilegios", Value = "%", Selected = true });
                DropFiltroAutores    .Items.Add(new ListItem { Text = "Todos los autores"    , Value = "%", Selected = true });
                DropFiltroObras      .Items.Add(new ListItem { Text = "Todas las obras"      , Value = "%", Selected = true });

                DataFiltroFlorilegios.SelectCommand = "SELECT DISTINCT Florilegio FROM Extractos WHERE Florilegio IS NOT NULL ORDER BY Florilegio";
                DataFiltroAutores    .SelectCommand = "SELECT DISTINCT Autor      FROM Extractos WHERE Autor      IS NOT NULL ORDER BY Autor     ";
                DataFiltroObras      .SelectCommand = "SELECT DISTINCT Obra       FROM Extractos WHERE Obra       IS NOT NULL ORDER BY Obra      ";

                DataFiltroFlorilegios.DataBind();
                DataFiltroAutores    .DataBind();
                DataFiltroObras      .DataBind();

                DropFiltroFlorilegios.DataBind();
                DropFiltroAutores    .DataBind();
                DropFiltroObras      .DataBind();

                UpdateFiltro.Update();

                ActualizarTablaExtractos();
            }
            catch (Exception Excepcion)
            {
                new Log(Session["usuario"].ToString(), "EXCEPCIÓN EN FILTRO", Excepcion.Message).RegistrarActividad();

                new Popup(Notificacion);
            }
        }

        protected void GenerarGrafico()
        {
            try
            {
                int i = 0;
                List<string> Recuento    = new List<string>();
                List<string> Florilegios = new List<string>();

                // Formato del gráfico (serie)
                var series = new Series
                {
                    Name = "Series1",
                    ChartType = SeriesChartType.Bar,                   // Tipo de gráfico (barras)
                    Color = Color.FromArgb(200, 66, 139, 202),         // Color del interior de las barras (el primer parámetro es la transparencia (0 (transparente) a 255 (sólido))
                    BorderColor = ColorTranslator.FromHtml("#428BCA"), // Color del borde de las barras
                    BorderWidth = 1                                    // Ancho del borde de las barras
                };

                // Formato de la región de los ejes (area)
                var area = new ChartArea("ChartArea1");
                area.AxisX.LineWidth = 1; // Ancho de la línea del eje X.
                area.AxisY.LineWidth = 1; // Ancho de la línea del eje Y.
                area.AxisX.LineColor = ColorTranslator.FromHtml("#DDDDDD"); // Color de la línea del eje X.
                area.AxisY.LineColor = ColorTranslator.FromHtml("#DDDDDD"); // Color de la línea del eje Y.
                area.AxisX.MajorGrid.LineWidth = 1; // Ancho de los separadores del eje X.
                area.AxisY.MajorGrid.LineWidth = 1; // Ancho de los separadores del eje Y.
                area.AxisX.MajorGrid.LineColor = ColorTranslator.FromHtml("#DDDDDD"); // Color de los separadores del eje X.
                area.AxisY.MajorGrid.LineColor = ColorTranslator.FromHtml("#DDDDDD"); // Color de los separadores del eje Y.
                area.AxisX.MajorTickMark.LineColor = ColorTranslator.FromHtml("#FFFFFF"); // Color de los marcadores del eje X.
                area.AxisY.MajorTickMark.LineColor = ColorTranslator.FromHtml("#DDDDDD"); // Color de los marcadores del eje Y.
                area.AxisX.LabelAutoFitMaxFontSize = 7; // Tamaño máximo de la fuente del eje X.
                area.AxisY.LabelAutoFitMaxFontSize = 7; // Tamaño máximo de la fuente del eje Y.
                area.AxisX.LabelStyle.ForeColor = ColorTranslator.FromHtml("#666666"); // Color de la fuente del eje X.
                area.AxisY.LabelStyle.ForeColor = ColorTranslator.FromHtml("#666666"); // Color de la fuente del eje Y.

                // Preparativos de la consulta
                string consulta = "SELECT Florilegio, COUNT(Florilegio) AS Recuento FROM Extractos WHERE Florilegio IS NOT NULL GROUP BY Florilegio";
                OleDbConnection conexion = new OleDbConnection(CadenaConexion);
                OleDbCommand comando = new OleDbCommand(consulta, conexion);

                // Refrescamos
                DataRecuento.SelectCommand = consulta;
                DataRecuento.DataBind();
                TablaRecuento.DataBind();

                // Conexión con la BD y ejecución de la consulta, respectivamente
                conexion.Open();
                OleDbDataReader reader = comando.ExecuteReader();

                // Recorremos todos los registros
                while (reader.Read())
                {
                    // Almacenamos los datos necesarios para el gráfico
                    Florilegios.Add(reader["Florilegio"].ToString());
                    Recuento.Add(reader["Recuento"].ToString());
                    i++;
                }

                // Incorporamos los datos en el gráfico
                series.Points.DataBindXY(Florilegios, Recuento);

                // Aunque estas dos instrucciones se ejecuten siempre, solo son necesarias cuando eliminamos el último extracto de un florilegio
                // ¿Por qué? Supón que eliminas el último extracto del florilegio "IO. MURMELLIUS", en cuyo caso no tiene sentido que este florilegio aparezca en el gráfico al no tener extractos
                // En este caso en concreto el gráfico se genera al invocarse a la función GenerarGrafico() dentro del evento Page_Load()
                // El problema es que el evento Page_Load se ejecuta ANTES del manejador del evento Click del botón de eliminación del extracto seleccionado
                // Esto implica que estamos generando un gráfico desactuaizado, ya que seguiría teniendo en cuenta el recuento de ese florilegio sin extractos
                // Por este motivo se vuelve a invocar la función GenerarGrafico explícitamente tras eliminar el extracto, dentro del manejador del evento Click del botón de eliminación
                // Lo malo es que el gráfico ya se había generado en el evento Page_Load, de modo que lo estaríamos generando por segunda vez, dando error y saltando una excepción
                // Para que esto no ocurra, eliminamos el gráfico desactualizado generado durante el evento Page_Load para poder volverlo a crear sin fallos
                ChartFlorilegios.ChartAreas.Clear();
                ChartFlorilegios.Series.Clear();

                // Renderizamos el gráfico
                ChartFlorilegios.ChartAreas.Add(area);
                ChartFlorilegios.Series.Add(series);

                conexion.Close();
            }
            catch (Exception Excepcion)
            {
                new Log(Session["usuario"].ToString(), "EXCEPCIÓN EN GENERAR GRÁFICO", Excepcion.Message).RegistrarActividad();

                new Popup(Notificacion);
            }
        }

        private void VaciarFormulario()
        {
            // Entradas
            ExtractosSeleccionados.Text = "";

            // Bloqueamos los botones
            ButtonPublicar   .Enabled = false;
            ButtonDespublicar.Enabled = false;

            // Desactivamos todas las filas
            foreach (GridViewRow Row in TablaExtractos.Rows)
                Row.CssClass = "";

            UpdateTabla.Update();
        }
    }
}