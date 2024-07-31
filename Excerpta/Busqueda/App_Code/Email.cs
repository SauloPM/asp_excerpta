using System.Text;
using System.Net.Mail;

namespace Busqueda.App_Code
{
    public class Email
    {
        // Atributos
        private string Asunto;
        private string Mensaje;
        private string EmailEmisor;
        private string NombreEmisor;
        private string EmailDestino;

        /*
        ╔═════════════════╗
        ║     MÉTODOS     ║
        ╚═════════════════╝
        */

        // Constructor
        public Email(string Asunto, string Mensaje, string NombreEmisor, string EmailEmisor)
        {
            this.Asunto       = Asunto;
            this.Mensaje      = Mensaje;
            this.EmailEmisor  = EmailEmisor;
            this.NombreEmisor = NombreEmisor;
            this.EmailDestino = "gregorio.rodriguez@ulpgc.es";
        }

        // Enviar email
        public void EnviarEmail()
        {            
            MailMessage Correo = new MailMessage
            {
                Priority        = MailPriority.Normal,
                IsBodyHtml      = true,
                SubjectEncoding = Encoding.UTF8,
                BodyEncoding    = Encoding.UTF8
            };

            // Emisor
            Correo.From = new MailAddress("proyectoexcerpta@gmail.com");

            // Receptor
            Correo.To.Add(EmailDestino);

            // Asunto
            Correo.Subject = "Proyecto Excerpta » " + Asunto;

            // Mensaje (HTML)
            Correo.Body = "<html><head>";
            Correo.Body += "<style>";
            Correo.Body += "p  { margin: 0 }";
            Correo.Body += "hr { border-color: rgb(225,225,225); margin: 25px 0 }";
            Correo.Body += "</style>";
            Correo.Body += "</head>";
            Correo.Body += "<body>";
            Correo.Body += "<p>Mensaje enviado por " + NombreEmisor + " (" + EmailEmisor +  ")</p>";
            Correo.Body += "<br>";
            Correo.Body += "<p>" + this.Mensaje + "</p>";
            Correo.Body += "<hr>";
            Correo.Body += "<a href='https://excerpta.iatext.ulpgc.es' target='_blank'>Proyecto Excerpta</a>, perteneciente al <a href='https://www.iatext.ulpgc.es' target='_blank'>Instituto Universitario de An&aacute;lisis y Aplicaciones Textuales</a>";
            Correo.Body += "</body></html>";
            
            SmtpClient ClienteSMTP = new SmtpClient
            {
                // Datos de acceso de la cuenta de correo emisora
                Credentials = new System.Net.NetworkCredential("proyectoexcerpta@gmail.com", "$iatext$"),

                // Requisitos Gmail
                Port      = 587,
                EnableSsl = true,
                Host      = "smtp.gmail.com"
            };

            ClienteSMTP.Send(Correo);
        }
    }
}
