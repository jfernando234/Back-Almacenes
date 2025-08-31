using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Common
{
    public static class Cifrado
    {
        public static byte[] encriptarSHA256(string texto)
        {
            
            byte[] value;

            //convertir el texto de entrada en un array de bytes
            byte[] cadenaUTFBytes = System.Text.Encoding.UTF8.GetBytes(texto);
            
            using (SHA256 mySHA256 = SHA256.Create())
            {
                 value = mySHA256.ComputeHash(cadenaUTFBytes);
            }

            return value;
            
        }

        public static bool compararBytes(byte[] ar1, byte[] ar2)
        {

            if (ar1 == null || ar2 == null) return false;
            
            // Si no coincide la longitud de los arrays devolvemos false
            if (ar1.Length != ar2.Length)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < ar1.Length; i++)
                {
                    // Mientras coincidan nunca entrará aqui.
                    if (!(ar1[i].Equals(ar2[i])))
                    {
                        // Si no coinciden, devolvemos false
                        return false;
                    }
                }
                // Si realiza todo el bucle devolvemos true.
                return true;
            }
        }
    }
}
