using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Entity;
using Newtonsoft.Json;
using static Service.EnvioWSP;

namespace Service
{
    public class EnvioWSPSE
    {
        private readonly HttpClient _httpClient;
        public EnvioWSPSE()
        {
            _httpClient = new HttpClient();
        }
        public async Task<Boolean> TemplateEnvioWSP(string template, string numero)
        {
            string url = "https://graph.facebook.com/v15.0/116132664728017/messages";
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            string token = "EAARb4gPZBeRMBALgZBNlmcR6g6YVjm8DdZBjwEV67J73czB4fkVkfW4MOOuwwsxrBgsZCpI33e3rjvHuMGF5m1OuMpU9FcIvQz46kHjdAYRaf4L3ZCJkn4ATRzMuk0yjEGwmvZBXagQC8Qlunxyj7S5ZAw4deU7jHzAPV6wSf0W6B95ZCORORSLkNzpYWtdpZCbl9aFZBKocDqcQZDZD";
            
            var data = new EnvioWSP.Peticion();
            var data_template = new EnvioWSP.Template();
            data_template.name = template;
            data.messaging_product = "whatsapp";
            data.to = numero;
            data.type = "template";
            data.template = new EnvioWSP.Template
            {
                name = template,
                language = new
                {
                    code = "es"
                },
            };

            request.Content = new StringContent(JsonConvert.SerializeObject(data));
            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.SendAsync(request);
            return true;
        }

    }
}
