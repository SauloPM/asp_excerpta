using System;
using System.IO;
using System.Web;
using System.Xml;
using System.Text;
using System.Data.OleDb;
using System.Web.UI.WebControls;

namespace Excerpta.App_Code
{
    public class Backup
    {
        readonly string RutaDirectorioXML    = HttpContext.Current.Server.MapPath("~/XML"   );
        readonly string RutaDirectorioBackup = HttpContext.Current.Server.MapPath("~/Backup");

        private static readonly string CadenaConexion = "Provider = Microsoft.ACE.OLEDB.12.0; Data Source = " + @"c:\inetpub\excerpta\excerpta.accdb;";
        
        /*
        ╔═════════════════╗
        ║     MÉTODOS     ║
        ╚═════════════════╝
        */

        public void Descargar()
        {
            VaciarDirectorio(RutaDirectorioBackup);

            // Creamos un nuevo subdirectorio para alojar los ficheros XML de los florilegios
            Directory.CreateDirectory(RutaDirectorioBackup + "/florilegios");

            // Creamos un fichero XML por florilegio dentro de este subdirectorio
            using (OleDbConnection Conexion = new OleDbConnection(CadenaConexion))
            {
                Conexion.Open();

                OleDbCommand Comando = new OleDbCommand("SELECT DISTINCT Florilegio FROM Extractos WHERE Florilegio IS NOT NULL ORDER BY Florilegio", Conexion);

                OleDbDataReader Reader = Comando.ExecuteReader();

                while (Reader.Read())
                    CrearFicheroXMLFlorilegio(Reader["Florilegio"].ToString());
            }
        }

        public void Subir(FileUpload CopiaSeguridad)
        {
            XmlDocument FicheroXML = new XmlDocument();

            VaciarDirectorio(RutaDirectorioXML);

            // Guardamos el fichero XML importado
            CopiaSeguridad.SaveAs(RutaDirectorioXML + "/" + CopiaSeguridad.FileName);

            // Cargamos el nodo raíz
            FicheroXML.Load(RutaDirectorioXML + "/" + CopiaSeguridad.FileName);

            XmlNode NodoXMLRaiz = FicheroXML.DocumentElement;

            string Florilegio = Sanitize(NodoXMLRaiz.SelectSingleNode("titulo").InnerText);

            // Recorremos y procesamos cada nodo XML
            foreach (XmlNode NodoXML in NodoXMLRaiz.SelectNodes("campos"))
            {
                string Usuario      = NodoXML["usuario"     ] == null ? ""  : NodoXML["usuario"     ].InnerText;
                string Libro        = NodoXML["libro"       ] == null ? "0" : NodoXML["libro"       ].InnerText;
                string Poema        = NodoXML["poema"       ] == null ? "0" : NodoXML["poema"       ].InnerText;
                string VersoInicial = NodoXML["versoinicial"] == null ? "0" : NodoXML["versoinicial"].InnerText;
                string VersoFinal   = NodoXML["versofinal"  ] == null ? "0" : NodoXML["versofinal"  ].InnerText;
                string Pagina       = NodoXML["pagina"      ] == null ? "0" : NodoXML["pagina"      ].InnerText;
                string Autor        = NodoXML["autor"       ] == null ? ""  : Sanitize(NodoXML["autor"    ].InnerText);
                string Obra         = NodoXML["obra"        ] == null ? ""  : Sanitize(NodoXML["obra"     ].InnerText);
                string Capitulo     = NodoXML["capitulo"    ] == null ? ""  : Sanitize(NodoXML["capitulo" ].InnerText);
                string Subcapitulo  = NodoXML["subcapilo"   ] == null ? ""  : Sanitize(NodoXML["subcapilo"].InnerText);
                string Extracto     = NodoXML["extracto"    ] == null ? ""  : Sanitize(NodoXML["extracto" ].InnerText);
                string TLL          = NodoXML["tll"         ] == null ? ""  : Sanitize(NodoXML["tll"      ].InnerText);
                string Vernacula    = NodoXML["vernacula"   ] == null ? ""  : Sanitize(NodoXML["vernacula"].InnerText);

                if (!new Extracto().ExisteExtracto(Florilegio, Autor, Obra, Libro, Poema, Capitulo, Subcapitulo, VersoInicial, VersoFinal))
                    new Extracto().Crear(Florilegio, Autor, Obra, Libro, Poema, Capitulo, Subcapitulo, VersoInicial, VersoFinal, Extracto, TLL, Vernacula, Pagina, Usuario);
            }
        }

        /*
        ╔══════════════════╗
        ║     AUXILIAR     ║
        ╚══════════════════╝
        */

        private void VaciarDirectorio(string Ruta)
        {
            foreach (FileInfo Fichero in new DirectoryInfo(Ruta).GetFiles())
                Fichero.Delete();

            foreach (DirectoryInfo Directorio in new DirectoryInfo(Ruta).GetDirectories())
                Directorio.Delete(true); 
        }

        private void CrearFicheroXMLFlorilegio(string Florilegio)
        {
            // Creamos el fichero XML del florilegio correspondiente
            using (XmlTextWriter FicheroXML = new XmlTextWriter(RutaDirectorioBackup + "/florilegios/" + Florilegio + ".xml", Encoding.Unicode))
            {
                FicheroXML.WriteStartDocument();
                FicheroXML.Formatting = Formatting.Indented;

                FicheroXML.WriteStartElement("florilegio"); // Nodo principal
                FicheroXML.WriteElementString("titulo", Florilegio); // Título del florilegio

                FicheroXML.Indentation = 2;
                FicheroXML.Flush();
            }

            // Escribimos el resto de nodos XML
            using (OleDbConnection Conexion = new OleDbConnection(CadenaConexion))
            {
                Conexion.Open();

                OleDbCommand Comando = new OleDbCommand("SELECT Autor, Obra, Libro, Poema, [Capítulo], [Subcapítulo], VersoInicial, VersoFinal, Extracto, TLL, [Vernácula], [Página], Usuario FROM Extractos WHERE Florilegio = @Florilegio ORDER BY Florilegio, Autor, Obra, Libro, Poema, [Capítulo], [Subcapítulo]", Conexion);

                Comando.Parameters.AddWithValue("@Florilegio", Florilegio);

                OleDbDataReader Reader = Comando.ExecuteReader();

                XmlDocument FicheroXML = new XmlDocument();

                FicheroXML.Load(RutaDirectorioBackup + "/florilegios/" + Florilegio + ".xml");

                XmlNode NodoXMLRaiz = FicheroXML.DocumentElement;

                while (Reader.Read())
                {
                    // Creamos un nodo XML
                    XmlNode NodoXML = CrearNodoXML(FicheroXML, Reader["Autor"].ToString(), Reader["Obra"].ToString(), Reader["Libro"].ToString(), Reader["Poema"].ToString(), Reader["Capítulo"].ToString(), Reader["Subcapítulo"].ToString(), Reader["VersoInicial"].ToString(), Reader["VersoFinal"].ToString(), Reader["Extracto"].ToString(), Reader["TLL"].ToString(), Reader["Vernácula"].ToString(), Reader["Página"].ToString(), Reader["Usuario"].ToString());

                    // Insertamos el nodo al final del fichero XML
                    NodoXMLRaiz.InsertAfter(NodoXML, NodoXMLRaiz.LastChild);

                    // Guardamos los cambios
                    FicheroXML.Save(RutaDirectorioBackup + "/florilegios/" + Florilegio + ".xml");
                }
            }
        }

        private XmlNode CrearNodoXML(XmlDocument ficheroXML, string Autor, string Obra, string Libro, string Poema, string Capitulo, string Subcapitulo, string VersoInicial, string VersoFinal, string Extracto, string TLL, string Vernacula, string Pagina, string Usuario)
        {
            // Creamos el nodo XML <campos>
            XmlElement NodoXML = ficheroXML.CreateElement("campos");

            // Creamos el nodo XML <autor>
            XmlElement nodoAutor = ficheroXML.CreateElement("autor");
            nodoAutor.InnerText = Autor;
            NodoXML.AppendChild(nodoAutor);

            // Creamos el nodo XML <obra>
            XmlElement nodoObra = ficheroXML.CreateElement("obra");
            nodoObra.InnerText = Obra;
            NodoXML.AppendChild(nodoObra);

            // Creamos el nodo XML <libro>
            XmlElement nodoLibro = ficheroXML.CreateElement("libro");
            nodoLibro.InnerText = Libro;
            NodoXML.AppendChild(nodoLibro);

            // Creamos el nodo XML <poema>
            XmlElement nodoPoema = ficheroXML.CreateElement("poema");
            nodoPoema.InnerText = Poema;
            NodoXML.AppendChild(nodoPoema);

            // Creamos el nodo XML <capitulo>
            if (Capitulo.Trim() != "")
            {
                XmlElement nodoCapitulo = ficheroXML.CreateElement("capitulo");
                nodoCapitulo.InnerText = Capitulo;
                NodoXML.AppendChild(nodoCapitulo);
            }

            // Creamos el nodo XML <subcapitulo>
            if (Subcapitulo.Trim() != "")
            {
                XmlElement nodoSubcapitulo = ficheroXML.CreateElement("subcapitulo");
                nodoSubcapitulo.InnerText = Subcapitulo;
                NodoXML.AppendChild(nodoSubcapitulo);
            }

            // Creamos el nodo XML <versoinicial>
            XmlElement nodoVersoInicial = ficheroXML.CreateElement("versoinicial");
            nodoVersoInicial.InnerText = VersoInicial;
            NodoXML.AppendChild(nodoVersoInicial);

            // Creamos el nodo XML <versofinal>
            XmlElement nodoVersoFinal = ficheroXML.CreateElement("versofinal");
            nodoVersoFinal.InnerText = VersoFinal;
            NodoXML.AppendChild(nodoVersoFinal);

            // Creamos el nodo XML <pagina>
            XmlElement nodoPagina = ficheroXML.CreateElement("pagina");
            nodoPagina.InnerText = Pagina;
            NodoXML.AppendChild(nodoPagina);

            // Creamos el nodo XML <extracto>
            XmlElement nodoExtracto = ficheroXML.CreateElement("extracto");
            nodoExtracto.InnerText = Extracto;
            NodoXML.AppendChild(nodoExtracto);

            // Creamos el nodo XML <tll>
            XmlElement nodoTLL = ficheroXML.CreateElement("tll");
            nodoTLL.InnerText = TLL;
            NodoXML.AppendChild(nodoTLL);

            // Creamos el nodo XML <vernacula>
            if (Vernacula.Trim() != "")
            {
                XmlElement nodoVernacula = ficheroXML.CreateElement("vernacula");
                nodoVernacula.InnerText = Vernacula;
                NodoXML.AppendChild(nodoVernacula);
            }

            // Creamos el nodo XML <usuario>
            if (Usuario.Trim() != "")
            {
                XmlElement nodoUsuario = ficheroXML.CreateElement("usuario");
                nodoUsuario.InnerText = Usuario;
                NodoXML.AppendChild(nodoUsuario);
            }

            return NodoXML;
        }

        private string Sanitize(string Cadena)
        {
            Cadena = Cadena.Trim().Replace("'", "''").Replace(Environment.NewLine, "/");

            while (Cadena.IndexOf("  ") > -1)
                Cadena = Cadena.Replace("  ", " ");

            return Cadena;
        }
    }
}