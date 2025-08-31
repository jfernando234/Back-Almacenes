using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class CatalogosSER
    {
        private readonly string cnBD = "";
        private readonly IMapper mapper;
        private readonly HttpClient _httpClient;

        public CatalogosSER(string cnBD)
        {
            this.cnBD = cnBD;
            this._httpClient = new HttpClient();
        }
        public async Task<DTO.dniDTO> getDNI(string accessToken, string dni)
        {
            try
            {
                var requestUri = "https://dniruc.apisperu.com/api/v1/dni/" + dni + "?token=" + accessToken;
                DTO.dniDTO value = new DTO.dniDTO();
                var request = new HttpRequestMessage(HttpMethod.Get, requestUri);

                //request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await _httpClient.SendAsync(request);
                string responseBody = await response.Content.ReadAsStringAsync();

                value = JsonConvert.DeserializeObject<DTO.dniDTO>(responseBody);
                return value;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<DTO.rucDTO> getRUC(string accessToken, string ruc)
        {
            var requestUri = "https://dniruc.apisperu.com/api/v1/ruc/" + ruc + "?token=" + accessToken;
            DTO.rucDTO value = new DTO.rucDTO();
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);

            //request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.SendAsync(request);
            string responseBody = await response.Content.ReadAsStringAsync();

            value = JsonConvert.DeserializeObject<DTO.rucDTO>(responseBody);

            return value;
        }
    }
}
