using System.Web;
using System.Data.OleDb;

namespace Excerpta.App_Code
{
    public class Usuario
    {
        // Atributos
        private static readonly string CadenaConexion = "Provider = Microsoft.ACE.OLEDB.12.0; Data Source = " + @"c:\inetpub\excerpta\excerpta.accdb;";

        /*
        ╔═════════════════╗
        ║     MÉTODOS     ║
        ╚═════════════════╝
        */

        public void Crear(string Nombre, string Apellidos, string Centro, string Telefono, string Email, string Clave)
        {
            // Insertamos el registro en la BD
            using (OleDbConnection Conexion = new OleDbConnection(CadenaConexion))
            {
                Conexion.Open();

                OleDbCommand Comando = new OleDbCommand("INSERT INTO Usuarios (Nombre, Apellidos, Centro, [Teléfono], Email, Clave, Alta) VALUES (@Nombre, @Apellidos, @Centro, @Telefono, @Email, @Clave, 0)", Conexion);

                Comando.Parameters.AddWithValue("@Nombre"   , new cifrado(Nombre   ).cifrar());
                Comando.Parameters.AddWithValue("@Apellidos", new cifrado(Apellidos).cifrar());
                Comando.Parameters.AddWithValue("@Centro"   , new cifrado(Centro   ).cifrar());
                Comando.Parameters.AddWithValue("@Telefono" , new cifrado(Telefono ).cifrar());
                Comando.Parameters.AddWithValue("@Email"    , new cifrado(Email    ).cifrar());
                Comando.Parameters.AddWithValue("@Clave"    , new cifrado(Clave    ).cifrar());

                Comando.ExecuteNonQuery();
            }
        }

        public void Eliminar(string Email)
        {
            if (ContieneExtractos(Email)) TransferirExtractos(Email);

            // Eliminamos el registro de la BD
            using (OleDbConnection Conexion = new OleDbConnection(CadenaConexion))
            {
                Conexion.Open();

                OleDbCommand Comando = new OleDbCommand("DELETE FROM Usuarios WHERE Email = @Email", Conexion);

                Comando.Parameters.AddWithValue("@Email", new cifrado(Email).cifrar());

                Comando.ExecuteNonQuery();
            }
        }

        public void CambiarClave(string Email, string Clave)
        {
            // Actualizamos el registro en la BD
            using (OleDbConnection Conexion = new OleDbConnection(CadenaConexion))
            {
                Conexion.Open();

                OleDbCommand Comando = new OleDbCommand("UPDATE Usuarios SET Clave = @Clave WHERE Email = @Email", Conexion);

                Comando.Parameters.AddWithValue("@Clave", new cifrado(Clave).cifrar());
                Comando.Parameters.AddWithValue("@Email", new cifrado(Email).cifrar());

                Comando.ExecuteNonQuery();
            }
        }

        public void DarDeAlta(string Emails)
        {
            foreach (string Email in Emails.Split(','))
            {
                if (Email == "") continue;

                // Modificamos el registro en la BD
                using (OleDbConnection Conexion = new OleDbConnection(CadenaConexion))
                {
                    Conexion.Open();

                    OleDbCommand Comando = new OleDbCommand("UPDATE Usuarios SET Alta = -1 WHERE Email = @Email", Conexion);

                    Comando.Parameters.AddWithValue("@Email", new cifrado(Email).cifrar());

                    Comando.ExecuteNonQuery();
                }

                // Notificamos al usuario
                new Email("Ha sido dado de alta", "Le comunicamos que ha sido dado de alta satisfactoriamente en el Proyecto Excerpta", Email).EnviarEmail();

                new Log("gregorio.rodriguez@ulpgc.es", "HA DADO DE ALTA A '" + Email + "'").RegistrarActividad();
            }
        }

        public void DarDeBaja(string Emails)
        {
            foreach (string Email in Emails.Split(','))
            {
                if (Email == "" || Email == "gregorio.rodriguez@ulpgc.es")
                    continue;

                new Usuario().Eliminar(Email);

                // Notificamos al usuario
                new Email("Ha sido dado de baja", "Le comunicamos que ha sido dado de baja del Proyecto Excerpta.", Email).EnviarEmail();

                new Log("gregorio.rodriguez@ulpgc.es", "HA DADO DE BAJA A '" + Email + "'").RegistrarActividad();
            }
        }

        public bool ExisteUsuario(string Email)
        {
            using (OleDbConnection Conexion = new OleDbConnection(CadenaConexion))
            {
                Conexion.Open();

                OleDbCommand Comando = new OleDbCommand("SELECT * FROM Usuarios WHERE Email = @Email", Conexion);

                Comando.Parameters.AddWithValue("@Email", new cifrado(Email).cifrar());

                OleDbDataReader Reader = Comando.ExecuteReader();

                return Reader.Read();
            }
        }

        public bool ComprobarDatos(string Email, string Clave)
        {
            using (OleDbConnection Conexion = new OleDbConnection(CadenaConexion))
            {
                Conexion.Open();

                OleDbCommand Comando = new OleDbCommand("SELECT * FROM Usuarios WHERE Email = @Email AND Clave = @Clave AND Alta = -1", Conexion);

                Comando.Parameters.AddWithValue("@Email", new cifrado(Email).cifrar());
                Comando.Parameters.AddWithValue("@Clave", new cifrado(Clave).cifrar());

                OleDbDataReader Reader = Comando.ExecuteReader();

                return Reader.Read();
            }
        }

        public string ObtenerClave(string Email)
        {
            string Clave = "";

            using (OleDbConnection Conexion = new OleDbConnection(CadenaConexion))
            {
                Conexion.Open();

                OleDbCommand Comando = new OleDbCommand("SELECT Clave FROM Usuarios WHERE Email = @Email", Conexion);

                Comando.Parameters.AddWithValue("@Email", new cifrado(Email).cifrar());

                OleDbDataReader Reader = Comando.ExecuteReader();

                Clave = Reader.Read() ? new cifrado(Reader["Clave"].ToString()).descifrar() : "";
            }

            return Clave;
        }

        /*
        ╔══════════════════╗
        ║     AUXILIAR     ║
        ╚══════════════════╝
        */

        private bool ContieneExtractos(string Email)
        {
            using (OleDbConnection Conexion = new OleDbConnection(CadenaConexion))
            {
                Conexion.Open();

                OleDbCommand Comando = new OleDbCommand("SELECT * FROM Extractos WHERE Usuario = @Email", Conexion);

                Comando.Parameters.AddWithValue("@Email", Email);

                OleDbDataReader Reader = Comando.ExecuteReader();

                return Reader.Read();
            }
        }

        private void TransferirExtractos(string Email)
        {
            // Eliminamos el registro de la BD
            using (OleDbConnection Conexion = new OleDbConnection(CadenaConexion))
            {
                Conexion.Open();

                OleDbCommand Comando = new OleDbCommand("UPDATE Extractos SET Usuario = 'gregorio.rodriguez@ulpgc.es' WHERE Usuario LIKE @Email", Conexion);

                Comando.Parameters.AddWithValue("@Email", Email);

                Comando.ExecuteNonQuery();
            }
        }
    }
}