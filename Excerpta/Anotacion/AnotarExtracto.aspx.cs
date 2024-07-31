using System;
using System.Web;
using System.Web.UI;
using Excerpta.App_Code;
using System.Data.OleDb;
using System.Web.UI.WebControls;

namespace Excerpta
{
    public partial class AnotarExtracto : Page
    {
        private static readonly string CadenaConexion = "Provider = Microsoft.ACE.OLEDB.12.0; Data Source = " + @"c:\inetpub\excerpta\excerpta.accdb;";

        protected void Page_Load(object sender, EventArgs e)
        {
            // Sesión expirada o usuario no registrado
            Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
            if (Session["usuario"] == null)
                Response.Redirect("~/Default.aspx");

            // Reseteamos
            InputLibro.Attributes["placeholder"] = "";
            InputPoema.Attributes["placeholder"] = "";

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
            Master.navbarExtractos_Active      = "active";
            Master.navbarAdministracion_Active = "";
        }

        /*
        ╔═══════════════════╗
        ║     EXTRACTOS     ║
        ╚═══════════════════╝
        */

        // Crear
        protected void BotonCrearExtracto_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            try
            {
                new Extracto().Crear(DropFlorilegios.SelectedValue, DropAutores.SelectedValue, DropObras.SelectedValue, InputLibro.Text.Trim(), InputPoema.Text.Trim(), InputCapitulo.Text.Trim(), InputSubcapitulo.Text.Trim(), InputVersoInicial.Text.Trim(), InputVersoFinal.Text.Trim(), InputExtracto.Text.Trim(), InputTLL.Text.Trim(), InputVernacula.Text.Trim(), InputPagina.Text.Trim(), Session["usuario"].ToString());

                new Log(Session["usuario"].ToString(), "CREAR EXTRACTO EN EL FLORILEGIO '" + DropFlorilegios.SelectedValue + "'").RegistrarActividad();

                new Popup(Notificacion, "Operación completada", false);

                // Vaciamos parcialmente el formulario
                InputCapitulo   .Text = "";
                InputSubcapitulo.Text = "";
                InputExtracto   .Text = "";
                InputTLL        .Text = "";
                InputVernacula  .Text = "";
            }
            catch (Exception Excepcion)
            {
                new Log(Session["usuario"].ToString(), "EXCEPCIÓN EN CREAR EXTRACTO", Excepcion.Message).RegistrarActividad();

                new Popup(Notificacion);
            }
        }

        /*
        ╔══════════════════════╗
        ║     DESPLEGABLES     ║
        ╚══════════════════════╝
        */

        protected void DropFlorilegios_SelectedIndexChanged(object sender, EventArgs e)
        {
            SincronizarAutores();
        }

        protected void DropAutores_SelectedIndexChanged(object sender, EventArgs e)
        {
            SincronizarObras();
        }

        protected void DropObras_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropObras.SelectedIndex == 0)
                VaciarFormulario();
            else
                RellenarFormulario();
        }

        private void SincronizarAutores()
        {
            DropAutores.Items.Clear();

            if (DropFlorilegios.SelectedIndex > 0) DropAutores.Items.Add(new ListItem { Text = "Seleccionar autor", Value = "%", Selected = true });

            DropAutores.Enabled = DropFlorilegios.SelectedIndex > 0 ? true : false;

            DataAutores.SelectCommand = DropFlorilegios.SelectedIndex > 0 ? "SELECT DISTINCT Autor FROM Descripciones WHERE Florilegio = '" + DropFlorilegios.SelectedValue + "' ORDER BY Autor" : "";

            DataAutores.DataBind();
            DropAutores.DataBind();

            SincronizarObras();
        }

        private void SincronizarObras()
        {
            DropObras.Items.Clear();

            if (DropAutores.SelectedIndex > 0) DropObras.Items.Add(new ListItem { Text = "Seleccionar obra", Value = "%", Selected = true });

            DropObras.Enabled = DropAutores.SelectedIndex > 0 ? true : false;

            DataObras.SelectCommand = DropAutores.SelectedIndex > 0 ? "SELECT DISTINCT Obra FROM Descripciones WHERE Florilegio = '" + DropFlorilegios.SelectedValue + "' AND Autor = '" + DropAutores.SelectedValue + "' ORDER BY Obra" : "";

            DataObras.DataBind();
            DropObras.DataBind();

            VaciarFormulario();
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

                new Popup(Notificacion, "El verso inicial no puede ser mayor que el verso final");
            }
        }

        // ¿Extracto repetido?
        protected void ValidarExtractoRepetido(object source, ServerValidateEventArgs args)
        {
            try
            {
                if (new Extracto().ExisteExtracto(DropFlorilegios.SelectedValue, DropAutores.SelectedValue, DropObras.SelectedValue, InputLibro.Text.Trim(), InputPoema.Text.Trim(), InputCapitulo.Text.Trim(), InputSubcapitulo.Text.Trim(), InputVersoInicial.Text.Trim(), InputVersoFinal.Text.Trim()))
                {
                    args.IsValid = false;

                    new Popup(Notificacion, "Extracto repetido");
                }
            }
            catch(Exception Excepcion)
            {
                args.IsValid = false;

                new Log(Session["usuario"].ToString(), "EXCEPCIÓN EN COMPROBAR SI UN EXTRACTO ESTABA REPETIDO", Excepcion.Message).RegistrarActividad();

                new Popup(Notificacion);
            }
        }

        /*
        ╔══════════════════╗
        ║     AUXILIAR     ║
        ╚══════════════════╝
        */

        private void RellenarFormulario()
        {
            try
            {
                using (OleDbConnection Conexion = new OleDbConnection(CadenaConexion))
                {
                    Conexion.Open();

                    OleDbCommand Comando = new OleDbCommand("SELECT Libro, Poema FROM Descripciones WHERE Florilegio = @Florilegio AND Autor = @Autor AND Obra = @Obra", Conexion);

                    Comando.Parameters.AddWithValue("@Florilegio", DropFlorilegios.SelectedValue);
                    Comando.Parameters.AddWithValue("@Autor"     , DropAutores    .SelectedValue);
                    Comando.Parameters.AddWithValue("@Obra"      , DropObras      .SelectedValue);

                    OleDbDataReader Reader = Comando.ExecuteReader();

                    if (Reader.Read())
                    {
                        // Separamos los índices
                        string[] indicesLibro = Reader["Libro"].ToString().Split(';');
                        string[] indicesPoema = Reader["Poema"].ToString().Split(';');

                        // Inicializamos los índices
                        int minLibro = Convert.ToInt32(indicesLibro[0]);
                        int maxLibro = Convert.ToInt32(indicesLibro[1]);
                        int minPoema = Convert.ToInt32(indicesPoema[0]);
                        int maxPoema = Convert.ToInt32(indicesPoema[1]);

                        // Activamos el resto del formulario
                        InputLibro       .Enabled = true;
                        InputPoema       .Enabled = true;
                        InputVersoInicial.Enabled = true;
                        InputVersoFinal  .Enabled = true;
                        InputCapitulo    .Enabled = true;
                        InputSubcapitulo .Enabled = true;
                        InputExtracto    .Enabled = true;
                        InputTLL         .Enabled = true;
                        InputVernacula   .Enabled = true;
                        InputPagina      .Enabled = true;
                        BotonGuardar     .Enabled = true;

                        // Vaciamos los campos "Libro" y "Poema"
                        InputVersoInicial.Text = "";
                        InputVersoFinal  .Text = "";

                        // Seguimos leyendo (por si hubiera descripciones repetidas)
                        while (Reader.Read())
                        {
                            // Separamos los índices
                            indicesLibro = Reader["Libro"].ToString().Split(';');
                            indicesPoema = Reader["Poema"].ToString().Split(';');

                            // Reajustamos los índices
                            minLibro = (minLibro > Convert.ToInt32(indicesLibro[0])) ? Convert.ToInt32(indicesLibro[0]) : minLibro;
                            maxLibro = (maxLibro < Convert.ToInt32(indicesLibro[1])) ? Convert.ToInt32(indicesLibro[1]) : maxLibro;
                            minPoema = (minPoema > Convert.ToInt32(indicesPoema[0])) ? Convert.ToInt32(indicesPoema[0]) : minPoema;
                            maxPoema = (maxPoema < Convert.ToInt32(indicesPoema[1])) ? Convert.ToInt32(indicesPoema[1]) : maxPoema;
                        }

                        // Especificamos el rango al usuario a través del atributo placeholder
                        InputLibro.Attributes["placeholder"] = "mín. " + minLibro + ", máx. " + maxLibro;
                        InputPoema.Attributes["placeholder"] = "mín. " + minPoema + ", máx. " + maxPoema;
                    }
                }
            }
            catch (Exception Excepcion)
            {
                new Popup(Notificacion);

                new Log(Session["usuario"].ToString(), "EXCEPCIÓN EN RELLENAR EL FORMULARIO DE EXTRACTOS", Excepcion.Message).RegistrarActividad();
            }
        }

        private void VaciarFormulario()
        {
            // Vaciamos los campos
            InputLibro       .Text = "";
            InputPoema       .Text = "";
            InputVersoInicial.Text = "";
            InputVersoFinal  .Text = "";
            InputCapitulo    .Text = "";
            InputSubcapitulo .Text = "";
            InputExtracto    .Text = "";
            InputTLL         .Text = "";
            InputVernacula   .Text = "";
            InputPagina      .Text = "";

            // Desactivamos los campos
            InputLibro       .Enabled = false;
            InputPoema       .Enabled = false;
            InputVersoInicial.Enabled = false;
            InputVersoFinal  .Enabled = false;
            InputCapitulo    .Enabled = false;
            InputSubcapitulo .Enabled = false;
            InputExtracto    .Enabled = false;
            InputTLL         .Enabled = false;
            InputVernacula   .Enabled = false;
            InputPagina      .Enabled = false;
            BotonGuardar     .Enabled = false;
        }
    }
}