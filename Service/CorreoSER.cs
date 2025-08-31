using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class CorreoSER
    {

        //private readonly string linkApp = "<a href='http://10.10.10.40:8090/' target='_blank'> SIGE </a> <br>  <br>";

        public async Task crearUsuario(string linkApp, string ambiente, string ApellidosyNombres, string usuarioLogin, string usuarioClave,
            string usuarioCorreo)
        {
            bool value = false;
            /*enviar correo, notificando al usuario*/
            string asunto = "Creación de cuenta de usuario - Sistema SIGE";
            string mensaje = "Estimado (a): <br>" +
                             ApellidosyNombres.ToUpper() + "<br> <br>" +
                             "Su cuenta ha sido creado, para efectos de autentificarse (inicio sesión) al Sistema SIGE, ingresar los siguientes datos; <br> <br>" +
                             "Ambiente: " + ambiente + "<br>" +
                             "Usuario: " + usuarioLogin + "<br>" +
                             "Clave: " + usuarioClave + "<br> <br>" +
                             "Link: " + getLinkFortmatHTML(linkApp) +
                             "Por motivos de seguridad, usted debe cambiar su clave inicial y realizar el mantenimiento de cambio de clave frecuentemente.";

            string mensajeError = "";

            value = enviarCorreo(asunto, mensaje, new List<string> { usuarioCorreo }, null, out mensajeError);

        }

        public async Task restaurarClaveUsuario(string linkApp, string ambiente, string ApellidosyNombres, string usuarioLogin, 
            string usuarioClave,string usuarioCorreo)
        {
            bool value = false;
            /*enviar correo, notificando al usuario*/
            string asunto = "Restauración de cuenta de usuario - Sistema SIGE";
            string mensaje = "Estimado (a): <br>" +
                             ApellidosyNombres.ToUpper() + "<br> <br>" +
                             "Su cuenta de Usuario ha sido restaurado, para efectos de autentificarse (inicio sesión) al Sistema SIGE, ingresar los siguientes datos; <br> <br>" +
                             "Ambiente: " + ambiente + "<br>" +
                             "Usuario: " + usuarioLogin + "<br>" +
                             "Clave: " + usuarioClave + "<br> <br>" +
                             "Link: " + getLinkFortmatHTML(linkApp) +
                             "Por motivos de seguridad, usted debe cambiar su clave inicial y realizar el mantenimiento de cambio de clave frecuentemente.";

            string mensajeError = "";

            value = enviarCorreo(asunto, mensaje, new List<string> { usuarioCorreo }, null, out mensajeError);

        }

        private string getLinkFortmatHTML(string linkAPP  )
        {
            return $"<a href='{linkAPP}' target='_blank'> SIGE </a> <br>  <br>";
        }
        private bool enviarCorreo(string asunto, string mensaje, List<string> listaPara, List<string> listaCC,
                                          out string mensajeOut)
        {

            ServiceWeb.Correo correo = new ServiceWeb.Correo();
            return correo.enviarCorreo(asunto, mensaje, listaPara, listaCC, out mensajeOut);

        }
    }
}
