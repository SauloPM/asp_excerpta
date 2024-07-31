using System;
using System.IO;
using System.Web;

namespace Excerpta.App_Code
{
    public class Log
    {
        // Atributos
        string Usuario;
        string Mensaje;
        string Excepcion;
        string Fecha;
        string IP;

        /*
        ╔═════════════════╗
        ║     MÉTODOS     ║
        ╚═════════════════╝
        */

        // Constructor
        public Log(string Usuario, string Mensaje, string Excepcion = "")
        {
            this.Usuario   = Usuario;
            this.Mensaje   = Mensaje;
            this.Excepcion = Excepcion == "" ? "" : " » " + Excepcion;

            Fecha = DateTime.Today.ToString("dd-MM-yyyy") + " " + DateTime.Now.ToString("HH:mm");
            IP    = HttpContext.Current != null ? HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString() : "";
        }

        // Registrar actividad
        public void RegistrarActividad()
        {
            try
            {
                string Ruta = @"~/Log/" + Usuario;

                // Creamos el directorio del usuario
                if (!Directory.Exists(HttpContext.Current.Server.MapPath(Ruta)))
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(Ruta));

                // Ruta del fichero de texto dentro del directorio del usuario
                string Fichero = HttpContext.Current.Server.MapPath(Ruta + @"/" + DateTime.Today.ToString("dd-MM-yyyy") + ".txt");

                // El fichero no se ha creado previamente, lo creamos
                if (!File.Exists(Fichero))
                {
                    // Escribimos en el fichero de texto.
                    using (StreamWriter ficheroLog = File.CreateText(Fichero))
                    {
                        ficheroLog.WriteLine(Fecha + " \t\t " + IP + " \t\t " + Mensaje + Excepcion);
                    }
                }

                // El fichero sí se ha creado previamente, lo abrimos
                else
                {
                    // Escribimos en el fichero de texto.
                    using (StreamWriter ficheroLog = File.AppendText(Fichero))
                    {
                        ficheroLog.WriteLine(Fecha + " \t\t " + IP + " \t\t " + Mensaje + Excepcion);
                    }
                }
            }
            catch
            {

            }
        }
    }
}