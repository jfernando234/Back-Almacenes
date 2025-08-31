using ServiceWeb.ServicioRest;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceWeb
{
    public class Correo
    {
        //private readonly string urlBase = "http://localhost:49121/";
        //private readonly string urlBase = "http://localhost:5000/";

        //private readonly string urlBase = "http://10.10.10.40:8085/DEV/WebApiMensajeria/";

        private readonly string urlBase = "http://10.10.10.44:8093/";
        private CorreoCliente servicio = null;
      
        public bool enviarCorreo(string asunto, string mensaje, List<string> listaPara, List<string> listaCC, 
                                    out string mensajeOut)
        {

            bool value = false;
            servicio = new CorreoCliente(this.urlBase);
            Models.CorreoDTO dto = new Models.CorreoDTO();

            dto.asunto = asunto;
            dto.mensaje = mensaje;
            dto.listaPara = listaPara;
            dto.listaCC = listaCC;         

            Task<bool> task = servicio.postEnviarCorreo(dto);

            value = task.Result;
            mensajeOut = servicio.errorMensaje;

            return value;
        }
    }
}
