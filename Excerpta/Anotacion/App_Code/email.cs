using System.Text;
using System.Net.Mail;

namespace Excerpta.App_Code
{
    public class Email
    {
        // Atributos
        private string EmailDestino;
        private string Asunto;
        private string Mensaje;

        /*
        ╔═════════════════╗
        ║     MÉTODOS     ║
        ╚═════════════════╝
        */

        // Constructor
        public Email(string Asunto, string Mensaje, string EmailDestino = "gregorio.rodriguez@ulpgc.es")
        {
            this.Asunto       = Asunto;
            this.Mensaje      = Mensaje;
            this.EmailDestino = EmailDestino;
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
            Correo.Body += "<p>" + Mensaje + "</p>";
            Correo.Body += "<hr>";
            Correo.Body += "<a href='http://anotaexcerpta.iatext.ulpgc.es' target='_blank'>Proyecto Excerpta</a>, perteneciente al <a href='http://www.iatext.ulpgc.es' target='_blank'>Instituto Universitario de An&aacute;lisis y Aplicaciones Textuales</a>";
            Correo.Body += "</body></html>";


            SmtpClient ClienteSMTP = new SmtpClient
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
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