using AutoMapper;
using DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Service;
using System;

namespace WebApiSeguridad.Controllers
{
    [Route("api/venta")]
    [ApiController]
    public class VentaController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper mapper;
        private readonly string cnBD = "";
        private ProductoSER objBss;

        public VentaController(IConfiguration configuration, IMapper mapper)
        {
            this._configuration = configuration;
            this.mapper = mapper;
            this.cnBD = this._configuration.GetConnectionString("cn_bd_sige");
        }
        [HttpPost("RegistrarVenta")]
        public ActionResult RegistrarVenta([FromBody] VentaAgregarDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                dto.pcIp = GetClientIP();
                dto.pcHost = "web";
                dto.idUsuarioLogin = 1; // aquí tu lógica para obtener usuario logeado

                var servicio = new VentaSER(_configuration, mapper);
                var resultado = servicio.Agregar(dto);

                if (resultado > 0)
                    return Ok(new { message = "Venta registrada exitosamente", id = resultado });
                else
                    return BadRequest(new { message = "No se pudo registrar la venta" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        private string GetClientIP()
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            if (string.IsNullOrEmpty(ipAddress) || ipAddress == "::1")
            {
                ipAddress = "127.0.0.1";
            }
            return ipAddress;
        }

        private int GetCurrentUserId()
        {
            return 1;
        }
    }
}
