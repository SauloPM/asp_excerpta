using System.Web;
using System.Data.OleDb;

namespace Excerpta.App_Code
{
    public class Florilegio
    {
        // Atributos
        private static readonly string CadenaConexion = "Provider = Microsoft.ACE.OLEDB.12.0; Data Source = " + @"c:\inetpub\excerpta\excerpta.accdb;";

        /*
        ╔═════════════════╗
        ║     MÉTODOS     ║
        ╚═════════════════╝
        */

        public void Crear(string Titulo)
        {
            // Insertamos el registro en la BD
            using (OleDbConnection Conexion = new OleDbConnection(CadenaConexion))
            {
                Conexion.Open();

                OleDbCommand Comando = new OleDbCommand("INSERT INTO Florilegios ([Título]) VALUES (@Titulo)", Conexion);

                Comando.Parameters.AddWithValue("@Titulo", Sanitize(Titulo));

                Comando.ExecuteNonQuery();
            }
        }

        public void Modificar(string TituloActual, string TituloNuevo)
        {
            // Actualizamos el título del florilegio en la tabla de florilegios
            using (OleDbConnection Conexion = new OleDbConnection(CadenaConexion))
            {
                Conexion.Open();

                OleDbCommand Comando = new OleDbCommand("UPDATE Florilegios SET [Título] = @TituloNuevo WHERE [Título] = @TituloActual", Conexion);

                Comando.Parameters.AddWithValue("@TituloNuevo" , Sanitize(TituloNuevo) );
                Comando.Parameters.AddWithValue("@TituloActual", Sanitize(TituloActual));

                Comando.ExecuteNonQuery();
            }

            // Actualizamos el título del florilegio en la tabla de registros
            using (OleDbConnection Conexion = new OleDbConnection(CadenaConexion))
            {
                Conexion.Open();

                OleDbCommand Comando = new OleDbCommand("UPDATE Descripciones SET Florilegio = @TituloNuevo WHERE Florilegio = @TituloActual", Conexion);

                Comando.Parameters.AddWithValue("@TituloNuevo" , Sanitize(TituloNuevo) );
                Comando.Parameters.AddWithValue("@TituloActual", Sanitize(TituloActual));

                Comando.ExecuteNonQuery();
            }

            // Actualizamos el título del florilegio en la tabla de extractos
            using (OleDbConnection Conexion = new OleDbConnection(CadenaConexion))
            {
                Conexion.Open();

                OleDbCommand Comando = new OleDbCommand("UPDATE Extractos SET Florilegio = @TituloNuevo WHERE Florilegio = @TituloActual", Conexion);

                Comando.Parameters.AddWithValue("@TituloNuevo" , Sanitize(TituloNuevo) );
                Comando.Parameters.AddWithValue("@TituloActual", Sanitize(TituloActual));

                Comando.ExecuteNonQuery();
            }
        }

        public void Eliminar(string Titulo)
        {
            // Eliminamos el registro de la BD
            using (OleDbConnection Conexion = new OleDbConnection(CadenaConexion))
            {
                Conexion.Open();

                OleDbCommand Comando = new OleDbCommand("DELETE FROM Florilegios WHERE [Título] = @Titulo", Conexion);

                Comando.Parameters.AddWithValue("@Titulo", Titulo);

                Comando.ExecuteNonQuery();
            }
        }

        public bool ExisteFlorilegio(string Titulo)
        {
            using (OleDbConnection Conexion = new OleDbConnection(CadenaConexion))
            {
                Conexion.Open();

                OleDbCommand Comando = new OleDbCommand("SELECT * FROM Florilegios WHERE [Título] = @Titulo", Conexion);

                Comando.Parameters.AddWithValue("@Titulo", Titulo);

                OleDbDataReader Reader = Comando.ExecuteReader();

                return Reader.Read();
            }
        }

        public bool ContieneRegistros(string Florilegio)
        {
            using (OleDbConnection Conexion = new OleDbConnection(CadenaConexion))
            {
                Conexion.Open();

                OleDbCommand Comando = new OleDbCommand("SELECT * FROM Descripciones WHERE Florilegio = @Florilegio", Conexion);

                Comando.Parameters.AddWithValue("@Florilegio", Florilegio);

                OleDbDataReader Reader = Comando.ExecuteReader();

                return Reader.Read();
            }
        }

        public bool ContieneExtractos(string Florilegio)
        {
            using (OleDbConnection Conexion = new OleDbConnection(CadenaConexion))
            {
                Conexion.Open();

                OleDbCommand Comando = new OleDbCommand("SELECT * FROM Extractos WHERE Florilegio = @Florilegio", Conexion);

                Comando.Parameters.AddWithValue("@Florilegio", Florilegio);

                OleDbDataReader Reader = Comando.ExecuteReader();

                return Reader.Read();
            }
        }

        /*
        ╔══════════════════╗
        ║     AUXILIAR     ║
        ╚══════════════════╝
        */

        private string Sanitize(string Cadena)
        {
            Cadena = Cadena.Trim().Replace("'", "''");

            while (Cadena.IndexOf("  ") > -1) Cadena = Cadena.Replace("  ", " ");

            return Cadena;
        }
    }
}