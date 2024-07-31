using System;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Drawing;
using System.Data.OleDb;
using Excerpta.App_Code;
using System.Web.UI.WebControls;
using System.Web.UI.DataVisualization.Charting;

namespace Excerpta
{
    public partial class MisExtractos : Page
    {
        private static readonly String CadenaConexion = "Provider = Microsoft.ACE.OLEDB.12.0; Data Source = " + @"c:\inetpub\excerpta\excerpta.accdb;";

        protected void Page_Load(object sender, EventArgs e)
        {
            // Sesión expirada o usuario no registrado
            Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
            if (Session["usuario"] == null)
                Response.Redirect("~/Default.aspx");

            // Ocultamos las notificaciones residuales
            Notificacion.Visible = false;

            if (Page.IsPostBack) return;

            MostrarFiltroUsuarios.Visible = (Session["usuario"].ToString() == "gregorio.rodriguez@ulpgc.es") ? true : false;

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

            GenerarFiltro();
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

        /*
        ╔═══════════════════╗
        ║     EXTRACTOS     ║
        ╚═══════════════════╝
        */

        // Recuento
        protected void DataExtractos_Selected(object sender, SqlDataSourceStatusEventArgs e)
        {
            RecuentoExtractos.Text = e.AffectedRows.ToString() + " extracto(s)";

            // Mostramos el paginador solo cuando es necesario
            TablaExtractos.AllowPaging = e.AffectedRows > 500 ? true : false;
            TablaExtractos.CssClass    = e.AffectedRows > 500 ? "margin-bottom-50" : "";
        }

        // Formato
        protected void TablaExtractos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                try
                {
                    // Traducimos el estado del alta
                    e.Row.Cells[1].Text = e.Row.Cells[1].Text == "True" ? "Sí" : "No";

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

            try
            {
                // Posición de la fila seleccionada
                int i = Convert.ToInt32(e.CommandArgument);

                // Hemos seleccionado la misma fila que antes
                if (TablaExtractos.Rows[i].Cells[0].Text.Trim() == ExtractoSeleccionado.Text.Trim())
                {
                    VaciarFormulario();

                    GenerarGrafico();

                    return;
                }

                VaciarFormulario();

                // Resaltamos la fila
                TablaExtractos.Rows[i].CssClass = TablaExtractos.Rows[i].CssClass = "active";

                // Obtenemos el ID del extracto seleccionado
                ExtractoSeleccionado.Text = WebUtility.HtmlDecode(TablaExtractos.Rows[i].Cells[0].Text).Trim();

                // Rellenamos el desplegable de florilegios
                DropFlorilegios.Items.Clear();

                DataFlorilegios.SelectCommand = "SELECT DISTINCT Florilegio FROM Descripciones ORDER BY Florilegio";

                DataFlorilegios.DataBind();
                DropFlorilegios.DataBind();

                // Rellenamos el formulario
                using (OleDbConnection Conexion = new OleDbConnection(CadenaConexion))
                {
                    Conexion.Open();

                    OleDbCommand Comando = new OleDbCommand("SELECT * FROM Extractos WHERE ID = @ID", Conexion);

                    Comando.Parameters.AddWithValue("@ID", ExtractoSeleccionado.Text.Trim());

                    OleDbDataReader Reader = Comando.ExecuteReader();

                    if (Reader.Read())
                    {
                        // Tras seleccionar el ítem de un desplegable debemos rellenar el siguiente en función del ítem seleccionado en el anterior
                        DropFlorilegios.SelectedValue = Reader["Florilegio"].ToString(); SincronizarAutores();
                        DropAutores    .SelectedValue = Reader["Autor"     ].ToString(); SincronizarObras();
                        DropObras      .SelectedValue = Reader["Obra"      ].ToString();

                        InputLibro       .Text = Reader["Libro"       ].ToString();
                        InputPoema       .Text = Reader["Poema"       ].ToString();
                        InputVersoInicial.Text = Reader["VersoInicial"].ToString();
                        InputVersoFinal  .Text = Reader["VersoFinal"  ].ToString();
                        InputCapitulo    .Text = Reader["Capítulo"    ].ToString();
                        InputSubcapitulo .Text = Reader["Subcapítulo" ].ToString();
                        InputExtracto    .Text = Reader["Extracto"    ].ToString().Replace("/", "\n");
                        InputTLL         .Text = Reader["TLL"         ].ToString().Replace("/", "\n");
                        InputVernacula   .Text = Reader["Vernácula"   ].ToString().Replace("/", "\n");
                        InputPagina      .Text = Reader["Página"      ].ToString();
                    }

                    Reader.Close();
                }

                DesbloquearFormulario();

                GenerarGrafico();

                UpdateTabla.Update();
            }
            catch (Exception Excepcion)
            {
                VaciarFormulario();

                new Log(Session["usuario"].ToString(), "EXCEPCIÓN EN SELECCIONAR EXTRACTO", Excepcion.Message).RegistrarActividad();

                new Popup(Notificacion, "No se ha podido cargar el elemento seleccionado");
            }
        }

        // Modificar
        protected void BotonEditarExtracto_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            try
            {
                new Extracto().Modificar(ExtractoSeleccionado.Text.Trim(), DropFlorilegios.SelectedValue, DropAutores.SelectedValue, DropObras.SelectedValue, InputLibro.Text.Trim(), InputPoema.Text.Trim(), InputCapitulo.Text.Trim(), InputSubcapitulo.Text.Trim(), InputVersoInicial.Text.Trim(), InputVersoFinal.Text.Trim(), InputExtracto.Text.Trim(), InputTLL.Text.Trim(), InputVernacula.Text.Trim(), InputPagina.Text.Trim());

                ActualizarTablaExtractos(); // Refrescamos la tabla para poder saber si está vacía

                // Actualizamos el filtro en caso de quedar la tabla vacía
                if (TablaExtractos.Rows.Count == 0) GenerarFiltro();

                new Log(Session["usuario"].ToString(), "MODIFICAR EXTRACTO").RegistrarActividad();

                new Popup(Notificacion, "Operación completada", false);
            }
            catch (Exception Excepcion)
            {
                new Log(Session["usuario"].ToString(), "EXCEPCIÓN EN MODIFICAR EXTRACTO", Excepcion.Message).RegistrarActividad();

                new Popup(Notificacion);
            }
        }

        // Eliminar
        protected void BotonEliminarExtracto_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            try
            {
                new Extracto().Eliminar(ExtractoSeleccionado.Text.Trim());

                ActualizarTablaExtractos(); // Refrescamos la tabla para poder saber si está vacía

                // Actualizamos el filtro en caso de quedar la tabla vacía
                if (TablaExtractos.Rows.Count == 0) GenerarFiltro();

                new Log(Session["usuario"].ToString(), "ELIMINAR EXTRACTO").RegistrarActividad();

                new Popup(Notificacion, "Operación completada", false);
            }
            catch (Exception Excepcion)
            {
                new Log(Session["usuario"].ToString(), "EXCEPCIÓN EN ELIMINAR EXTRACTO", Excepcion.Message).RegistrarActividad();

                new Popup(Notificacion);
            }
        }

        // Sincronizar el desplegable de autores
        protected void DropFlorilegios_SelectedIndexChanged(object sender, EventArgs e)
        {
            SincronizarAutores();
        }

        // Sincronizar el desplegable de obras
        protected void DropAutores_SelectedIndexChanged(object sender, EventArgs e)
        {
            SincronizarObras();
        }

        /*
        ╔══════════════════════╗
        ║     VALIDACIONES     ║
        ╚══════════════════════╝
        */

        // El verso inicial debe ser menor o igual que el final
        protected void ValidarVersos(object source, ServerValidateEventArgs args)
        {
            if (Convert.ToInt32(InputVersoInicial.Text.Trim()) > Convert.ToInt32(InputVersoFinal.Text.Trim()))
            {
                args.IsValid = false;

                new Popup(Notificacion, "El verso final no puede ser mayor que el verso final");
            }
        }

        // Un extracto publicado no puede modificarse ni eliminarse
        protected void ValidarPublicado(object source, ServerValidateEventArgs args)
        {
            if (new Extracto().EstaPublicado(ExtractoSeleccionado.Text.Trim()))
            {
                args.IsValid = false;

                new Popup(Notificacion, "El extracto se encuentra publicado. Contacte con el administrador");
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

                DataFiltroFlorilegios.SelectCommand = "SELECT DISTINCT Florilegio FROM Extractos WHERE Usuario LIKE '" + (( Session["usuario"].ToString() == "gregorio.rodriguez@ulpgc.es" ) ? DropFiltroUsuarios.SelectedValue : Session["usuario"].ToString()) + "' ORDER BY Florilegio";

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

                DataFiltroAutores.SelectCommand = "SELECT DISTINCT Autor FROM Extractos WHERE Usuario LIKE '" + (( Session["usuario"].ToString() == "gregorio.rodriguez@ulpgc.es" ) ? DropFiltroUsuarios.SelectedValue : Session["usuario"].ToString()) + "' AND Florilegio LIKE '" + DropFiltroFlorilegios.SelectedValue + "' ORDER BY Autor";

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

                DataFiltroObras.SelectCommand = "SELECT DISTINCT Obra FROM Extractos WHERE Usuario LIKE '" + (( Session["usuario"].ToString() == "gregorio.rodriguez@ulpgc.es" ) ? DropFiltroUsuarios.SelectedValue : Session["usuario"].ToString()) + "' AND Florilegio LIKE '" + DropFiltroFlorilegios.SelectedValue + "' AND Autor LIKE '" + DropFiltroAutores.SelectedValue + "' ORDER BY Obra";

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

        private void ActualizarTablaExtractos()
        {
            try
            {
                DataExtractos.SelectCommand = "SELECT ID, Alta, Florilegio, Autor, Obra, Libro, Poema, [Capítulo], [Subcapítulo], VersoInicial, VersoFinal, Extracto, TLL, [Vernácula], [Página] FROM Extractos WHERE Usuario LIKE '" + (( Session["usuario"].ToString() == "gregorio.rodriguez@ulpgc.es" ) ? DropFiltroUsuarios.SelectedValue : Session["usuario"].ToString()) + "' ";

                if (DropFiltroFlorilegios.SelectedIndex > 0) DataExtractos.SelectCommand = DataExtractos.SelectCommand + "AND Florilegio LIKE '" + DropFiltroFlorilegios.SelectedValue + "' ";
                if (DropFiltroAutores    .SelectedIndex > 0) DataExtractos.SelectCommand = DataExtractos.SelectCommand + "AND Autor      LIKE '" + DropFiltroAutores    .SelectedValue + "' ";
                if (DropFiltroObras      .SelectedIndex > 0) DataExtractos.SelectCommand = DataExtractos.SelectCommand + "AND Obra       LIKE '" + DropFiltroObras      .SelectedValue + "' ";

                DataExtractos.SelectCommand = DataExtractos.SelectCommand + "AND Alta LIKE '" + DropFiltroEstados.SelectedValue + "' ORDER BY Florilegio, Autor, Obra, Libro, Poema, [Capítulo], [Subcapítulo]";

                DataExtractos .DataBind();
                TablaExtractos.DataBind();

                UpdateTabla   .Update();
                UpdateRecuento.Update();

                GenerarPDF();
                GenerarGrafico();

                VaciarFormulario();
            }
            catch (Exception Excepcion)
            {
                new Log(Session["usuario"].ToString(), "EXCEPCIÓN EN FILTRO", Excepcion.Message).RegistrarActividad();

                new Popup(Notificacion);
            }
        }

        private void SincronizarAutores()
        {
            try
            {
                DropAutores.Items.Clear();

                DataAutores.SelectCommand = "SELECT DISTINCT Autor FROM Descripciones WHERE Florilegio LIKE '" + DropFlorilegios.SelectedValue + "' ORDER BY Autor";

                DataAutores.DataBind();
                DropAutores.DataBind();

                SincronizarObras();
            }
            catch (Exception Excepcion)
            {
                new Log(Session["usuario"].ToString(), "EXCEPCIÓN EN SELECCIONAR AUTOR", Excepcion.Message).RegistrarActividad();

                new Popup(Notificacion);
            }
        }

        private void SincronizarObras()
        {
            try
            {
                DropObras.Items.Clear();

                DataObras.SelectCommand = "SELECT DISTINCT Obra FROM Descripciones WHERE Florilegio LIKE '" + DropFlorilegios.SelectedValue + "' AND Autor LIKE '" + DropAutores.SelectedValue + "' ORDER BY Obra";

                DataObras.DataBind();
                DropObras.DataBind();
            }
            catch (Exception Excepcion)
            {
                new Log(Session["usuario"].ToString(), "EXCEPCIÓN EN SELECCIONAR OBRA", Excepcion.Message).RegistrarActividad();

                new Popup(Notificacion);
            }
        }

        private void GenerarFiltro()
        {
            try
            {
                DropFiltroFlorilegios.Items.Clear();
                DropFiltroAutores    .Items.Clear();
                DropFiltroObras      .Items.Clear();

                DropFiltroFlorilegios.Items.Add(new ListItem { Text = "Todos los florilegios", Value = "%", Selected = true });
                DropFiltroAutores    .Items.Add(new ListItem { Text = "Todos los autores"    , Value = "%", Selected = true });
                DropFiltroObras      .Items.Add(new ListItem { Text = "Todas las obras"      , Value = "%", Selected = true });

                DataFiltroFlorilegios.SelectCommand = "SELECT DISTINCT Florilegio FROM Extractos WHERE Usuario LIKE '" + (( Session["usuario"].ToString() == "gregorio.rodriguez@ulpgc.es" ) ? DropFiltroUsuarios.SelectedValue : Session["usuario"].ToString()) + "' AND Florilegio IS NOT NULL ORDER BY Florilegio";
                DataFiltroAutores    .SelectCommand = "SELECT DISTINCT Autor      FROM Extractos WHERE Usuario LIKE '" + (( Session["usuario"].ToString() == "gregorio.rodriguez@ulpgc.es" ) ? DropFiltroUsuarios.SelectedValue : Session["usuario"].ToString()) + "' AND Autor      IS NOT NULL ORDER BY Autor     ";
                DataFiltroObras      .SelectCommand = "SELECT DISTINCT Obra       FROM Extractos WHERE Usuario LIKE '" + (( Session["usuario"].ToString() == "gregorio.rodriguez@ulpgc.es" ) ? DropFiltroUsuarios.SelectedValue : Session["usuario"].ToString()) + "' AND Obra       IS NOT NULL ORDER BY Obra      ";

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

        private void GenerarGrafico()
        {
            try
            {
                int i = 0;
                int[]    Recuento    = new int   [DropFiltroFlorilegios.Items.Count - 1];
                string[] Florilegios = new string[DropFiltroFlorilegios.Items.Count - 1];

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
                string consulta = "SELECT Florilegio, COUNT(Florilegio) AS Recuento FROM Extractos WHERE Usuario LIKE '" + (( Session["usuario"].ToString() == "gregorio.rodriguez@ulpgc.es" ) ? DropFiltroUsuarios.SelectedValue : Session["usuario"].ToString()) + "' AND Florilegio IS NOT NULL GROUP BY Florilegio";
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
                    Recuento[i] = Convert.ToInt32(reader["Recuento"].ToString());
                    Florilegios[i] = reader["Florilegio"].ToString();
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

                new Popup(Notificacion, Excepcion.Message);
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

        private void DesbloquearFormulario()
        {
            // Desplegables
            DropFlorilegios.Enabled = true;
            DropAutores    .Enabled = true;
            DropObras      .Enabled = true;

            // Entradas
            InputLibro       .Enabled = true;
            InputPoema       .Enabled = true;
            InputCapitulo    .Enabled = true;
            InputSubcapitulo .Enabled = true;
            InputVersoInicial.Enabled = true;
            InputVersoFinal  .Enabled = true;
            InputExtracto    .Enabled = true;
            InputTLL         .Enabled = true;
            InputVernacula   .Enabled = true;
            InputPagina      .Enabled = true;

            // Botones
            BotonGuardar .Enabled = true;
            BotonEliminar.Enabled = true;
        }

        private void BloquearFormulario()
        {
            // Desplegables
            DropFlorilegios.Enabled = false;
            DropAutores    .Enabled = false;
            DropObras      .Enabled = false;

            // Entradas
            InputLibro       .Enabled = false;
            InputPoema       .Enabled = false;
            InputCapitulo    .Enabled = false;
            InputSubcapitulo .Enabled = false;
            InputVersoInicial.Enabled = false;
            InputVersoFinal  .Enabled = false;
            InputExtracto    .Enabled = false;
            InputTLL         .Enabled = false;
            InputVernacula   .Enabled = false;
            InputPagina      .Enabled = false;

            // Botones
            BotonGuardar .Enabled = false;
            BotonEliminar.Enabled = false;
        }

        private void VaciarFormulario()
        {
            // Desplegables
            DataFlorilegios.SelectCommand = "";
            DataAutores    .SelectCommand = "";
            DataObras      .SelectCommand = "";

            DataFlorilegios.DataBind();
            DataAutores    .DataBind();
            DataObras      .DataBind();

            DropFlorilegios.DataBind();
            DropAutores    .DataBind();
            DropObras      .DataBind();

            DropFlorilegios.Items.Clear();
            DropAutores    .Items.Clear();
            DropObras      .Items.Clear();

            // Entradas
            ExtractoSeleccionado.Text = "";
            InputLibro          .Text = "";
            InputPoema          .Text = "";
            InputVersoInicial   .Text = "";
            InputVersoFinal     .Text = "";
            InputCapitulo       .Text = "";
            InputSubcapitulo    .Text = "";
            InputExtracto       .Text = "";
            InputTLL            .Text = "";
            InputVernacula      .Text = "";
            InputPagina         .Text = "";

            BloquearFormulario();

            // Desactivamos todas las filas
            foreach (GridViewRow Row in TablaExtractos.Rows)
                Row.CssClass = "";

            UpdateTabla.Update();
        }
    }
}