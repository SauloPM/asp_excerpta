using System;
using System.IO;
using System.Web;

namespace Busqueda.App_Code
{
    public class Log
    {
        // Atributos
        string Mensaje;
        string Excepcion;
        string Fecha;

        /*
        ╔═════════════════╗
        ║     MÉTODOS     ║
        ╚═════════════════╝
        */

        // Constructor
        public Log(string Mensaje, string Excepcion = "")
        {
            this.Mensaje   = Mensaje;
            this.Excepcion = Excepcion == "" ? "" : " » " + Excepcion;

            Fecha = DateTime.Today.ToString("dd-MM-yyyy") + " " + DateTime.Now.ToString("HH:mm");
        }

        // Registrar actividad
        public void RegistrarActividad()
        {
            try
            {
                string Ruta = @"~/Log/";

                // Ruta del fichero de texto dentro del directorio del usuario
                string Fichero = HttpContext.Current.Server.MapPath(Ruta + @"/" + DateTime.Today.ToString("dd-MM-yyyy") + ".txt");

                // El fichero no se ha creado previamente, lo creamos
                if (!File.Exists(Fichero))
                {
                    // Escribimos en el fichero de texto
                    using (StreamWriter ficheroLog = File.CreateText(Fichero))
                    {
                        ficheroLog.WriteLine(Fecha + " \t\t " + Mensaje + Excepcion);
                    }
                }

                // El fichero sí se ha creado previamente, lo abrimos
                else
                {
                    // Escribimos en el fichero de texto
                    using (StreamWriter ficheroLog = File.AppendText(Fichero))
                    {
                        ficheroLog.WriteLine(Fecha + " \t\t " + Mensaje + Excepcion);
                    }
                }
            }
            catch
            {

            }
        }
    }
}