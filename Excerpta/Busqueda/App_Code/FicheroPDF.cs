using System;
using System.IO;
using System.Web;
using iTextSharp.text;
using System.Data.OleDb;
using iTextSharp.text.pdf;

namespace Busqueda.App_Code
{
	class FicheroPDF
	{
        // Directorios
        readonly string RutaDirectorioDescargas = HttpContext.Current.Server.MapPath("~/Descargas");
        readonly string RutaDirectorioImagenes  = HttpContext.Current.Server.MapPath("~/Images"   );

        // Cadena de conexión
        //readonly string CadenaConexion = "Provider = Microsoft.Jet.OLEDB.4.0; Data Source = " + HttpContext.Current.Server.MapPath("~/App_Data") + @"\excerpta.mdb";
        private static readonly String CadenaConexion = "Provider = Microsoft.ACE.OLEDB.12.0; Data Source = " + @"c:\inetpub\excerpta\excerpta.accdb;";

        // Fuentes
        private Font _small        = new Font(Font.FontFamily.HELVETICA,  8, Font.NORMAL, BaseColor.BLACK);
		private Font _small2       = new Font(Font.FontFamily.HELVETICA,  8, Font.BOLD  , BaseColor.BLACK);
        private Font _smallFont    = new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK);
        private Font _largeFont    = new Font(Font.FontFamily.HELVETICA, 18, Font.BOLD  , BaseColor.BLACK);
		private Font _standardFont = new Font(Font.FontFamily.HELVETICA, 14, Font.NORMAL, BaseColor.BLACK);

        // Atributos
        string Titulo;
        string Florilegio;
        string Recuento;

        // Constructor
        public FicheroPDF(string Titulo, string Florilegio, string Recuento = "1")
        {
            this.Titulo     = RutaDirectorioDescargas + "/" + Titulo + ".pdf";
            this.Florilegio = Florilegio;
            this.Recuento   = Recuento;
        }

        public void Generar(string ConsultaSQL)
		{
            Document Documento = null;

            try
			{
                if (File.Exists(Titulo)) File.Delete(Titulo);

                Documento = new Document();
				PdfWriter writer = PdfWriter.GetInstance(Documento, new FileStream(Titulo, FileMode.Create));

				// Cambio de página
				writer.PageEvent = new ITextEvents();

                // Preparativos
				SetStandardPageSize(Documento);
				Documento.Open();
                Documento.AddTitle("Proyecto Excerpta");
                Documento.AddAuthor("IATEXT - ULPGC");

                // Cabecera
                EscribeCabecera(Documento, Florilegio, RutaDirectorioImagenes, Recuento);

                // Contenido
                if (Titulo.IndexOf("Biblioteca") == -1)
                    EscribeExtractos(Documento, ConsultaSQL);
                else
                    EscribeFuentesBibliograficas(Documento, ConsultaSQL);
            }
			catch (Exception Excepcion)
            {
                string Mensaje = Excepcion.Message;
			}
			finally
			{
				Documento.Close();
				Documento = null;
			}
		}

		// Procedimiento que escribe la cabecera de la primera página
		private void EscribeCabecera(Document doc, string TituloFlorilegio, string RutaDirectorioImages, string Recuento)
		{
            // Logo IATEXT
            Image image = Image.GetInstance(RutaDirectorioImages + @"\iatext.jpg");

            image.ScaleAbsoluteWidth(150);
            image.ScaleAbsoluteHeight(88);

            image.SetAbsolutePosition(doc.PageSize.Width - 200f, doc.PageSize.Height - 125f);
            doc.Add(image);

            // Cabecera
			AddParagraph(doc, Element.ALIGN_LEFT, _standardFont, new Chunk("PROYECTO EXCERPTA\n\n"         ));
            AddParagraph(doc, Element.ALIGN_LEFT, _small       , new Chunk("Dr. Gregorio Rodríguez Herrera"));

            // Enlace web IATEXT
            Font link     = FontFactory.GetFont("Arial", 10, Font.UNDERLINE, new BaseColor(0, 0, 255));
            Anchor anchor = new Anchor("Instituto Universitario de Análisis y Aplicaciones Textuales » IATEXT", link) { Reference = "http://www.iatext.ulpgc.es/" };
            doc.Add(anchor);

            // Número de extractos
            AddParagraph(doc, Element.ALIGN_LEFT, _small2, new Chunk("\n\n" + Recuento + " resultado(s)"));

            var blackListTextFont = FontFactory.GetFont("HELVETICA", 8, BaseColor.BLACK);
			var redListTextFont   = FontFactory.GetFont("HELVETICA", 8, BaseColor.RED  );
			var campos            = FontFactory.GetFont("HELVETICA", 8, BaseColor.BLUE );

            // Fecha
            AddParagraph(doc, Element.ALIGN_LEFT, _small, new Chunk("PDF generado el " + DateTime.Now.Day.ToString() + " de " + System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Now.Month) + " de " + DateTime.Now.Year.ToString() + " a las " + DateTime.Now.ToShortTimeString() + "\n\n"));

            // Título del florilegio
            AddParagraph(doc, Element.ALIGN_CENTER, _small, new Chunk(TituloFlorilegio + "\n"));

            // Separador
            AddParagraph(doc, Element.ALIGN_CENTER, _small, new Chunk("___________________________________________________________________________________________________________________\n\n"));

            return;
		}

        // Función que escribe los extractos en el PDF
		private void EscribeExtractos(Document doc, string IDExtracto)
		{
            // Colores
            var ColorNegro = FontFactory.GetFont("HELVETICA", 8, BaseColor.BLACK);
            var ColorAzul  = FontFactory.GetFont("HELVETICA", 8, BaseColor.BLUE);

            try
            {
                using (OleDbConnection Conexion = new OleDbConnection(CadenaConexion))
                {
                    Conexion.Open();

                    OleDbCommand Comando = new OleDbCommand("GetExtracto", Conexion);
                    Comando.CommandType  = System.Data.CommandType.StoredProcedure;

                    Comando.Parameters.AddWithValue("@ID", IDExtracto);

                    OleDbDataReader Reader = Comando.ExecuteReader();

                    // Recorremos todos los registros
                    while (Reader.Read())
                    {
                        // Formato
                        string Autor        = EliminarEspaciosInnecesarios(Reader["Autor"       ].ToString().Trim());
                        string Obra         = EliminarEspaciosInnecesarios(Reader["Obra"        ].ToString().Trim());
                        string Libro        = EliminarEspaciosInnecesarios(Reader["Libro"       ].ToString().Trim());
                        string Poema        = EliminarEspaciosInnecesarios(Reader["Poema"       ].ToString().Trim());
                        string VersoInicial = EliminarEspaciosInnecesarios(Reader["VersoInicial"].ToString().Trim());
                        string VersoFinal   = EliminarEspaciosInnecesarios(Reader["VersoFinal"  ].ToString().Trim());
                        string Capitulo     = EliminarEspaciosInnecesarios(Reader["Capítulo"    ].ToString().Trim());
                        string Subcapitulo  = EliminarEspaciosInnecesarios(Reader["Subcapítulo" ].ToString().Trim());
                        string Extracto     = EliminarEspaciosInnecesarios(Reader["Extracto"    ].ToString().Trim()).Replace("/", "\n");
                        string TLL          = EliminarEspaciosInnecesarios(Reader["TLL"         ].ToString().Trim()).Replace("/", "\n");
                        string Vernacula    = EliminarEspaciosInnecesarios(Reader["Vernácula"   ].ToString().Trim()).Replace("/", "\n");
                        string Pagina       = EliminarEspaciosInnecesarios(Reader["Página"      ].ToString().Trim());

                        // Separador
                        AddParagraph(doc, Element.ALIGN_CENTER, _small, new Chunk("\n"));

                        // Escribimos los campos en el PDF
                        if (Autor        != "" ) AddParagraph(doc, Element.ALIGN_LEFT, _small, new Phrase(new Chunk("Autor: "        , ColorAzul)) { new Chunk(Autor       , ColorNegro) });
                        if (Obra         != "Ø") AddParagraph(doc, Element.ALIGN_LEFT, _small, new Phrase(new Chunk("Obra: "         , ColorAzul)) { new Chunk(Obra        , ColorNegro) });
                        if (Libro        != "0") AddParagraph(doc, Element.ALIGN_LEFT, _small, new Phrase(new Chunk("Libro: "        , ColorAzul)) { new Chunk(Libro       , ColorNegro) });
                        if (Poema        != "0") AddParagraph(doc, Element.ALIGN_LEFT, _small, new Phrase(new Chunk("Poema: "        , ColorAzul)) { new Chunk(Poema       , ColorNegro) });
                        if (Capitulo     != "" ) AddParagraph(doc, Element.ALIGN_LEFT, _small, new Phrase(new Chunk("Capítulo: "     , ColorAzul)) { new Chunk(Capitulo    , ColorNegro) });
                        if (Subcapitulo  != "" ) AddParagraph(doc, Element.ALIGN_LEFT, _small, new Phrase(new Chunk("Subcapítulo: "  , ColorAzul)) { new Chunk(Subcapitulo , ColorNegro) });
                        if (VersoInicial != "0") AddParagraph(doc, Element.ALIGN_LEFT, _small, new Phrase(new Chunk("Verso inicial: ", ColorAzul)) { new Chunk(VersoInicial, ColorNegro) });
                        if (VersoFinal   != "0") AddParagraph(doc, Element.ALIGN_LEFT, _small, new Phrase(new Chunk("Verso final: "  , ColorAzul)) { new Chunk(VersoFinal  , ColorNegro) });
                        if (Extracto     != "" ) AddParagraph(doc, Element.ALIGN_LEFT, _small, new Phrase(new Chunk("Extracto: "     , ColorAzul)) { new Chunk(Extracto    , ColorNegro) });
                        if (TLL          != "" ) AddParagraph(doc, Element.ALIGN_LEFT, _small, new Phrase(new Chunk("TLL: "          , ColorAzul)) { new Chunk(TLL         , ColorNegro) });
                        if (Vernacula    != "" ) AddParagraph(doc, Element.ALIGN_LEFT, _small, new Phrase(new Chunk("Vernácula: "    , ColorAzul)) { new Chunk(Vernacula   , ColorNegro) });
                        if (Pagina       != "" ) AddParagraph(doc, Element.ALIGN_LEFT, _small, new Phrase(new Chunk("Página: "       , ColorAzul)) { new Chunk(Pagina      , ColorNegro) });
                    }

                    Reader.Close();
                }
            }
            catch (Exception Excepcion)
            {
                string Mensaje = Excepcion.Message;
            }
		}

        // Función que escribe las fuentes bibliográficas en el PDF
        private void EscribeFuentesBibliograficas(Document doc, string ConsultaSQL)
        {
            // Colores
            var ColorNegro = FontFactory.GetFont("HELVETICA", 8, BaseColor.BLACK);
            var ColorAzul  = FontFactory.GetFont("HELVETICA", 8, BaseColor.BLUE);

            // Preparativos de la consulta SQL
            OleDbConnection Conexion = new OleDbConnection(CadenaConexion);
            OleDbCommand    Comando  = new OleDbCommand(ConsultaSQL, Conexion);

            try
            {
                // Ejecutamos el comando SQL
                Conexion.Open();
                OleDbDataReader reader = Comando.ExecuteReader();

                // Recorremos todos los registros
                while (reader.Read())
                {
                    // Separador
                    AddParagraph(doc, Element.ALIGN_CENTER, _small, new Chunk("\n"));

                    // Formato
                    string Bibliografia = EliminarEspaciosInnecesarios(reader["Bibliografía"].ToString());

                    // Escribimos el campo en el PDF
                    if (Bibliografia != "") AddParagraph(doc, Element.ALIGN_LEFT, _small, new Phrase(new Chunk(Bibliografia, ColorNegro)));
                }

                reader.Close();
            }
            catch (Exception Excepcion)
            {
                string Mensaje = Excepcion.Message;
            }
            finally
            {
                Conexion.Close();
            }
        }

        // Procedimiento que establece el margen y el tamaño de página del PDF
        private void SetStandardPageSize(Document doc)
		{
			doc.SetMargins(50, 50, 50, 50);
			doc.SetPageSize(new Rectangle(PageSize.LETTER.Width, PageSize.LETTER.Height));
		}

		// Procedimiento que añade un párrafo al PDF
		private void AddParagraph(Document doc, int alignment, Font font, IElement content)
		{
			Paragraph paragraph = new Paragraph();
			paragraph.SetLeading(0f, 1.2f);
			paragraph.Alignment = alignment;
			paragraph.Font = font;
			paragraph.Add(content);
			doc.Add(paragraph);
		}

        // Función que elimina los espacios innecesarios de una ristra
        private string EliminarEspaciosInnecesarios(string Frase)
        {
            // Espacios del principio y final
            Frase = Frase.Trim();

            // Espacios intermedios
            while (Frase.IndexOf("  ") > -1)
                Frase = Frase.Replace("  ", " ");

            return Frase;
        }
    }

	public class ITextEvents : PdfPageEventHelper
	{

		// This is the contentbyte object of the writer
		PdfContentByte cb;

		// we will put the final number of pages in a template
		PdfTemplate headerTemplate, footerTemplate;

		// this is the BaseFont we are going to use for the header / footer
		BaseFont bf = null;

		// This keeps track of the creation time
		DateTime PrintTime = DateTime.Now;

		#region Fields
		private string _header;
		#endregion

		#region Properties
		public string Header
		{
			get { return _header; }
			set { _header = value; }
		}
		#endregion

		public override void OnOpenDocument(PdfWriter writer, Document document)
		{
			try
			{
				PrintTime = DateTime.Now;
				bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
				cb = writer.DirectContent;
				headerTemplate = cb.CreateTemplate(100, 100);
				footerTemplate = cb.CreateTemplate(50, 50);
			}
			catch (DocumentException de)
			{
				//handle exception here
			}
			catch (IOException ioe)
			{
				//handle exception here
			}
		}

        // Procedimiento que escribe el número de página en el pie de cada página
		public override void OnEndPage(PdfWriter writer, Document document)
		{
			base.OnEndPage(writer, document);
            string text = "Página " + writer.PageNumber;
            cb.BeginText();
			cb.SetFontAndSize(bf, 8);
            cb.SetColorFill(BaseColor.GRAY);
			cb.SetTextMatrix(document.PageSize.GetLeft(50), document.PageSize.GetBottom(45));
			cb.ShowText("___________________________________________________________________________________________________________________"); 
			cb.SetTextMatrix(document.PageSize.GetRight(90), document.PageSize.GetBottom(30));
			cb.ShowText(text); 
			cb.EndText();
		}

		public override void OnCloseDocument(PdfWriter writer, Document document)
		{
			base.OnCloseDocument(writer, document);

			footerTemplate.BeginText();
			footerTemplate.SetFontAndSize(bf, 8);
			footerTemplate.SetTextMatrix(0, 0);
			footerTemplate.ShowText((writer.PageNumber - 1).ToString());
			footerTemplate.EndText();
		}
	}
}