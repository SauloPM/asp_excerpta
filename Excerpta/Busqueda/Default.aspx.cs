using System;
using System.Web;
using System.Web.UI;
using System.Data.OleDb;
using Busqueda.App_Code;
using System.Web.UI.DataVisualization.Charting;

namespace Busqueda
{
    public partial class _Default : Page
    {
        //readonly string CadenaConexion = "Provider = Microsoft.Jet.OLEDB.4.0; Data Source = " + HttpContext.Current.Server.MapPath("~/App_Data") + @"\excerpta.mdb";
        private static readonly String CadenaConexion = "Provider = Microsoft.ACE.OLEDB.12.0; Data Source = " + @"c:\inetpub\excerpta\excerpta.accdb;";
        protected void Page_Load(object sender, EventArgs e)
        {
            // Ítem activo del navbar
            Master.NavHojear_Active     = "";
            Master.NavBuscar_Active     = "";
            Master.NavBiblioteca_Active = "";
            Master.NavCreditos_Active   = "active";

            // Eliminamos las notificaciones residuales
            Notificacion.Visible = false;

            if (!Page.IsPostBack) GenerarGrafico();
        }

        /*
        ╔═════════════════╗
        ║     GRÁFICO     ║
        ╚═════════════════╝
        */

        protected void GenerarGrafico()
        {
            try
            {
                int i = 0;
                Series Serie = ChartFlorilegios.Series["Serie"];

                // Color palette customization
                ChartFlorilegios.Palette = ChartColorPalette.None;
                //ChartFlorilegios.PaletteCustomColors = new Color[] {
                //    ColorTranslator.FromHtml("#FF4444"),
                //    ColorTranslator.FromHtml("#FFBB33"),
                //    ColorTranslator.FromHtml("#00C851"),
                //    ColorTranslator.FromHtml("#33B5E5"),
                //    ColorTranslator.FromHtml("#AA66CC")
                //};

                using (OleDbConnection Conexion = new OleDbConnection(CadenaConexion))
                {
                    Conexion.Open();

                    OleDbCommand Comando = new OleDbCommand("SELECT Florilegio, COUNT(Florilegio) AS Recuento FROM Extractos WHERE Florilegio IS NOT NULL AND ALTA=True GROUP BY Florilegio", Conexion);

                    OleDbDataReader Reader = Comando.ExecuteReader();

                    // Poblamos el gráfico
                    while (Reader.Read())
                    {
                        Serie.Points.AddXY("", Reader["Recuento"]);
                        Serie.Points[i++].LegendText = Reader["Florilegio"].ToString().Replace(" (per L. Colvandrum)", "");
                    }
                }
            }
            catch (Exception Excepcion)
            {
                new Log("EXCEPCIÓN EN GENERAR GRÁFICO", Excepcion.Message).RegistrarActividad();

                new Popup(Notificacion, "Se ha producido un error en la generación del gráfico");
            }
        }

        /*
        ╔══════════════════╗
        ║     CONTACTO     ║
        ╚══════════════════╝
        */

        // Enviar mensaje
        protected void ButtonEnviar_Click(object sender, EventArgs e)
        {
            try
            {
                new Email(InputAsunto.Text.Trim(), InputMensaje.Text.Trim(), InputNombre.Text.Trim(), InputEmail.Text.Trim()).EnviarEmail();

                new Popup(Notificacion, "Operación completada", false);
            }
            catch (Exception Excepcion)
            {
                new Log("EXCEPCIÓN EN ENVIAR EMAIL", Excepcion.Message).RegistrarActividad();

                new Popup(Notificacion);
            }

            // Vaciamos el formulario
            InputNombre .Text = "";
            InputEmail  .Text = "";
            InputAsunto .Text = "";
            InputMensaje.Text = "";
        }
    }
}