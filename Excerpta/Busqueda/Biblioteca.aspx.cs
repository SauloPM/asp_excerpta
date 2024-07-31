using System;
using System.Web;
using System.Web.UI;
using Busqueda.App_Code;
using System.Web.UI.WebControls;

namespace Busqueda
{
    public partial class Biblioteca : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Ítem activo del navbar
            Master.NavHojear_Active     = "";
            Master.NavBuscar_Active     = "";
            Master.NavBiblioteca_Active = "active";
            Master.NavCreditos_Active   = "";

            if (!Page.IsPostBack)
            {
                GenerarPDF();

                // Activamos el enlace de descarga
                LinkDescargarPDF.Attributes.Add("href", "~/Descargas/Biblioteca.pdf");
            }
        }

        /*
        ╔════════════════════════════════╗
        ║     FUENTES BIBLIOGRÁFICAS     ║
        ╚════════════════════════════════╝
        */

        // Recuento
        protected void DataBiblioteca_Selected(object sender, SqlDataSourceStatusEventArgs e)
        {
            Recuento.Text = e.AffectedRows.ToString();
        }

        // Formato
        protected void TablaBiblioteca_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                try
                {
                    // Cambiamos el ID por iconos
                    e.Row.Cells[0].Text = "<i class='icono fa fa-bookmark' aria-hidden='true'></i>" + "<i class='icono fa fa-bookmark-o' aria-hidden='true'></i>";
                }
                catch
                {

                }
            }
        }

        // Seleccionar letra inicial
        protected void DropLetras_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataBiblioteca.SelectCommand = (DropLetras.SelectedIndex == 0) ? "SELECT ID, [Bibliografía] FROM Biblioteca ORDER BY [Bibliografía]" : "SELECT ID, [Bibliografía] FROM Biblioteca WHERE [Bibliografía] LIKE '" + DropLetras.SelectedValue + "%' ORDER BY [Bibliografía]";

            // Refrescamos
            DataBiblioteca .DataBind();
            TablaBiblioteca.DataBind();

            GenerarPDF();

            // Activamos el enlace de descarga
            LinkDescargarPDF.Attributes.Add("href", "~/Descargas/Biblioteca.pdf");
        }

        /*
        ╔══════════════════╗
        ║     AUXILIAR     ║
        ╚══════════════════╝
        */

        protected void GenerarPDF()
        {
            try
            {
                new FicheroPDF("Biblioteca", "Fuentes bibliográficas", TablaBiblioteca.Rows.Count.ToString()).Generar(DataBiblioteca.SelectCommand);
            }
            catch (Exception Excepcion)
            {
                new Log("BIBLIOTECA » EXCEPCIÓN EN GENERAR PDF", Excepcion.Message);
            }
        }
    }
}