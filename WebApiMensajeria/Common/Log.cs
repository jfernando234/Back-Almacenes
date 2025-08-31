using System;
using System.IO;

namespace WebApiMensajeria.Common
{
    public class Log
    {
        public static void EscribirLog(string Seccion, string MensajeTitulo, string MensajeSistema, bool EscribirLogWindows)
        {
            //string rutaBase = Assembly.GetExecutingAssembly().Location.Replace("WebApiAntecedentesPide.dll", string.Empty);
            string strNombreArchivo = DateTime.Now.Year + "_" + DateTime.Now.Month.ToString("00") + ".milog"; // log por mes
            string strRuta = Directory.GetCurrentDirectory() + "/Log";
            //string strRuta = rutaBase + "/Log";

            if (Directory.Exists(strRuta) == false)
                Directory.CreateDirectory(strRuta);

            strRuta = strRuta + "/" + strNombreArchivo;

            if (File.Exists(strRuta) == false)
            {
                System.IO.StreamWriter objFile = File.CreateText(strRuta);
                objFile.Close();
            }

            string strMensaje = Seccion + "/" + MensajeTitulo + "/" + MensajeSistema;
            System.IO.StreamWriter fichero = new System.IO.StreamWriter(strRuta, true);

            fichero.WriteLine("--->" + DateTime.Now + " " + strMensaje);
            fichero.Close();
        }
    }
}
