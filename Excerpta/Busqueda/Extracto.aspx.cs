using System;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Data.OleDb;
using Busqueda.App_Code;
using System.Web.UI.WebControls;

namespace Busqueda
{
    public partial class Extracto : Page
    {
        // Obtenemos el ID del extracto a partir de la URL
        string IDExtracto = HttpUtility.ParseQueryString(new Uri(HttpContext.Current.Request.Url.AbsoluteUri).Query).Get("id");

        // Cadena de conexión
        //readonly string CadenaConexion = "Provider = Microsoft.Jet.OLEDB.4.0; Data Source = " + HttpContext.Current.Server.MapPath("~/App_Data") + @"\excerpta.mdb";
        readonly string CadenaConexion = "Provider = Microsoft.ACE.OLEDB.12.0; Data Source = " + @"c:\inetpub\excerpta\excerpta.accdb;";

        protected void Page_Load(object sender, EventArgs e)
        {
            // Ítem activo del navbar
            Master.NavHojear_Active     = "";
            Master.NavBuscar_Active     = "active";
            Master.NavBiblioteca_Active = "";
            Master.NavCreditos_Active   = "";

            if (Page.IsPostBack) return;

            if (IDExtracto == null)
                Response.Redirect("~/Default.aspx");

            // Cargamos la ficha del extracto
            try
            {
                using (OleDbConnection Conexion = new OleDbConnection(CadenaConexion))
                {
                    Conexion.Open();

                    // OleDbCommand Comando = new OleDbCommand("SELECT DISTINCT ID, Florilegio, Autor, Obra, Libro, Poema, VersoInicial, VersoFinal, [Capítulo], [Subcapítulo], Extracto, TLL, [Vernácula], [Página] FROM Extractos WHERE ID = @ID", Conexion);
                    OleDbCommand Comando = new OleDbCommand("GetExtracto", Conexion);
                    Comando.CommandType  = System.Data.CommandType.StoredProcedure;

                    Comando.Parameters.AddWithValue("@ID", IDExtracto);

                    OleDbDataReader Reader = Comando.ExecuteReader();

                    if (!Reader.Read())
                        Response.Redirect("~/Default.aspx");

                    CrearFicha(Reader["ID"].ToString(), Reader["Florilegio"].ToString(), Reader["Autor"].ToString(), Reader["Obra"].ToString(), Reader["Libro"].ToString(), Reader["Poema"].ToString(), Reader["VersoInicial"].ToString(), Reader["VersoFinal"].ToString(), Reader["Capítulo"].ToString(), Reader["Subcapítulo"].ToString(), Reader["Extracto"].ToString(), Reader["TLL"].ToString(), Reader["Vernácula"].ToString(), Reader["Página"].ToString());

                    GenerarPDF(Reader["Florilegio"].ToString());
                }

                // Activamos el enlace de descarga
                LinkDescargarPDF.Attributes.Add("href", "~/Descargas/" + "Extracto" + IDExtracto + ".pdf");
            }
            catch (Exception Excepcion)
            {
                new Log("EXTRACTO » EXCEPCIÓN EN GENERAR FICHA", Excepcion.Message);

                Response.Redirect("~/Default.aspx");
            }
        }

        /*
        ╔══════════════════╗
        ║     EXTRACTO     ║
        ╚══════════════════╝
        */

        // Ver anterior miniatura
        protected void ButtonAnteriorMiniatura_Click(object sender, EventArgs e)
        {
            try
            {
                string TituloFlorilegio = Miniatura.AlternateText.Replace(" ", "");

                int NumeroPaginas = ObtenerNumeroPaginas();

                int PaginaActual = Convert.ToInt32(Miniatura.ImageUrl.Replace("~/Images/" + TituloFlorilegio + "/", "").Replace(".jpg", ""));

                PaginaActual = PaginaActual == 1 ? NumeroPaginas : PaginaActual - 1;

                Miniatura.ImageUrl = "~/Images/" + TituloFlorilegio + "/" + PaginaActual + ".jpg";

                LinkMiniaturaPDF.NavigateUrl = "~/PDF/" + TituloFlorilegio + "/" + PaginaActual + ".pdf";

                // Actualizamos el índice
                MiniaturaActual.Text = PaginaActual + " - " + ObtenerNumeroPaginas();
            }
            catch
            {
                // No hacemos nada
            }
        }

        // Ver siguiente miniatura
        protected void ButtonSiguienteMiniatura_Click(object sender, EventArgs e)
        {
            try
            {
                string TituloFlorilegio = Miniatura.AlternateText.Replace(" ", "");
                string Ruta = HttpContext.Current.Server.MapPath("~/PDF") + "/" + TituloFlorilegio;

                int NumeroPaginas = Directory.GetFiles(Ruta, "*", SearchOption.TopDirectoryOnly).Length;

                int PaginaActual = Convert.ToInt32(Miniatura.ImageUrl.Replace("~/Images/" + TituloFlorilegio + "/", "").Replace(".jpg", ""));

                PaginaActual = PaginaActual == NumeroPaginas ? 1 : PaginaActual + 1;

                Miniatura.ImageUrl = "~/Images/" + TituloFlorilegio + "/" + PaginaActual + ".jpg";

                LinkMiniaturaPDF.NavigateUrl = "~/PDF/" + TituloFlorilegio + "/" + PaginaActual + ".pdf";

                // Actualizamos el índice
                MiniaturaActual.Text = PaginaActual + " - " + ObtenerNumeroPaginas();
            }
            catch
            {
                // No hacemos nada
            }
        }

        /*
        ╔══════════════════╗
        ║     AUXILIAR     ║
        ╚══════════════════╝
        */

        private void CrearFicha(string ID, string Florilegio, string Autor, string Obra, string Libro, string Poema, string VersoInicial, string VersoFinal, string Capitulo, string Subcapitulo, string Extracto, string TLL, string Vernacula, string Pagina)
        {
            // Apartados de la primera columna de la ficha del extracto
            string[] ColumnaIzquierda = {
                "ID",
                Resources.General.Florilegio,
                Resources.General.Autor,
                Resources.General.Obra,
                Resources.General.Libro,
                Resources.General.Poema,
                Resources.General.Capitulo,
                Resources.General.Subcapitulo,
                Resources.General.VersoInicial,
                Resources.General.VersoFinal,
                Resources.General.Extracto,
                Resources.General.EdicionModerna,
                Resources.General.Vernacula,
                Resources.General.Pagina
            };

            // Apartados de la segunda columna de la ficha del extracto
            string[] ColumnaDerecha = { ID, Florilegio, Autor, Obra, Libro, Poema, Capitulo, Subcapitulo, VersoInicial, VersoFinal, Extracto, TLL, Vernacula, Pagina };

            // Rellenamos la ficha
            for (int i = 0; i < 14; i++)
            {
                // Campo sin contenido
                if (ColumnaDerecha[i] == "" || ColumnaDerecha[i] == "&nbsp;")
                    continue;

                // La obra Ø no aparece en la ficha
                if (i == 3 && (ColumnaDerecha[i] == "Ø"))
                    continue;

                // Los campos Libro, Poema, VersoInicial, VersoFinal que valgan 0 no aparecen en la ficha
                if ((i == 4 || i == 5 || i == 8 || i == 9) && (ColumnaDerecha[i] == "0"))
                    continue;

                TableRow Fila = new TableRow();

                TableCell Celda1 = new TableCell { Text = ColumnaIzquierda[i] };
                Fila.Cells.Add(Celda1);

                TableCell Celda2 = new TableCell { Text = ColumnaDerecha[i].Replace("/", "<br />") };
                Fila.Cells.Add(Celda2);

                Ficha.Rows.Add(Fila);
            }


            // Actualizamos la ruta de la miniatura
            Miniatura.ImageUrl = "~/Images/" + Florilegio.Replace(" ", "") + "/" + Pagina + ".jpg";

            // Guardamos el título del florilegio para ser utilizado en otros métodos
            Miniatura.AlternateText = Florilegio;

            // Actualizamos el índice
            MiniaturaActual.Text = Pagina + " - " + ObtenerNumeroPaginas();

            // Actualizamos la ruta del link del PDF de la página actual
            LinkMiniaturaPDF.NavigateUrl = "~/PDF/" + Florilegio.Replace(" ", "") + "/" + Pagina + ".pdf";
        }

        protected void GenerarPDF(string Florilegio)
        {
            try
            {
                new FicheroPDF("Extracto" + IDExtracto, Florilegio).Generar(IDExtracto);
            }
            catch (Exception Excepcion)
            {
                new Log("EXTRACTO » EXCEPCIÓN EN GENERAR PDF", Excepcion.Message);
            }
        }

        private string ObtenerTituloFlorilegio(string ID)
        {
            using (OleDbConnection Conexion = new OleDbConnection(CadenaConexion))
            {
                Conexion.Open();

                OleDbCommand Comando = new OleDbCommand("SELECT Florilegio FROM Extractos WHERE Alta=True AND ID = @ID", Conexion);

                Comando.Parameters.AddWithValue("@ID", ID);

                OleDbDataReader Reader = Comando.ExecuteReader();

                return Reader.Read() ? Reader["Florilegio"].ToString() : "";
            }
        }

        private int ObtenerNumeroPaginas()
        {
            string TituloFlorilegio = Miniatura.AlternateText.Replace(" ", "");

            string Ruta = HttpContext.Current.Server.MapPath("~/PDF") + "/" + TituloFlorilegio;

            return Directory.GetFiles(Ruta, "*", SearchOption.TopDirectoryOnly).Length;
        }
    }
}