using System.Web;
using System.Data.OleDb;

namespace Excerpta.App_Code
{
    public class FuenteBibliografica
    {
        // Atributos
        private static readonly string CadenaConexion = "Provider = Microsoft.ACE.OLEDB.12.0; Data Source = " + @"c:\inetpub\excerpta\excerpta.accdb;";

        /*
        ╔═════════════════╗
        ║     MÉTODOS     ║
        ╚═════════════════╝
        */

        public void Crear(string Bibliografia)
        {
            // Insertamos el registro en la BD
            using (OleDbConnection Conexion = new OleDbConnection(CadenaConexion))
            {
                Conexion.Open();

                OleDbCommand Comando = new OleDbCommand("INSERT INTO Biblioteca ([Bibliografía]) VALUES (@Bibliografia)", Conexion);

                Comando.Parameters.AddWithValue("@Bibliografia", Bibliografia);

                Comando.ExecuteNonQuery();
            }
        }

        public void Modificar(string ID, string Bibliografia)
        {
            // Actualizamos el registro de la BD
            using (OleDbConnection Conexion = new OleDbConnection(CadenaConexion))
            {
                Conexion.Open();

                OleDbCommand Comando = new OleDbCommand("UPDATE Biblioteca SET [Bibliografía] = @Bibliografia WHERE ID = @ID", Conexion);

                Comando.Parameters.AddWithValue("@Bibliografia", EliminarEspaciosInnecesarios(Bibliografia));
                Comando.Parameters.AddWithValue("@ID", ID);

                Comando.ExecuteNonQuery();
            }
        }

        public void Eliminar(string ID)
        {
            // Eliminamos el registro de la BD
            using (OleDbConnection Conexion = new OleDbConnection(CadenaConexion))
            {
                Conexion.Open();

                OleDbCommand Comando = new OleDbCommand("DELETE FROM Biblioteca WHERE ID = @ID", Conexion);

                Comando.Parameters.AddWithValue("@ID", ID);

                Comando.ExecuteNonQuery();
            }
        }

        /*
        ╔══════════════════╗
        ║     AUXILIAR     ║
        ╚══════════════════╝
        */

        private string EliminarEspaciosInnecesarios(string Frase)
        {
            // Quitamos los espacios innecesarios del principio y del final
            Frase = Frase.Trim();

            // Quitamos los espacios innecesarios intermedios
            while (Frase.IndexOf("  ") > -1) Frase = Frase.Replace("  ", " ");

            return Frase;
        }
    }
}