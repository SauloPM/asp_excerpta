using System;
using System.Web;
using System.Linq;
using System.Data.OleDb;
using System.Text.RegularExpressions;

namespace Excerpta.App_Code
{
    public class Extracto
    {
        // Atributos
        private static readonly string CadenaConexion = "Provider = Microsoft.ACE.OLEDB.12.0; Data Source = " + @"c:\inetpub\excerpta\excerpta.accdb;";

        /*
        ╔═════════════════╗
        ║     MÉTODOS     ║
        ╚═════════════════╝
        */

        public void Crear(string Florilegio, string Autor, string Obra, string Libro, string Poema, string Capitulo, string Subcapitulo, string VersoInicial, string VersoFinal, string Extracto, string TLL, string Vernacula, string Pagina, string Usuario)
        {
            // Insertamos el registro en la BD
            using (OleDbConnection Conexion = new OleDbConnection(CadenaConexion))
            {
                Conexion.Open();

                OleDbCommand Comando = new OleDbCommand("INSERT INTO Extractos (Florilegio, Autor, Obra, Libro, Poema, [Capítulo], [Subcapítulo], VersoInicial, VersoFinal, Extracto, TLL, [Vernácula], [Página], Usuario) VALUES (@Florilegio, @Autor, @Obra, @Libro, @Poema, @Capitulo, @Subcapitulo, @VersoInicial, @VersoFinal, @Extracto, @TLL, @Vernacula, @Pagina, @Usuario)", Conexion);

                Comando.Parameters.AddWithValue("@Florilegio"  , Florilegio  );
                Comando.Parameters.AddWithValue("@Autor"       , Autor       );
                Comando.Parameters.AddWithValue("@Obra"        , Obra        );
                Comando.Parameters.AddWithValue("@Libro"       , Libro       );
                Comando.Parameters.AddWithValue("@Poema"       , Poema       );
                Comando.Parameters.AddWithValue("@Capitulo"    , Capitulo    );
                Comando.Parameters.AddWithValue("@Subcapitulo" , Subcapitulo );
                Comando.Parameters.AddWithValue("@VersoInicial", VersoInicial);
                Comando.Parameters.AddWithValue("@VersoFinal"  , VersoFinal  );
                Comando.Parameters.AddWithValue("@Extracto"    , Extracto    );
                Comando.Parameters.AddWithValue("@TLL"         , TLL         );
                Comando.Parameters.AddWithValue("@Vernacula"   , Vernacula   );
                Comando.Parameters.AddWithValue("@Pagina"      , Pagina      );
                Comando.Parameters.AddWithValue("@Usuario"     , Usuario     );

                Comando.ExecuteNonQuery();
            }

            string ID = ObtenerUltimoID(Usuario);

            if (ID != "") InsertarPalabras(ID, Capitulo, Subcapitulo, Extracto, TLL, Vernacula);
        }

        public void Modificar(string ID, string Florilegio, string Autor, string Obra, string Libro, string Poema, string Capitulo, string Subcapitulo, string VersoInicial, string VersoFinal, string Extracto, string TLL, string Vernacula, string Pagina)
        {
            // Actualizamos el registro en la BD
            using (OleDbConnection Conexion = new OleDbConnection(CadenaConexion))
            {
                Conexion.Open();

                OleDbCommand Comando = new OleDbCommand("UPDATE Extractos SET Florilegio = @Florilegio, Autor = @Autor, Obra = @Obra, Libro = @Libro, Poema = @Poema, [Capítulo] = @Capitulo, [Subcapítulo] = @Subcapitulo, VersoInicial = @VersoInicial, VersoFinal = @VersoFinal, Extracto = @Extracto, TLL = @TLL, [Vernácula] = @Vernacula, [Página] = @Pagina WHERE ID = @ID", Conexion);

                Comando.Parameters.AddWithValue("@Florilegio"  , Florilegio  );
                Comando.Parameters.AddWithValue("@Autor"       , Autor       );
                Comando.Parameters.AddWithValue("@Obra"        , Obra        );
                Comando.Parameters.AddWithValue("@Libro"       , Libro       );
                Comando.Parameters.AddWithValue("@Poema"       , Poema       );
                Comando.Parameters.AddWithValue("@Capitulo"    , Capitulo   .Replace("'", "''"));
                Comando.Parameters.AddWithValue("@Subcapitulo" , Subcapitulo.Replace("'", "''"));
                Comando.Parameters.AddWithValue("@VersoInicial", VersoInicial);
                Comando.Parameters.AddWithValue("@VersoFinal"  , VersoFinal  );
                Comando.Parameters.AddWithValue("@Extracto"    , Extracto .Replace("'", "''"));
                Comando.Parameters.AddWithValue("@TLL"         , TLL      .Replace("'", "''"));
                Comando.Parameters.AddWithValue("@Vernacula"   , Vernacula.Replace("'", "''"));
                Comando.Parameters.AddWithValue("@Pagina"      , Pagina      );
                Comando.Parameters.AddWithValue("@ID"          , ID          );

                Comando.ExecuteNonQuery();
            }

            EliminarPalabras(ID);

            InsertarPalabras(ID, Capitulo, Subcapitulo, Extracto, TLL, Vernacula);
        }

        public void Eliminar(string ID)
        {
            EliminarPalabras(ID);

            // Eliminamos el registro de la BD
            using (OleDbConnection Conexion = new OleDbConnection(CadenaConexion))
            {
                Conexion.Open();

                OleDbCommand Comando = new OleDbCommand("DELETE FROM Extractos WHERE ID = @ID", Conexion);

                Comando.Parameters.AddWithValue("@ID", ID);

                Comando.ExecuteNonQuery();
            }
        }

        public void Publicar(string Identificadores)
        {
            string ConsultaSQL = "UPDATE Extractos SET Alta = -1 WHERE ";

            foreach (string ID in Identificadores.Split(','))
            {
                if (ID == "")
                    continue;

                ConsultaSQL += "ID = " + ID + " OR ";
            }

            ConsultaSQL = ConsultaSQL.Substring(0, ConsultaSQL.Length - 4); // Retiramos el último " OR " de la sentencia SQL

            // Modificamos el registro en la BD
            using (OleDbConnection Conexion = new OleDbConnection(CadenaConexion))
            {
                Conexion.Open();

                OleDbCommand Comando = new OleDbCommand(ConsultaSQL, Conexion);

                Comando.ExecuteNonQuery();
            }
        }

        public void Despublicar(string Identificadores)
        {
            string ConsultaSQL = "UPDATE Extractos SET Alta = 0 WHERE ";

            foreach (string ID in Identificadores.Split(','))
            {
                if (ID == "")
                    continue;

                ConsultaSQL += "ID = " + ID + " OR ";
            }

            ConsultaSQL = ConsultaSQL.Substring(0, ConsultaSQL.Length - 4); // Retiramos el último " OR " de la sentencia SQL

            // Modificamos el registro en la BD
            using (OleDbConnection Conexion = new OleDbConnection(CadenaConexion))
            {
                Conexion.Open();

                OleDbCommand Comando = new OleDbCommand(ConsultaSQL, Conexion);

                Comando.ExecuteNonQuery();
            }
        }

        public bool ExisteExtracto(string Florilegio, string Autor, string Obra, string Libro, string Poema, string Capitulo, string Subcapitulo, string VersoInicial, string VersoFinal)
        {
            // Abortamos si el autor es "Incerti autores"
            if (Autor.Trim().ToLower() == "incerti autores")
                return false;

            using (OleDbConnection Conexion = new OleDbConnection(CadenaConexion))
            {
                Conexion.Open();

                OleDbCommand Comando = new OleDbCommand("SELECT * FROM Extractos WHERE Florilegio LIKE @Florilegio AND Autor LIKE @Autor AND Obra LIKE @Obra AND Libro LIKE @Libro AND Poema LIKE @Poema AND [Capítulo] LIKE @Capitulo AND [Subcapítulo] LIKE @Subcapitulo AND VersoInicial LIKE @VersoInicial AND VersoFinal LIKE @VersoFinal", Conexion);

                Comando.Parameters.AddWithValue("@Florilegio"  , Florilegio  );
                Comando.Parameters.AddWithValue("@Autor"       , Autor       );
                Comando.Parameters.AddWithValue("@Obra"        , Obra        );
                Comando.Parameters.AddWithValue("@Libro"       , Libro       );
                Comando.Parameters.AddWithValue("@Poema"       , Poema       );
                Comando.Parameters.AddWithValue("@Capitulo"    , Capitulo    );
                Comando.Parameters.AddWithValue("@Subcapitulo" , Subcapitulo );
                Comando.Parameters.AddWithValue("@VersoInicial", VersoInicial);
                Comando.Parameters.AddWithValue("@VersoFinal"  , VersoFinal  );

                OleDbDataReader Reader = Comando.ExecuteReader();

                return Reader.Read();
            }
        }

        public bool EstaPublicado(string ID)
        {
            using (OleDbConnection Conexion = new OleDbConnection(CadenaConexion))
            {
                Conexion.Open();

                OleDbCommand Comando = new OleDbCommand("SELECT * FROM Extractos WHERE ID = @ID AND Alta = -1", Conexion);

                Comando.Parameters.AddWithValue("@ID", ID);

                OleDbDataReader Reader = Comando.ExecuteReader();

                return Reader.Read();
            }
        }

        /*
        ╔══════════════════╗
        ║     AUXILIAR     ║
        ╚══════════════════╝
        */

        private string ObtenerUltimoID(string Usuario)
        {
            using (OleDbConnection Conexion = new OleDbConnection(CadenaConexion))
            {
                Conexion.Open();

                OleDbCommand Comando = new OleDbCommand("SELECT TOP 1 ID FROM Extractos WHERE Usuario = @Usuario ORDER BY ID DESC", Conexion);

                Comando.Parameters.AddWithValue("@Usuario", Usuario);

                OleDbDataReader Reader = Comando.ExecuteReader();

                return Reader.Read() ? Reader["ID"].ToString() : "";
            }
        }

        private void InsertarPalabras(string ID, string Capitulo, string Subcapitulo, string Extracto, string TLL, string Vernacula)
        {
            string[] ListaPalabras;

            string[] Cadenas    = { Capitulo, Subcapitulo, Extracto, TLL, Vernacula };
            string[] Categorias = { "Capítulo", "Subcapítulo", "Extracto", "TLL", "Vernácula" };

            for (int i = 0; i < 5; i++)
            {
                if (Cadenas[i] != "")
                {
                    ListaPalabras = SepararPalabras(Cadenas[i]);

                    for (int j = 0; j < ListaPalabras.Length; j++)
                    {
                        try
                        {
                            using (OleDbConnection Conexion = new OleDbConnection(CadenaConexion))
                            {
                                Conexion.Open();

                                OleDbCommand Comando = new OleDbCommand("INSERT INTO Palabras VALUES (@Palabra, @ExtractoID, @Grupo)", Conexion);

                                Comando.Parameters.AddWithValue("@Palabra"   , ListaPalabras[j]);
                                Comando.Parameters.AddWithValue("@ExtractoID", ID              );
                                Comando.Parameters.AddWithValue("@Grupo"     , Categorias[i]   );

                                Comando.ExecuteNonQuery();
                            }
                        }
                        catch (Exception)
                        {
                            string pepe = "";
                            // Palabra repetida
                        }
                    }
                }
            }
        }

        private void EliminarPalabras(string ID)
        {
            // Eliminamos los registros de la BD
            using (OleDbConnection Conexion = new OleDbConnection(CadenaConexion))
            {
                Conexion.Open();

                OleDbCommand Comando = new OleDbCommand("DELETE FROM Palabras WHERE ExtractoID = @ExtractoID", Conexion);

                Comando.Parameters.AddWithValue("@ExtractoID", ID);

                Comando.ExecuteNonQuery();
            }
        }

        private string[] SepararPalabras(string frase)
        {
            // Consideramos separador todos los caracteres que NO pertenezcan al rango a-z (el operador ^ es la negación). Con la opción "IgnoreCase", no consideraremos las letras mayúsculas como separadores al entrar dentro del conjunto a-z. Si no usáramos + dentro de la expresión regular, ante "hola###adiós", obtendíamos el conjunto de palabras "hola, "", "", "adiós". ¿Por qué? Porque sin el + estamos definiendo separadores de un solo carácter, y si hay varios separadores concatenados, todos ellos deben considerarse como uno solo. Para evitar la aparición de palabras de longiutd 0, hay que usarlo.
            string[] palabras = Regex.Split(frase, "[^a-z]+", RegexOptions.IgnoreCase);

            // Formato (todas en minúscula, eliminación de palabras duplicados y nulas)
            return palabras.Select(s => s.ToLowerInvariant()).Distinct().Where(val => val != "").ToArray();
        }
    }
}