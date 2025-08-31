using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;

namespace WebApiMensajeria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CorreoController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string serverMail = "";
        private readonly int serverPuerto = 0;
        private readonly string usuarioFrom = "";
        private readonly string usuarioClave = "";
        private readonly string listaCorreoCCO = "";

        public CorreoController(IConfiguration configuracion)
        {
            this._configuration = configuracion;

            this.serverMail = _configuration["Correo_Config:Server"];
            this.serverPuerto = int.Parse(_configuration["Correo_Config:Port"]);
            this.usuarioFrom = _configuration["Correo_Config:UserFrom"];
            this.usuarioClave = _configuration["Correo_Config:UserPassword"];
            listaCorreoCCO = _configuration["Correo_Config:CCOSoporte"];
            
        }

        [HttpGet]
        [Route("obtenerFecha")]
        public string getFecha()
        {
            return DateTime.Now.ToString();
        }

        [HttpPost]
        [Route("enviar")]
        public IActionResult EnviarCorreo(Entity.CorreoBE dto)
        {
            Service.CorreoSER objBss = new Service.CorreoSER(serverMail, serverPuerto, usuarioFrom, usuarioClave, listaCorreoCCO);
            Entity.Result<bool> result = new Entity.Result<bool>();

            result = objBss.enviar(dto);

            return Ok(result);
        }
    }
}
