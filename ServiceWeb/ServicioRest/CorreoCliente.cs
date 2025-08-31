using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ServiceWeb.ServicioRest
{
    public class CorreoCliente
    {
        private readonly string urlBase = "";
        public string errorMensaje { get; set; } = "";
        public CorreoCliente(string urlBase)
        {
            this.urlBase = urlBase;
        }

        public async Task<bool> postEnviarCorreo(Models.CorreoDTO dto)
        {
            bool value =false;
            string strURI = urlBase + "api/Correo/enviar";
       
            using (var client = new HttpClient())
            {
                var objSerialModelo = JsonConvert.SerializeObject(dto);
                var content = new StringContent(objSerialModelo, Encoding.UTF8, "application/json");
                
                var result = await client.PostAsync(strURI, content);

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Models.Result<bool>>();
                    readTask.Wait();
                    var objModeloResult = readTask.Result;
                    value = objModeloResult.value;
                }
                else
                {
                    string strMensaje = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                    if (strMensaje.Trim().Length < 3)
                        strMensaje = result.StatusCode.ToString();

                    errorMensaje = "Hubo problemas en la operacion: " + strMensaje;
                }
            }

            return value;
        }

    }
}
