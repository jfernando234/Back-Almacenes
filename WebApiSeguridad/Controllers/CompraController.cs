using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using AutoMapper;
using DTO;
using Service;

namespace WebApiSeguridad.Controllers
{
    [Route("api/Compra")]
    [ApiController]
    public class CompraController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper mapper;
        private readonly string cnBD = "";
        private CompraSER objBss;
        public CompraController(IConfiguration configuration, IMapper mapper)
        {
            this._configuration = configuration;
            this.mapper = mapper;
            this.cnBD = this._configuration.GetConnectionString("cn_bd_sige");
        }

        [HttpPost("RegistrarCompra")]
        public ActionResult RegistrarCompra([FromBody] CompraAgregarDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                dto.pcIp = GetClientIP();
                dto.pcHost = "web";
                dto.idUsuarioLogin = GetCurrentUserId();

                objBss = new CompraSER(_configuration, mapper);
                var resultado = objBss.agregar(dto);

                if (resultado > 0)
                {
                    return Ok(new { message = "compra creado exitosamente", id = resultado });
                }
                else
                {
                    return BadRequest(new { message = "No se pudo crear el producto" });
                }
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
