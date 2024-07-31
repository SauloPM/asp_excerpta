using System.Web;
using System.Data.OleDb;

namespace Excerpta.App_Code
{
    public class Estructura
    {
        // Atributos
        private static readonly string CadenaConexion = "Provider = Microsoft.ACE.OLEDB.12.0; Data Source = " + @"c:\inetpub\excerpta\excerpta.accdb;";

        /*
        ╔═════════════════╗
        ║     MÉTODOS     ║
        ╚═════════════════╝
        */

        public void Crear(string Florilegio, string Autor, string Obra, string Libro, string Poema)
        {
            // Insertamos el registro en la BD
            using (OleDbConnection Conexion = new OleDbConnection(CadenaConexion))
            {
                Conexion.Open();

                OleDbCommand Comando = new OleDbCommand("INSERT INTO Descripciones (Florilegio, Autor, Obra, Libro, Poema) VALUES (@Florilegio, @Autor, @Obra, @Libro, @Poema)", Conexion);

                Comando.Parameters.AddWithValue("@Florilegio", Sanitize(Florilegio));
                Comando.Parameters.AddWithValue("@Autor"     , Sanitize(Autor));
                Comando.Parameters.AddWithValue("@Obra"      , Sanitize(Obra ));
                Comando.Parameters.AddWithValue("@Libro"     , Libro);
                Comando.Parameters.AddWithValue("@Poema"     , Poema);

                Comando.ExecuteNonQuery();
            }
        }

        public void Modificar(string ID, string Florilegio, string Autor, string Obra, string Libro, string Poema)
        {
            // Actualizamos los cambios en la tabla de registros
            using (OleDbConnection Conexion = new OleDbConnection(CadenaConexion))
            {
                Conexion.Open();

                OleDbCommand Comando = new OleDbCommand("UPDATE Descripciones SET Florilegio = @Florilegio, Autor = @Autor, Obra = @Obra, Libro = @Libro, Poema = @Poema WHERE ID = @ID", Conexion);

                Comando.Parameters.AddWithValue("@Florilegio", Sanitize(Florilegio));
                Comando.Parameters.AddWithValue("@Autor"     , Sanitize(Autor));
                Comando.Parameters.AddWithValue("@Obra"      , Sanitize(Obra ));
                Comando.Parameters.AddWithValue("@Libro"     , Libro);
                Comando.Parameters.AddWithValue("@Poema"     , Poema);
                Comando.Parameters.AddWithValue("@ID"        , ID   );

                Comando.ExecuteNonQuery();
            }

            // Actualizamos los cambios en la tabla de extractos
            //using (OleDbConnection Conexion = new OleDbConnection(CadenaConexion))
            //{
            //    Conexion.Open();

            //    OleDbCommand Comando = new OleDbCommand("UPDATE Extractos SET Autor = @AutorNuevo AND Obra = @ObraNueva WHERE Florilegio = @Florilegio AND Autor = @AutorAnterior AND Obra = @ObraAnterior", Conexion);

            //    Comando.Parameters.AddWithValue("@AutorNuevo"   , Sanitize());
            //    Comando.Parameters.AddWithValue("@ObraNueva"    , Sanitize( ));
            //    Comando.Parameters.AddWithValue("@Florilegio"   , Sanitize());
            //    Comando.Parameters.AddWithValue("@AutorAnterior", Sanitize());
            //    Comando.Parameters.AddWithValue("@AutorAnterior", Sanitize());

            //    Comando.ExecuteNonQuery();
            //}
        }

        public void Eliminar(string Titulo)
        {
            // Eliminamos el registro de la BD
            using (OleDbConnection Conexion = new OleDbConnection(CadenaConexion))
            {
                Conexion.Open();

                OleDbCommand Comando = new OleDbCommand("DELETE FROM Descripciones WHERE ID = @ID", Conexion);

                Comando.Parameters.AddWithValue("@ID", Titulo);

                Comando.ExecuteNonQuery();
            }
        }

        public bool ExisteEstructura(string Florilegio, string Autor, string Obra)
        {
            using (OleDbConnection Conexion = new OleDbConnection(CadenaConexion))
            {
                Conexion.Open();

                OleDbCommand Comando = new OleDbCommand("SELECT * FROM Descripciones WHERE Florilegio = @Florilegio AND Autor = @Autor AND Obra = @Obra", Conexion);

                Comando.Parameters.AddWithValue("@Florilegio" , Sanitize(Florilegio));
                Comando.Parameters.AddWithValue("@Autor"      , Sanitize(Autor     ));
                Comando.Parameters.AddWithValue("@Obra"       , Sanitize(Obra      ));

                OleDbDataReader Reader = Comando.ExecuteReader();

                return Reader.Read();
            }
        }

        public bool ContieneExtractos(string Florilegio, string Autor, string Obra)
        {
            using (OleDbConnection Conexion = new OleDbConnection(CadenaConexion))
            {
                Conexion.Open();

                OleDbCommand Comando = new OleDbCommand("SELECT * FROM Extractos WHERE Florilegio = @Florilegio AND Autor = @Autor AND Obra = @Obra", Conexion);

                Comando.Parameters.AddWithValue("@Florilegio" , Sanitize(Florilegio));
                Comando.Parameters.AddWithValue("@Autor"      , Sanitize(Autor     ));
                Comando.Parameters.AddWithValue("@Obra"       , Sanitize(Obra      ));

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