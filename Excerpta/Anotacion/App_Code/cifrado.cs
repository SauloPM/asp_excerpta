using System;
using System.Text;
using System.Security.Cryptography;

namespace Excerpta.App_Code
{
    public class cifrado
    {
        // ================= //
        //     ATRIBUTOS     //
        // ================= //

        private string palabra; // Palabra que se quiere cifrar/descifrar.
        private string clave;   // Clave de cifrado/descifrado.

        // =============== //
        //     MÉTODOS     //
        // =============== //

        // Constructor.
        public cifrado(string palabra, string clave = "Excerpta_Key")
        {
            this.palabra = palabra;
            this.clave = clave;
        }

        // Función que cifra una cadena.
        public string cifrar()
        {
            byte[] data = Encoding.UTF8.GetBytes(palabra);
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(Encoding.UTF8.GetBytes(clave));
                using (TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform transform = tripDes.CreateEncryptor();
                    byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                    return Convert.ToBase64String(results, 0, results.Length);
                }
            }
            /*
            byte[] llave; //Arreglo donde guardaremos la llave para el cifrado 3DES.
            byte[] arreglo = Encoding.UTF8.GetBytes(palabra); //Arreglo donde guardaremos la cadena descifrada.

            // Ciframos utilizando el Algoritmo MD5.
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            llave = md5.ComputeHash(Encoding.UTF8.GetBytes(clave));
            md5.Clear();

            //Ciframos utilizando el Algoritmo 3DES.
            TripleDESCryptoServiceProvider tripledes = new TripleDESCryptoServiceProvider();
            tripledes.Key = llave;
            tripledes.Mode = CipherMode.ECB;
            tripledes.Padding = PaddingMode.PKCS7;
            ICryptoTransform convertir = tripledes.CreateEncryptor(); // Iniciamos la conversión de la cadena
            byte[] resultado = convertir.TransformFinalBlock(arreglo, 0, arreglo.Length); //Arreglo de bytes donde guardaremos la cadena cifrada.
            tripledes.Clear();

            return Convert.ToBase64String(resultado, 0, resultado.Length); // Convertimos la cadena y la regresamos.*/
        }

        // Función que descifra una cadena.
        public string descifrar()
        {
            byte[] data = Convert.FromBase64String(palabra);
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(Encoding.UTF8.GetBytes(clave));
                using (TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform transform = tripDes.CreateDecryptor();
                    byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                    return Encoding.UTF8.GetString(results);
                }
            }

            /*
            byte[] llave;
            byte[] arreglo = Convert.FromBase64String(palabra); // Arreglo donde guardaremos la cadena descovertida.

            // Ciframos utilizando el Algoritmo MD5.
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            llave = md5.ComputeHash(Encoding.UTF8.GetBytes(clave));
            md5.Clear();

            //Ciframos utilizando el Algoritmo 3DES.
            TripleDESCryptoServiceProvider tripledes = new TripleDESCryptoServiceProvider();
            tripledes.Key = llave;
            tripledes.Mode = CipherMode.ECB;
            tripledes.Padding = PaddingMode.PKCS7;
            ICryptoTransform convertir = tripledes.CreateDecryptor();
            byte[] resultado = convertir.TransformFinalBlock(arreglo, 0, arreglo.Length);
            tripledes.Clear();

            string cadena_descifrada = Encoding.UTF8.GetString(resultado); // Obtenemos la cadena
            return cadena_descifrada; // Devolvemos la cadena*/
        }
    }
}