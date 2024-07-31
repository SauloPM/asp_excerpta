using System;
using System.IO;
using System.Net;
using System.Web;
using System.Linq;
using System.Web.UI;
using System.Data.OleDb;
using Busqueda.App_Code;
using System.Web.UI.WebControls;
using System.Diagnostics;

namespace Busqueda
{
    public partial class Hojear : Page
    {
        // Cadena de conexión
        //readonly string CadenaConexion = "Provider = Microsoft.Jet.OLEDB.4.0; Data Source = " + HttpContext.Current.Server.MapPath("~/App_Data") + @"\excerpta.mdb";
        private static readonly String CadenaConexion = "Provider = Microsoft.ACE.OLEDB.12.0; Data Source = " + @"c:\inetpub\excerpta\excerpta.accdb;";

        protected void Page_Load(object sender, EventArgs e)
        {
            // Ítem activo del navbar
            Master.NavHojear_Active     = "active";
            Master.NavBuscar_Active     = "";
            Master.NavBiblioteca_Active = "";
            Master.NavCreditos_Active   = "";
        }

        /*
        ╔════════════════╗
        ║     FILTRO     ║
        ╚════════════════╝
        */

        protected void DropFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (DropFiltro.SelectedValue)
            {
                case "Autor":
                    Florilegios.Visible = false;
                    Autores.Visible     = true;
                    Obras.Visible       = false;
                    break;
                case "Obra":
                    Florilegios.Visible = false;
                    Autores.Visible     = false;
                    Obras.Visible       = true;
                    break;
                default:
                    Florilegios.Visible = true;
                    Autores.Visible     = false;
                    Obras.Visible       = false;
                    break;
            }

            UpdateHojear.Update();
        }

        /*
        ╔═════════════════════╗
        ║     FLORILEGIOS     ║
        ╚═════════════════════╝
        */

        // Recuento
        protected void DataFlorilegios_Selected(object sender, SqlDataSourceStatusEventArgs e)
        {
            RecuentoFlorilegios.Text = e.AffectedRows.ToString();
        }

        // Abrir / ordenar
        protected void TablaFlorilegios_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            // Abrir
            if (e.CommandName.ToString() == "Abrir")
            {
                try
                {
                    int Fila = int.Parse(e.CommandArgument.ToString());

                    DataCapitulos.SelectCommand = "SELECT DISTINCT [Capítulo], COUNT([Capítulo]) AS Recuento FROM Extractos WHERE Alta=True AND Florilegio = '" + WebUtility.HtmlDecode(TablaFlorilegios.Rows[Fila].Cells[0].Text) + "' GROUP BY [Capítulo] HAVING COUNT([Capítulo]) > 0 ORDER BY Capítulo";

                    TablaCapitulos.DataBind();
                    DataCapitulos .DataBind();

                    Filtro     .Visible = false;
                    Florilegios.Visible = false;
                    Capitulos  .Visible = true;

                    ItemSeleccionado.Text = WebUtility.HtmlDecode(TablaFlorilegios.Rows[Fila].Cells[0].Text);

                    UpdateHojear.Update();
                }
                catch (Exception Excepcion)
                {
                    new Log("HOJEAR » EXCEPCIÓN EN SELECCIONAR FLORILEGIO", Excepcion.Message);
                }
            }

            // Ordenar
            else
            {
                DataFlorilegios.SelectCommand = "SELECT DISTINCT Florilegio, COUNT(Florilegio) AS Recuento FROM Extractos WHERE Alta=True GROUP BY Florilegio HAVING COUNT(Florilegio) > 0 ORDER BY Florilegio";

                DataFlorilegios.DataBind();
                TablaFlorilegios.DataBind();

                UpdateHojear.Update();
            }
        }

        /*
        ╔═════════════════╗
        ║     AUTORES     ║
        ╚═════════════════╝
        */

        // Recuento
        protected void DataAutores_Selected(object sender, SqlDataSourceStatusEventArgs e)
        {
            RecuentoAutores.Text = e.AffectedRows.ToString();
        }

        // Abrir / ordenar
        protected void TablaAutores_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            // Abrir
            if (e.CommandName.ToString() == "Abrir")
            {
                try
                {
                    int Fila = int.Parse(e.CommandArgument.ToString());

                    DataCapitulos.SelectCommand = "SELECT DISTINCT [Capítulo], COUNT([Capítulo]) AS Recuento FROM Extractos WHERE Alta=True AND Autor = '" + WebUtility.HtmlDecode(TablaAutores.Rows[Fila].Cells[0].Text) + "' GROUP BY [Capítulo] HAVING COUNT([Capítulo]) > 0 ORDER BY [Capítulo]";
                    
                    TablaCapitulos.DataBind();
                    DataCapitulos .DataBind();

                    Filtro   .Visible = false;
                    Autores  .Visible = false;
                    Capitulos.Visible = true;

                    ItemSeleccionado.Text = WebUtility.HtmlDecode(TablaAutores.Rows[Fila].Cells[0].Text);

                    UpdateHojear.Update();
                }
                catch (Exception Excepcion)
                {
                    new Log("HOJEAR » EXCEPCIÓN EN SELECCIONAR AUTOR", Excepcion.Message);
                }
            }

            // Ordenar
            else
            {
                DataAutores.SelectCommand = "SELECT DISTINCT Autor, COUNT(Autor) AS Recuento FROM Extractos WHERE Alta=True GROUP BY Autor HAVING COUNT(Autor) > 0 ORDER BY Autor";

                DataAutores.DataBind();
                TablaAutores.DataBind();

                UpdateHojear.Update();
            }
        }

        /*
        ╔═══════════════╗
        ║     OBRAS     ║
        ╚═══════════════╝
        */

        // Recuento
        protected void DataObras_Selected(object sender, SqlDataSourceStatusEventArgs e)
        {
            RecuentoObras.Text = e.AffectedRows.ToString();
        }

        // Formato
        protected void TablaObras_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                // Cambiamos Ø por nada
                if (e.Row.Cells[1].Text == "&#216;")
                    e.Row.Cells[1].Text = "";
            }
            catch
            {

            }
        }

        // Abrir / ordenar
        protected void TablaObras_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            // Abrir
            if (e.CommandName.ToString() == "Abrir")
            {
                try
                {
                    int Fila = int.Parse(e.CommandArgument.ToString());
                    string Obra = WebUtility.HtmlDecode(TablaObras.Rows[Fila].Cells[1].Text.Trim()) == "" ? "Ø" : WebUtility.HtmlDecode(TablaObras.Rows[Fila].Cells[0].Text);

                    DataCapitulos.SelectCommand = "SELECT DISTINCT [Capítulo], COUNT([Capítulo]) AS Recuento FROM Extractos WHERE Alta=True AND Obra = '" + Obra + "' GROUP BY [Capítulo] HAVING COUNT([Capítulo]) > 0 ORDER BY [Capítulo]";

                    TablaCapitulos.DataBind();
                    DataCapitulos .DataBind();

                    Filtro   .Visible = false;
                    Obras    .Visible = false;
                    Capitulos.Visible = true;

                    ItemSeleccionado.Text = Obra;

                    UpdateHojear.Update();
                }
                catch (Exception Excepcion)
                {
                    new Log("HOJEAR » EXCEPCIÓN EN SELECCIONAR OBRA", Excepcion.Message);
                }
            }

            // Ordenar
            else
            {
                DataObras.SelectCommand = "SELECT DISTINCT Autor, Obra, COUNT(Obra) AS Recuento FROM Extractos WHERE Alta=True GROUP BY Autor, Obra HAVING COUNT(Obra) > 0 ORDER BY Obra";

                DataObras.DataBind();
                TablaObras.DataBind();

                UpdateHojear.Update();
            }
        }

        /*
        ╔═══════════════════╗
        ║     CAPÍTULOS     ║
        ╚═══════════════════╝
        */

        // Recuento
        protected void DataCapitulos_Selected(object sender, SqlDataSourceStatusEventArgs e)
        {
            RecuentoCapitulos.Text = e.AffectedRows.ToString();
        }

        // Volver
        protected void ButtonCapitulosVolver_Click(object sender, EventArgs e)
        {
            DataCapitulos.SelectCommand = "";

            DataCapitulos .DataBind();
            TablaCapitulos.DataBind();

            switch (DropFiltro.SelectedValue)
            {
                case "Autor":
                    Florilegios.Visible = false;
                    Autores    .Visible = true;
                    Obras      .Visible = false;
                    break;

                case "Obra":
                    Florilegios.Visible = false;
                    Autores    .Visible = false;
                    Obras      .Visible = true;
                    break;

                default:
                    Florilegios.Visible = true;
                    Autores    .Visible = false;
                    Obras      .Visible = false;
                    break;
            }

            Filtro   .Visible = true;
            Capitulos.Visible = false;
            Extractos.Visible = false;

            ItemSeleccionado.Text = string.Empty;

            UpdateHojear.Update();
        }

        // Abrir / ordenar
        protected void TablaCaptiulos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                // Abrir capítulo
                if (e.CommandName.ToString() == "Abrir")
                {
                    Identificadores.Text = "";

                    int Fila = int.Parse(e.CommandArgument.ToString());

                    string ConsultaSQL = "SELECT ID, Florilegio, Autor, Obra, Libro, Poema, VersoInicial, VersoFinal, [Capítulo], [Subcapítulo], Extracto, TLL, [Vernácula], [Página] FROM Extractos WHERE Alta=True AND " + DropFiltro.SelectedValue + " = '" + ItemSeleccionado.Text + "' AND [Capítulo] = '" + EliminarEspaciosInnecesarios(WebUtility.HtmlDecode(TablaCapitulos.Rows[Fila].Cells[0].Text)) + "' ORDER BY [Página]";

                    // Recuento
                    ExtractoActual.Text = "1";
                    RecuentoExtractos.Text = TablaCapitulos.Rows[Fila].Cells[1].Text;

                    using (OleDbConnection Conexion = new OleDbConnection(CadenaConexion))
                    {
                        Conexion.Open();

                        OleDbCommand Comando = new OleDbCommand(ConsultaSQL, Conexion);

                        OleDbDataReader Reader = Comando.ExecuteReader();

                        if (Reader.Read())
                        {
                            Identificadores.Text += Reader["ID"].ToString();

                            // Almacenamos el ID de todos los extractos en una lista
                            while (Reader.Read())
                                Identificadores.Text += "," + Reader["ID"].ToString();
                        }
                        else
                            Response.Redirect("~/Default.aspx");

                        string[] IDs = Identificadores.Text.Split(',');
                        CrearFicha(IDs.First());
                    }

                    // Ocultamos las opciones de navegación si solo existe un extracto
                    ButonAnteriorExtracto .Visible = Convert.ToInt32(RecuentoExtractos.Text) > 1 ? true : false;
                    ButonSiguienteExtracto.Visible = Convert.ToInt32(RecuentoExtractos.Text) > 1 ? true : false;

                    // Mostramos y ocultamos según corresponda
                    Capitulos.Visible = false;
                    Extractos.Visible = true;

                    UpdateHojear.Update();
                }

                // Ordenar tabla
                else
                {
                    DataCapitulos.SelectCommand = "SELECT DISTINCT [Capítulo], COUNT([Capítulo]) AS Recuento FROM Extractos WHERE Alta=True AND " + DropFiltro.SelectedValue + " = '" + ItemSeleccionado.Text + "' GROUP BY [Capítulo] HAVING COUNT([Capítulo]) > 0 ORDER BY [Capítulo]";

                    DataCapitulos .DataBind();
                    TablaCapitulos.DataBind();

                    UpdateHojear.Update();
                }
            }
            catch (Exception Excepcion)
            {
                new Log("HOJEAR » EXCEPCIÓN EN SELECCIONAR CAPÍTULO", Excepcion.Message);

                Response.Redirect("~/Default.aspx");
            }
        }

        /*
        ╔═══════════════════╗
        ║     EXTRACTOS     ║
        ╚═══════════════════╝
        */

        // Volver
        protected void ButtonExtractosVolver_Click(object sender, EventArgs e)
        {
            Capitulos.Visible = true;
            Extractos.Visible = false;

            UpdateHojear.Update();
        }

        // Ver anterior extracto
        protected void ButonAnteriorExtracto_Click(object sender, EventArgs e)
        {
            try
            {
                string[] IDs = Identificadores.Text.Split(',').Where(x => !string.IsNullOrEmpty(x)).ToArray();

                ExtractoActual.Text = ExtractoActual.Text == "1" ? RecuentoExtractos.Text : (Convert.ToInt32(ExtractoActual.Text) - 1).ToString();

                int i = Convert.ToInt32(ExtractoActual.Text) - 1;

                CrearFicha(IDs[i]);

                UpdateHojear.Update();
            }
            catch (Exception Excepcion)
            {
                new Log("HOJEAR » EXCEPCIÓN EN NAVEGACIÓN DE EXTRACTOS", Excepcion.Message);

                Response.Redirect("~/Default.aspx");
            }
        }

        // Ver siguiente extracto
        protected void ButonSiguienteExtracto_Click(object sender, EventArgs e)
        {
            try
            {
                string[] IDs = Identificadores.Text.Split(',');

                ExtractoActual.Text = ExtractoActual.Text == RecuentoExtractos.Text ? "1" : (Convert.ToInt32(ExtractoActual.Text) + 1).ToString();

                int i = Convert.ToInt32(ExtractoActual.Text) - 1;

                CrearFicha(IDs[i]);

                UpdateHojear.Update();
            }
            catch (Exception Excepcion)
            {
                new Log("HOJEAR » EXCEPCIÓN EN NAVEGACIÓN DE EXTRACTOS", Excepcion.Message);

                Response.Redirect("~/Default.aspx");
            }
        }

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

                MiniaturaActual.Text = PaginaActual + " - " + ObtenerNumeroPaginas();
            }
            catch (Exception Excepcion)
            {
                new Log("HOJEAR » EXCEPCIÓN EN NAVEGACIÓN DE MINIATURAS", Excepcion.Message);

                Response.Redirect("~/Default.aspx");
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

                MiniaturaActual.Text = PaginaActual + " - " + ObtenerNumeroPaginas();
            }
            catch (Exception Excepcion)
            {
                new Log("HOJEAR » EXCEPCIÓN EN NAVEGACIÓN DE MINIATURAS", Excepcion.Message);

                Response.Redirect("~/Default.aspx");
            }
        }

        /*
        ╔══════════════════╗
        ║     AUXILIAR     ║
        ╚══════════════════╝
        */

        private void CrearFicha (string ID)
        {
            // Apartados de la ficha (primera columna de la tabla)
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
            string[] ColumnaDerecha = ObtenerCamposExtracto(ID);

            string Florilegio = ColumnaDerecha[1 ];
            string Pagina     = ColumnaDerecha[13];

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

                TableCell Celda2 = new TableCell { Text = ColumnaDerecha[i].Replace("/", "<br />").Replace("\n", "<br />") };
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

            // Generamos el fichero PDF
            GenerarPDF(Florilegio, ID);
        }

        protected void GenerarPDF(string Florilegio, string ID)
        {
            try
            {
                new FicheroPDF("Extractos", Florilegio, RecuentoExtractos.Text).Generar(ID);

                // Activamos el enlace de descarga
                LinkDescargarPDF.Attributes.Add("href", "~/Descargas/" + "Extractos.pdf");
            }
            catch (Exception Excepcion)
            {
                new Log("HOJEAR » EXCEPCIÓN EN GENERAR PDF", Excepcion.Message);
            }
        }

        private string[] ObtenerCamposExtracto(string ID)
        {
            using (OleDbConnection Conexion = new OleDbConnection(CadenaConexion))
            {
                Conexion.Open();

                // OleDbCommand Comando = new OleDbCommand("SELECT ID, Florilegio, Autor, Obra, Libro, Poema, VersoInicial, VersoFinal, [Capítulo], [Subcapítulo], Extracto, TLL, [Vernácula], [Página] FROM Extractos WHERE ID = @ID", Conexion);
                OleDbCommand Comando = new OleDbCommand("GetExtracto", Conexion);
                Comando.CommandType  = System.Data.CommandType.StoredProcedure;

                Comando.Parameters.AddWithValue("@ID", ID);

                OleDbDataReader Reader = Comando.ExecuteReader();

                if (Reader.Read())
                    return new string[] { Reader["ID"].ToString(), Reader["Florilegio"].ToString(), Reader["Autor"].ToString(), Reader["Obra"].ToString(), Reader["Libro"].ToString(), Reader["Poema"].ToString(), Reader["Capítulo"].ToString(), Reader["Subcapítulo"].ToString(), Reader["VersoInicial"].ToString(), Reader["VersoFinal"].ToString(), Reader["Extracto"].ToString(), Reader["TLL"].ToString(), Reader["Vernácula"].ToString(), Reader["Página"].ToString() };
            }

            return null;
        }

        private int ObtenerNumeroPaginas()
        {
            string TituloFlorilegio = Miniatura.AlternateText.Replace(" ", "");

            string Ruta = HttpContext.Current.Server.MapPath("~/PDF") + "/" + TituloFlorilegio;

            return Directory.GetFiles(Ruta, "*", SearchOption.TopDirectoryOnly).Length;
        }

        private string EliminarEspaciosInnecesarios(string Frase)
        {
            // Espacios del principio y final
            Frase = Frase.Trim();

            // Espacios intermedios
            while (Frase.IndexOf("  ") > -1)
                Frase = Frase.Replace("  ", " ");

            Frase = Frase.Replace("&nbsp;", "");

            return Frase;
        }
    }
}