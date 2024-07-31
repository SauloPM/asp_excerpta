using System;
using System.IO;
using System.Web;
using iTextSharp.text;
using System.Data.OleDb;
using iTextSharp.text.pdf;
using System.Globalization;

namespace Excerpta.App_Code
{
	class FicheroPDF
	{
        // Directorios
        string RutaDirectorioPDF      = HttpContext.Current.Server.MapPath("~/PDF");
        string RutaDirectorioImagenes = HttpContext.Current.Server.MapPath("~/Images");

        // Fuentes
        private Font _small        = new Font(Font.FontFamily.HELVETICA,  8, Font.NORMAL, BaseColor.BLACK);
		private Font _small2       = new Font(Font.FontFamily.HELVETICA,  8, Font.BOLD  , BaseColor.BLACK);
        private Font _smallFont    = new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK);
        private Font _largeFont    = new Font(Font.FontFamily.HELVETICA, 18, Font.BOLD  , BaseColor.BLACK);
		private Font _standardFont = new Font(Font.FontFamily.HELVETICA, 14, Font.NORMAL, BaseColor.BLACK);

        // Cadena de conexión con la BD
        private static readonly string CadenaConexion = "Provider = Microsoft.ACE.OLEDB.12.0; Data Source = " + @"c:\inetpub\excerpta\excerpta.accdb;";

        public void Generar(string Usuario, string ConsultaSQL, string Recuento)
        {
            Document Documento = new Document();

            // Creamos el directorio del usuario en caso de no existir
            if (!Directory.Exists(RutaDirectorioPDF + @"/" + Usuario)) Directory.CreateDirectory(RutaDirectorioPDF + @"/" + Usuario);

            PdfWriter Writer = PdfWriter.GetInstance(Documento, new FileStream(RutaDirectorioPDF + @"/" + Usuario + @"/extractos.pdf", FileMode.Create));

            // Cambio de página
            Writer.PageEvent = new ITextEvents();

            // Preparativos
            SetStandardPageSize(Documento);
            Documento.Open();
            Documento.AddTitle("Proyecto Excerpta");
            Documento.AddAuthor("IATEXT » ULPGC");

            // Cabecera
            EscribeCabecera(Documento, ConsultaSQL, Recuento, Usuario);

            // Contenido
            EscribeExtractos(Documento, ConsultaSQL);

            Documento.Close();
        }

		private void EscribeCabecera(Document Documento, string ConsultaSQL, string Recuento, string Usuario)
		{
            // Logo IATEXT
            Image image = Image.GetInstance(RutaDirectorioImagenes + @"\iatext.jpg");

            image.ScaleAbsoluteWidth(150);
            image.ScaleAbsoluteHeight(88);

            image.SetAbsolutePosition(Documento.PageSize.Width - 200f, Documento.PageSize.Height - 125f);
            Documento.Add(image);

            // Cabecera
			AddParagraph(Documento, Element.ALIGN_LEFT, _standardFont, new Chunk("PROYECTO EXCERPTA\n\n"         ));
            AddParagraph(Documento, Element.ALIGN_LEFT, _small       , new Chunk("Dr. Gregorio Rodríguez Herrera"));

            // Enlace web IATEXT
            Font Enlace   = FontFactory.GetFont("Arial", 10, Font.UNDERLINE, new BaseColor(0, 0, 255));
            Anchor anchor = new Anchor("Instituto Universitario de Análisis y Aplicaciones Textuales » IATEXT", Enlace) { Reference = "http://www.iatext.ulpgc.es" };
            Documento.Add(anchor);

            // Número de extractos
            AddParagraph(Documento, Element.ALIGN_LEFT, _small2, new Chunk("\n\n" + Recuento));

            var blackListTextFont = FontFactory.GetFont("HELVETICA", 8, BaseColor.BLACK);
			var redListTextFont   = FontFactory.GetFont("HELVETICA", 8, BaseColor.RED  );
			var campos            = FontFactory.GetFont("HELVETICA", 8, BaseColor.BLUE );

            // Fecha
            AddParagraph(Documento, Element.ALIGN_LEFT, _small, new Chunk("PDF generado el " + DateTime.Now.Day.ToString() + " de " + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Now.Month) + " de " + DateTime.Now.Year.ToString() + " a las " + DateTime.Now.ToShortTimeString() + "\n\n"));

            // Usuario
            AddParagraph(Documento, Element.ALIGN_CENTER, _small, new Chunk(Usuario + "\n"));

            // Separador
            AddParagraph(Documento, Element.ALIGN_CENTER, _small, new Chunk("___________________________________________________________________________________________________________________\n\n"));

            return;
		}

		private void EscribeExtractos(Document Documento, string ConsultaSQL)
		{
            // Colores
            var Negro = FontFactory.GetFont("HELVETICA", 8, BaseColor.BLACK);
            var Azul  = FontFactory.GetFont("HELVETICA", 8, BaseColor.BLUE);

            using (OleDbConnection Conexion = new OleDbConnection(CadenaConexion))
            {
                Conexion.Open();

                OleDbCommand Comando = new OleDbCommand(ConsultaSQL, Conexion);

                OleDbDataReader Reader = Comando.ExecuteReader();

                // Recorremos todos los registros
                while (Reader.Read())
                {
                    // Formato
                    string Florilegio   = EliminarEspaciosInnecesarios(Reader["Florilegio"  ].ToString());
                    string Autor        = EliminarEspaciosInnecesarios(Reader["Autor"       ].ToString());
                    string Obra         = EliminarEspaciosInnecesarios(Reader["Obra"        ].ToString());
                    string Libro        = EliminarEspaciosInnecesarios(Reader["Libro"       ].ToString());
                    string Poema        = EliminarEspaciosInnecesarios(Reader["Poema"       ].ToString());
                    string VersoInicial = EliminarEspaciosInnecesarios(Reader["VersoInicial"].ToString());
                    string VersoFinal   = EliminarEspaciosInnecesarios(Reader["VersoFinal"  ].ToString());
                    string Capitulo     = EliminarEspaciosInnecesarios(Reader["Capítulo"    ].ToString());
                    string Subcapitulo  = EliminarEspaciosInnecesarios(Reader["Subcapítulo" ].ToString());
                    string Extracto     = EliminarEspaciosInnecesarios(Reader["Extracto"    ].ToString()).Replace("/", "\n");
                    string TLL          = EliminarEspaciosInnecesarios(Reader["TLL"         ].ToString()).Replace("/", "\n");
                    string Vernacula    = EliminarEspaciosInnecesarios(Reader["Vernácula"   ].ToString()).Replace("/", "\n");
                    string Pagina       = EliminarEspaciosInnecesarios(Reader["Página"      ].ToString());

                    // Separador
                    AddParagraph(Documento, Element.ALIGN_CENTER, _small, new Chunk("\n"));

                    // Escribimos los campos en el PDF
                    if (Florilegio   != "") AddParagraph(Documento, Element.ALIGN_LEFT, _small, new Phrase(new Chunk("Florilegio: "   , Azul)) { new Chunk(Florilegio  , Negro) });
                    if (Autor        != "") AddParagraph(Documento, Element.ALIGN_LEFT, _small, new Phrase(new Chunk("Autor: "        , Azul)) { new Chunk(Autor       , Negro) });
                    if (Obra         != "") AddParagraph(Documento, Element.ALIGN_LEFT, _small, new Phrase(new Chunk("Obra: "         , Azul)) { new Chunk(Obra        , Negro) });
                    if (Libro        != "") AddParagraph(Documento, Element.ALIGN_LEFT, _small, new Phrase(new Chunk("Libro: "        , Azul)) { new Chunk(Libro       , Negro) });
                    if (Poema        != "") AddParagraph(Documento, Element.ALIGN_LEFT, _small, new Phrase(new Chunk("Poema: "        , Azul)) { new Chunk(Poema       , Negro) });
                    if (Capitulo     != "") AddParagraph(Documento, Element.ALIGN_LEFT, _small, new Phrase(new Chunk("Capítulo: "     , Azul)) { new Chunk(Capitulo    , Negro) });
                    if (Subcapitulo  != "") AddParagraph(Documento, Element.ALIGN_LEFT, _small, new Phrase(new Chunk("Subcapítulo: "  , Azul)) { new Chunk(Subcapitulo , Negro) });
                    if (VersoInicial != "") AddParagraph(Documento, Element.ALIGN_LEFT, _small, new Phrase(new Chunk("Verso inicial: ", Azul)) { new Chunk(VersoInicial, Negro) });
                    if (VersoFinal   != "") AddParagraph(Documento, Element.ALIGN_LEFT, _small, new Phrase(new Chunk("Verso final: "  , Azul)) { new Chunk(VersoFinal  , Negro) });
                    if (Extracto     != "") AddParagraph(Documento, Element.ALIGN_LEFT, _small, new Phrase(new Chunk("Extracto: "     , Azul)) { new Chunk(Extracto    , Negro) });
                    if (TLL          != "") AddParagraph(Documento, Element.ALIGN_LEFT, _small, new Phrase(new Chunk("TLL: "          , Azul)) { new Chunk(TLL         , Negro) });
                    if (Vernacula    != "") AddParagraph(Documento, Element.ALIGN_LEFT, _small, new Phrase(new Chunk("Vernácula: "    , Azul)) { new Chunk(Vernacula   , Negro) });
                    if (Pagina       != "") AddParagraph(Documento, Element.ALIGN_LEFT, _small, new Phrase(new Chunk("Página: "       , Azul)) { new Chunk(Pagina      , Negro) });
                }

                Reader.Close();
            }
		}

        // Procedimiento que establece el margen y el tamaño de página del PDF
        private void SetStandardPageSize(Document doc)
        {
            doc.SetMargins(50, 50, 50, 50);
            doc.SetPageSize(new iTextSharp.text.Rectangle(PageSize.LETTER.Width, PageSize.LETTER.Height));
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
            // Quitamos los espacios innecesarios del principio y del final
            Frase = Frase.Trim();

            // Quitamos los espacios innecesarios intermedios
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
			catch (System.IO.IOException ioe)
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