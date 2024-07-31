using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Busqueda
{
    public partial class Buscar : Page
    {
        // El directorio inetpub lo hemos creado manualmente en local porque en producción es el que se utiliza
        string RutaFichero = @"C:\inetpub\ExcerptaLastUpdate\fecha-ultima-actualizacion.txt";

        protected void Page_Load(object sender, EventArgs e)
        {
            // Ítem activo del navbar
            Master.NavHojear_Active     = "";
            Master.NavBuscar_Active     = "active";
            Master.NavBiblioteca_Active = "";
            Master.NavCreditos_Active   = "";

            try
            {
                System.IO.StreamReader Fichero = new System.IO.StreamReader(RutaFichero);
                UltimaActualizacion.Text = Fichero.ReadLine();
                Fichero.Close();
            }
            catch (Exception exception)
            {
                string Mensaje = exception.Message;
            }
        }

        // ─────────────────────── //
        //     BÚSQUEDA SIMPLE     //
        // ─────────────────────── //

        // Sincronización en cascada del desplegable de autores
        protected void DropFlorilegios_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropAutores.Items.Clear();

                // Selected item
                ListItem Item = new ListItem
                {
                    Text = "Todos los autores",
                    Value = "%",
                    Selected = true
                };

                DropAutores.Items.Add(Item);

                // Refrescamos
                DataAutores.SelectCommand = "SELECT DISTINCT Autor FROM Extractos WHERE ALTA=True AND Florilegio LIKE '" + DropFlorilegios.SelectedValue + "' ORDER BY Autor";
                DataAutores.DataBind();
                DropAutores.DataBind();

                // Sincronizamos el desplegable de obras
                DropAutores_SelectedIndexChanged(sender, e);
            }
            catch
            {
                // ...
            }
        }

        // Sincronización en cascada del desplegable de obras
        protected void DropAutores_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropObras.Items.Clear();

                // Selected item
                ListItem Item = new ListItem();
                //Item.Text = GetLocalResourceObject("dropObras.Text").ToString();
                Item.Text = "Todas las obras";
                Item.Value = "%";
                Item.Selected = true;

                DropObras.Items.Add(Item);

                // Refrescamos
                DataObras.SelectCommand = "SELECT DISTINCT Obra FROM Extractos WHERE ALTA=True AND Florilegio LIKE '" + DropFlorilegios.SelectedValue + "' AND Autor LIKE '" + DropAutores.SelectedValue + "' ORDER BY Obra";
                DropObras.DataBind();
                DataObras.DataBind();
            }
            catch
            {
                // ...
            }
        }

        // Búsqueda simple
        protected void ButtonBusquedaSimple_Click(object sender, EventArgs e)
        {
            try
            {
                ButtonLimpiarFiltroAvanzado_Click(sender, e);

                // Generamos los fragmentos de la query
                string QueryFlorilegio = "Florilegio LIKE '" + DropFlorilegios.SelectedValue + "'";
                string QueryAutor      = "Autor LIKE '" + DropAutores.SelectedValue + "'";
                string QueryObra       = "Obra LIKE '" + DropObras.SelectedValue + "'";
                string[] QueryPalabras = Palabras();
                string QueryFrases = Frases();

                string Join = QueryPalabras[0];

                // Ensamblamos los fragmentos
                DataExtractos.SelectCommand = "SELECT DISTINCT ID, Florilegio, Autor, Obra, [Capítulo], [Subcapítulo], Extracto, TLL FROM " + Join;
                DataExtractos.SelectCommand += " WHERE Alta=True AND " + QueryFlorilegio + " AND " + QueryAutor + " AND " + QueryObra;
                DataExtractos.SelectCommand += QueryPalabras[1] + QueryFrases + " ORDER BY Florilegio, Autor, Obra, [Capítulo], [Subcapítulo], Extracto, TLL";

                DataExtractos.SelectCommand = DataExtractos.SelectCommand.Replace("  ", " ");

                // Refrescamos
                DataExtractos.DataBind();
                TablaExtractos.DataBind();

                // Almacenamos la query en un buffer oculto (para evitar utilizar variables de sesión)
                InputBuffer.Text = DataExtractos.SelectCommand;
            }
            catch (Exception exception)
            {
                string mensaje = exception.Message;
                ButtonLimpiarFiltroSimple_Click(sender, e);
            }
        }

        // Búsqueda de palabras
        private string[] Palabras()
        {
            string[] query = { "Extractos", string.Empty };

            // No hemos escrito nada
            if (InputPalabras.Text.Trim() == string.Empty)
                return query;

            try
            {
                // Almacenamos las palabras en un array
                char[] Separador = { ' ' };
                string[] Palabras = InputPalabras.Text.Trim().ToLower().Replace('*', '%').Replace('?', '_').Split(Separador, StringSplitOptions.RemoveEmptyEntries);

                // Operador OR
                if (CheckPalabras.SelectedValue.Equals("OR"))
                {
                    // Analizamos cada palabra por separado.
                    foreach (string Palabra in Palabras)
                        query[1] += "Palabra LIKE '" + Palabra.Trim() + "' OR ";

                    // Eliminamos el fragmento final " OR " y ensamblamos todo
                    query[1] = " AND ( " + query[1].Substring(0, query[1].Length - 4) + " )";

                    // JOIN
                    query[0] += " INNER JOIN Palabras ON Extractos.ID = Palabras.ExtractoID ";
                }

                // Operador AND
                else
                {
                    // Analizamos cada palabra insertada por separado.
                    int i = 0;
                    foreach (string Palabra in Palabras)
                    {
                        query[0] += " INNER JOIN Palabras AS P" + i + " ON Extractos.ID = P" + i + ".ExtractoID) ";
                        query[1] += "P" + i + ".Palabra LIKE '" + Palabra.Trim() + "' AND ";
                        i++;
                    }

                    string parentesis = new string('(', Palabras.Length);
                    query[0] = " " + parentesis + query[0];

                    // Eliminamos el fragmento final " AND " y ensamblamos todo
                    query[1] = " AND " + query[1].Substring(0, query[1].Length - 5);
                }
            }
            catch (Exception)
            {
                // ...
            }

            return query;
        }

        // Búsqueda aproximada
        private string Frases()
        {
            string query = string.Empty;

            // No hemos escrito nada
            if (InputFrases.Text.Trim() == string.Empty)
                return query;

            // Almacenamos las frases en un array
            char[] Separador = { '/' };
            string[] Frases = InputFrases.Text.Trim().ToLower().Replace('*', '%').Replace('?', '_').Split(Separador, StringSplitOptions.RemoveEmptyEntries);

            // Analizamos cada frase por separado.
            foreach (string Frase in Frases)
                query += " ( [Capítulo] LIKE '" + Frase.Trim() + "' OR [Subcapítulo] LIKE '" + Frase.Trim() + "' OR Extracto LIKE '" + Frase.Trim() + "' OR TLL LIKE '" + Frase.Trim() + "' )  " + CheckFrases.SelectedValue;

            // Eliminamos el fragmento final " AND " u " OR  " y ensamblamos todo
            return " AND " + query.Substring(0, query.Length - 3);
        }

        // Limpiar filtro (tanto de la búsqueda simple como avanzada)
        protected void ButtonLimpiarFiltroSimple_Click(object sender, EventArgs e)
        {
            try
            {
                // Desplegables
                DropFlorilegios.SelectedIndex = 0;
                DropAutores.SelectedIndex = 0;
                DropObras.SelectedIndex = 0;

                // Checks
                CheckFrases.SelectedIndex = 0;
                CheckPalabras.SelectedIndex = 0;

                // Inputs
                InputBuffer.Text = string.Empty;
                InputFrases.Text = string.Empty;
                InputPalabras.Text = string.Empty;

                // Refrescamos
                DataExtractos.SelectCommand = "SELECT ID, Florilegio, Autor, Obra, [Capítulo], [Subcapítulo], Extracto, TLL FROM Extractos WHERE Alta=True ORDER BY Florilegio, Autor, Obra";
                DataExtractos.DataBind();
                TablaExtractos.DataBind();
            }
            catch (Exception exception)
            {
                string mensaje = exception.Message;
            }
        }

        // ───────────────────────── //
        //     BÚSQUEDA AVANZADA     //
        // ───────────────────────── //

        // Búsqueda avanzada
        protected void ButtonBusquedaAvanzada_Click(object sender, EventArgs e)
        {
            try
            {
                ButtonLimpiarFiltroSimple_Click(sender, e);

                // Si no hemos escrito ningún florilegio, el operador NOT que lo precede no tendrá efecto
                if (InputFlorilegio.Text.Trim() == "")
                    ANDOR1.SelectedValue = "";

                // Almacenamos la query en un buffer oculto (para evitar utilizar variables de sesión)
                InputBuffer.Text = "SELECT DISTINCT ID, Florilegio, Autor, Obra, [Capítulo], [Subcapítulo], Extracto, TLL FROM Extractos WHERE Alta=True AND " + ANDOR1.SelectedValue + " " + EntradaDatos(InputFlorilegio, "Florilegio") + " " + ANDOR2.SelectedValue + " " + EntradaDatos(InputAutor, "Autor") + " " + ANDOR3.SelectedValue + " " + EntradaDatos(InputObra, "Obra") + " " + ANDOR4.SelectedValue + " " + EntradaDatos(InputCapitulo, "Capítulo") + " " + ANDOR5.SelectedValue + " " + EntradaDatos(InputSubcapitulo, "Subcapítulo") + " " + ANDOR6.SelectedValue + " " + EntradaDatos(InputExtracto, "Extracto") + " " + ANDOR7.SelectedValue + " " + EntradaDatos(InputTLL, "TLL") + " ORDER BY Florilegio, Autor, Obra";
                DataExtractos.SelectCommand = InputBuffer.Text;

                // Refrescamos
                DataExtractos.DataBind();
                TablaExtractos.DataBind();

            }
            catch
            {
                ButtonLimpiarFiltroAvanzado_Click(sender, e);
            }
        }

        private string EntradaDatos(TextBox EntradaDatos, string grupo)
        {
            // No hemos escrito nada
            if (EntradaDatos.Text.Trim() == string.Empty)
                return "(ID = ID)";

            // Si el nombre del campo contiene tildes, se utilizan corchetes para evitar errores
            grupo = (grupo == "Capítulo" || grupo == "Subcapítulo") ? "[" + grupo + "]" : grupo;

            // Formato y mapping de operadores
            string resultado = EntradaDatos.Text.Trim().Replace('*', '%').Replace('?', '_').Replace('\'', '\"');

            // Incorporamos el comienzo de la sentencia SQL
            resultado = grupo + " LIKE '" + resultado;

            // Sustituimos el carácter "/" por disyunciones SQL
            resultado = resultado.Replace(" / ", "' OR " + grupo + " LIKE '");
            resultado = resultado.Replace("/ ", "' OR " + grupo + " LIKE '");
            resultado = resultado.Replace(" /", "' OR " + grupo + " LIKE '");
            resultado = resultado.Replace("/", "' OR " + grupo + " LIKE '");

            // Sustituimos el carácter "&" por conjunciones SQL
            resultado = resultado.Replace(" & ", "' AND " + grupo + " LIKE '");
            resultado = resultado.Replace("& ", "' AND " + grupo + " LIKE '");
            resultado = resultado.Replace(" &", "' AND " + grupo + " LIKE '");
            resultado = resultado.Replace("&", "' AND " + grupo + " LIKE '");

            // Incorporamos los paréntesis y el final de la consulta
            return "(" + resultado + "')";
        }

        // Limpiar filtro (tanto de la búsqueda simple como avanzada)
        protected void ButtonLimpiarFiltroAvanzado_Click(object sender, EventArgs e)
        {
            try
            {
                // Desplegables
                ANDOR1.SelectedIndex = 0;
                ANDOR2.SelectedIndex = 0;
                ANDOR3.SelectedIndex = 0;
                ANDOR4.SelectedIndex = 0;
                ANDOR5.SelectedIndex = 0;
                ANDOR6.SelectedIndex = 0;
                ANDOR7.SelectedIndex = 0;

                // Inputs
                InputBuffer.Text = string.Empty;
                InputFlorilegio.Text = string.Empty;
                InputAutor.Text = string.Empty;
                InputObra.Text = string.Empty;
                InputCapitulo.Text = string.Empty;
                InputSubcapitulo.Text = string.Empty;
                InputExtracto.Text = string.Empty;
                InputTLL.Text = string.Empty;

                // Refrescamos
                DataExtractos.SelectCommand = "SELECT ID, Florilegio, Autor, Obra, [Capítulo], [Subcapítulo], Extracto, TLL FROM Extractos WHERE Alta=True ORDER BY Florilegio, Autor, Obra";
                DataExtractos.DataBind();
                TablaExtractos.DataBind();
            }
            catch (Exception exception)
            {
                string mensaje = exception.Message;
            }
        }

        // ───────────────── //
        //     EXTRACTOS     //
        // ───────────────── //

        // Recuento
        protected void DataExtractos_Selected(object sender, SqlDataSourceStatusEventArgs e)
        {
            Recuento.Text = e.AffectedRows.ToString();
        }

        // Formato
        protected void TablaExtractos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                // Cambiamos Ø por nada
                if (e.Row.Cells[2].Text == "&#216;") e.Row.Cells[2].Text = "";

                // Reducimos la extensión
                if (e.Row.Cells[3].Text.Length > 100)
                    e.Row.Cells[3].Text = e.Row.Cells[3].Text.Substring(0, 99) + "...";
                if (e.Row.Cells[4].Text.Length > 100)
                    e.Row.Cells[4].Text = e.Row.Cells[4].Text.Substring(0, 99) + "...";
                if (e.Row.Cells[5].Text.Length > 100)
                    e.Row.Cells[5].Text = e.Row.Cells[5].Text.Substring(0, 99) + "...";
                if (e.Row.Cells[6].Text.Length > 100)
                    e.Row.Cells[6].Text = e.Row.Cells[6].Text.Substring(0, 99) + "...";
            }
            catch
            {

            }
        }

        // Abrir extracto / ordenar tabla
        protected void TablaExtractos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            // Abrir extracto
            if (e.CommandName.ToString() == "Abrir")
            {
                // ...
            }

            // Ordenar tabla
            else
            {
                DataExtractos.SelectCommand = InputBuffer.Text == string.Empty ? DataExtractos.SelectCommand : InputBuffer.Text;

                // Refrescamos
                DataExtractos.DataBind();
                DataExtractos.DataBind();
            }
        }
    }
}