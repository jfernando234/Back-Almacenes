using AutoMapper;
using DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Service;
using System;
using System.Collections.Generic;

namespace WebApiSeguridad.Controllers
{
    [Route("api/compra")]
    [ApiController]
    public class CompraController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper mapper;
        private readonly string cnBD = "";
        private CompraSer objBss;

        public CompraController(IConfiguration configuration, IMapper mapper)
        {
            this._configuration = configuration;
            this.mapper = mapper;
            this.cnBD = this._configuration.GetConnectionString("cn_bd_sige");
        }
        [HttpGet("ListarCompra")]
        public ActionResult<List<DTO.CompraListarDTO>> ListarAllProductos()
        {
            try
            {
                objBss = new CompraSer(_configuration, mapper);
                var lista = objBss.listarAll();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPost("RegistrarCompra")]
        public ActionResult RegistrarVenta([FromBody] CompraAgregarDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                dto.pcIp = GetClientIP();
                dto.pcHost = "web";
                dto.idUsuarioLogin = GetCurrentUserId(); // aquí tu lógica para obtener usuario logeado

                objBss = new CompraSer(_configuration, mapper);
                var resultado = objBss.Agregar(dto);

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
