using Service;
using System;
using System.Collections.Generic;

namespace AppDemoConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
           
            
            //byte[] claveGrabadaBytes = Common.Cifrado.encriptarSHA256(claveGrabada);

            //Console.WriteLine("Ingrese Clave: ");
            //string claveIngresada = Console.ReadLine();
            //byte[] claveIngresadaBytes = Common.Cifrado.encriptarSHA256(claveIngresada);

            //if (Common.Cifrado.compararBytes(claveGrabadaBytes, claveIngresadaBytes)==true )
            //{
            //    Console.WriteLine("Clave correcta");
            //}
            //else
            //{
            //    Console.WriteLine("Clave Erronea");
            //}

            //enviar correo al usuario, para notificar su clave restaurada
            //CorreoSER correo = new CorreoSER();
            //correo.restaurarClaveUsuario("Velorio Trucios, Cesar Augusto", "CVELORIO", "CVELORIO", "cavelorio@roda.com.pe");


            Console.ReadKey();

        }
    }
}
