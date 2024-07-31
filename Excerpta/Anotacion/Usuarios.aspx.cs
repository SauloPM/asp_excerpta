using System;
using System.IO;
using System.Net;
using System.Web;
using System.Data;
using System.Linq;
using System.Web.UI;
using Excerpta.App_Code;
using System.Data.OleDb;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Web.UI.DataVisualization.Charting;

namespace Excerpta
{
    public partial class Usuarios : Page
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

            Notificacion.Visible = false;

            GenerarGrafico();

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
        ╔══════════════════╗
        ║     USUARIOS     ║
        ╚══════════════════╝
        */

        // Filtro
        protected void DropUsuarios_SelectedIndexChanged(object sender, EventArgs e)
        {
            AplicarFiltro();
        }

        // Recuento
        protected void DataUsuarios_Selected(object sender, SqlDataSourceStatusEventArgs e)
        {
            Recuento.Text = e.AffectedRows.ToString() + " usuario(s)";
        }

        // Formato
        protected void TablaUsuarios_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    // Desciframos los datos
                    for (int i = 1; i < 6; i++)
                        e.Row.Cells[i].Text = new cifrado(e.Row.Cells[i].Text).descifrar();

                    // Representamos el estado del alta con iconos
                    e.Row.Cells[0].Text = e.Row.Cells[0].Text == "True" ? "<i class='fa fa-fw fa-check' aria-hidden='true'></i>" : "<i class='fa fa-fw fa-times' aria-hidden='true'></i>";
                }
            }
            catch
            {
                // No hacemos nada
            }
        }

        // Seleccionar
        protected void TablaUsuarios_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Select")
                return;

            try
            {
                // Posición de la fila seleccionada
                int i = Convert.ToInt32(e.CommandArgument);

                // Hemos seleccionado la misma fila que antes
                if (TablaUsuarios.Rows[i].CssClass == "active")
                {
                    TablaUsuarios.Rows[i].CssClass = "";

                    InputEmails.Text = InputEmails.Text.Replace(TablaUsuarios.Rows[i].Cells[3].Text + ",", "");

                    UpdateTabla.Update();

                    // Desactivamos los botones si no hubiera ninguna fila seleccionada
                    if (InputEmails.Text.Trim() == "")
                    {
                        ButtonAlta.Enabled = false;
                        ButtonBaja.Enabled = false;
                    }

                    return;
                }

                // Resaltamos la fila
                TablaUsuarios.Rows[i].CssClass = TablaUsuarios.Rows[i].CssClass = "active";

                // Rellenamos el formulario
                InputEmails.Text += WebUtility.HtmlDecode(TablaUsuarios.Rows[i].Cells[3].Text).Trim() + ",";

                // Activamos los botones
                ButtonAlta.Enabled = true;
                ButtonBaja.Enabled = true;

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

        // Dar de alta usuario
        protected void ButtonAlta_Click(object sender, EventArgs e)
        {
            try
            {
                new Usuario().DarDeAlta(InputEmails.Text.Trim());

                VaciarFormulario();

                new Popup(Notificacion, "Operación completada", false);
            }
            catch (Exception Excepcion)
            {
                new Log(Session["usuario"].ToString(), "EXCEPCIÓN EN DAR DE ALTA A UN USUARIO", Excepcion.Message).RegistrarActividad();

                new Popup(Notificacion);
            }
        }

        // Dar de baja usuario
        protected void ButtonBaja_Click(object sender, EventArgs e)
        {
            try
            {
                new Usuario().DarDeBaja(InputEmails.Text.Trim());

                VaciarFormulario();

                GenerarGrafico();

                new Popup(Notificacion, "Operación completada", false);
            }
            catch (Exception Excepcion)
            {
                new Log(Session["usuario"].ToString(), "EXCEPCIÓN EN DAR DE BAJA A UN USUARIO", Excepcion.Message).RegistrarActividad();

                new Popup(Notificacion);
            }
        }

        /*
        ╔═══════════════════╗
        ║     ACTIVIDAD     ║
        ╚═══════════════════╝
        */

        protected void ButtonVerActividad_Click(object sender, EventArgs e)
        {
            string dirUsuariosLog = HttpContext.Current.Server.MapPath("~/Log");
            DirectoryInfo directory;
            int[] calculo = { 0, 0 };

            string[] florilegios = Directory.GetDirectories(dirUsuariosLog);
            Array.Sort(florilegios);

            DataTable dt = new DataTable();
            dt.Columns.Add("Usuario", typeof(string));
            dt.Columns.Add("Fecha - Hora", typeof(string));
            dt.Columns.Add("Anotaciones / Actualizaciones", typeof(string));
            dt.Columns.Add("Sesiones", typeof(string));
            dt.Columns.Add("Florilegio", typeof(string));

            foreach (var florilegio in florilegios)
            {
                directory = new DirectoryInfo(florilegio);
                //FileInfo [] numFiles = directory.GetFiles().OrderByDescending(f => f).;
                if (directory.ToString().IndexOf("Administrador") > -1) continue;
                int[] x = ficheros(dt, directory);
                calculo[0] += x[0];
                calculo[1] += x[1];
            }
            ActividadUser.DataSource = dt;
            ActividadUser.DataBind();
            double ratio = (float)calculo[1] / (float)calculo[0];
            Resultados.Text = "Inserciones vs Actualizaciones: " + calculo[0] + " / " + calculo[1] + " " + string.Format("Ratio de correciones = {0:0.0%}", ratio);
            Resultados.Visible = true;
        }

        private int[] ficheros(DataTable dt, DirectoryInfo directory)
        {
            ListBox l = new ListBox();
            int[] calculo = { 0, 0 };
            DataRow row = dt.NewRow();
            List<FileInfo> myFile = directory.GetFiles().OrderByDescending(f => f.LastWriteTime).ToList();
            if (myFile.Count == 0) return calculo; // Tiene creado el directorio pero no ha trabajado
            string[] prev = recorre(myFile[0].FullName).Split('#');
            calculo[0] = System.Convert.ToInt16(prev[0]);
            calculo[1] = System.Convert.ToInt16(prev[1]);
            int inserciones = 0;
            int modificaciones = 0;
            string[] res = { "0", "0", "-" };
            for (int i = 1; i < myFile.Count; i++)
            {
                res = recorre(myFile[i].FullName).Split('#');
                inserciones += System.Convert.ToInt16(res[0]);
                modificaciones += System.Convert.ToInt16(res[1]);
                //dt.Rows.Add(ficheros(file.FullName, file.LastWriteTime.ToString(), first, dt));
                l.Items.Add(inserciones + "");
            }
            string aux = prev[0] + " - " + prev[1] + " / (" + inserciones + " - " + modificaciones + ")";
            dt.Rows.Add(directory.ToString().Substring(directory.ToString().LastIndexOf("\\") + 1), myFile[0].LastWriteTime.ToString(), aux, myFile.Count + "", prev[2]);
            calculo[0] += inserciones;
            calculo[1] += modificaciones;
            return calculo;
        }

        private string recorre(string myFile)
        {
            int c = 0;
            int a = 0;
            string flo = "";
            try
            {
                StreamReader sr = new StreamReader(myFile);
                string line = sr.ReadLine();
                while (line != null)
                {
                    if (line.IndexOf("HA INSERTADO") > -1)
                    {
                        c++;
                        int posi = line.IndexOf("'");
                        if (posi > -1)
                        {
                            flo = line.Substring(posi);
                        }
                    }
                    if (line.IndexOf("HA ACTUALIZAOD") > -1) a++;
                    line = sr.ReadLine();
                }
                sr.Close();
            }
            catch (Exception)
            { }
            if (flo.Length == 0) flo = "-";
            return c + "#" + a + "#" + flo;
        }

        /*
        ╔══════════════════╗
        ║     AUXILIAR     ║
        ╚══════════════════╝
        */

        private void VaciarFormulario()
        {
            // Vaciamos el formulario
            InputEmails.Text = "";

            // Desactivamos los botones
            ButtonAlta.Enabled = false;
            ButtonBaja.Enabled = false;

            // Desactivamos todas las filas
            foreach (GridViewRow Row in TablaUsuarios.Rows)
                Row.CssClass = "";

            // Refrescamos
            DataUsuarios .DataBind();
            TablaUsuarios.DataBind();

            UpdateTabla.Update();
        }

        protected void AplicarFiltro()
        {
            try
            {
                string ConsultaSQL = "SELECT * FROM Usuarios";

                if (DropUsuarios.SelectedIndex == 1) DataUsuarios.SelectCommand = ConsultaSQL + " WHERE Alta = -1 ";
                if (DropUsuarios.SelectedIndex == 2) DataUsuarios.SelectCommand = ConsultaSQL + " WHERE Alta =  0 ";

                // Refrescamos
                DataUsuarios .DataBind();
                TablaUsuarios.DataBind();

                UpdateTabla.Update();
            }
            catch (Exception Excepcion)
            {
                new Log(Session["usuario"].ToString(), "EXCEPCIÓN EN APLICAR FILTRO", Excepcion.Message).RegistrarActividad();

                new Popup(Notificacion);
            }
        }

        protected void GenerarGrafico()
        {
            try
            {
                // Vaciamos el contenido anterior, ahora obsoleto
                ChartUsuarios.Series.Clear();
                ChartUsuarios.Series.Add("SerieUsuarios");

                int i = 0;
                Series Serie = ChartUsuarios.Series["SerieUsuarios"];
                Serie.ChartType = SeriesChartType.Doughnut;

                // Llenamos el gráfico de datos
                using (OleDbConnection Conexion = new OleDbConnection(CadenaConexion))
                {
                    Conexion.Open();

                    OleDbCommand Comando = new OleDbCommand("SELECT Usuario, COUNT(Usuario) AS Recuento FROM Extractos WHERE Usuario IS NOT NULL GROUP BY Usuario", Conexion);

                    OleDbDataReader Reader = Comando.ExecuteReader();
                    
                    while (Reader.Read())
                    {
                        Serie.Points.AddXY("", Reader["Recuento"]);
                        Serie.Points[i++].LegendText = Reader["Usuario"].ToString();
                    }
                }
            }
            catch (Exception Excepcion)
            {
                new Log(Session["usuario"].ToString(), "EXCEPCIÓN EN GENERAR GRÁFICO", Excepcion.Message).RegistrarActividad();

                new Popup(Notificacion);
            }
        }
    }
}