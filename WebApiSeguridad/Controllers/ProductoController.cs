using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using AutoMapper;
using DTO;
using Service;

namespace WebApiSeguridad.Controllers
{
    [Route("api/producto")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper mapper;
        private readonly string cnBD = "";
        private ProductoSER objBss;

        public ProductoController(IConfiguration configuration, IMapper mapper)
        {
            this._configuration = configuration;
            this.mapper = mapper;
            this.cnBD = this._configuration.GetConnectionString("cn_bd_sige");
        }

        [HttpGet("ListarAllProductos")]
        public ActionResult<List<DTO.ProductoListarDTO>> ListarAllProductos()
        {
            try
            {
                objBss = new ProductoSER(_configuration, mapper);
                var lista = objBss.listarAll();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("FiltrarProductos")]
        public ActionResult<List<DTO.ProductoListarDTO>> FiltrarProductos([FromQuery] string inicio, [FromQuery] string fin, [FromQuery] string nombre = "")
        {
            try
            {
                DateTime fechaInicio = DateTime.MinValue;
                DateTime fechaFin = DateTime.MinValue;

                bool tieneInicio = !string.IsNullOrWhiteSpace(inicio);
                bool tieneFin = !string.IsNullOrWhiteSpace(fin);
                bool tieneNombre = !string.IsNullOrWhiteSpace(nombre);

                if (!tieneInicio && !tieneFin && !tieneNombre)
                {
                    return BadRequest(new { message = "Debe proporcionar al menos un criterio de filtro: fecha (inicio y fin) o nombre." });
                }

                if (tieneInicio ^ tieneFin)
                {
                    return BadRequest(new { message = "Si filtras por fecha debes proporcionar ambos parámetros 'inicio' y 'fin'." });
                }

                if (tieneInicio && tieneFin)
                {
                    if (!DateTime.TryParse(inicio, out fechaInicio))
                    {
                        return BadRequest(new { message = "Parámetro 'inicio' inválido. Use formato yyyy-MM-dd o datetime válido." });
                    }

                    if (!DateTime.TryParse(fin, out fechaFin))
                    {
                        return BadRequest(new { message = "Parámetro 'fin' inválido. Use formato yyyy-MM-dd o datetime válido." });
                    }

                    fechaFin = fechaFin.Date.AddDays(1).AddTicks(-1);
                }

                objBss = new ProductoSER(_configuration, mapper);
                var lista = objBss.filtro(tieneInicio ? fechaInicio : (DateTime?)null, tieneFin ? fechaFin : (DateTime?)null, nombre);
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("RegistrarProducto")]
        public ActionResult RegistrarProducto([FromBody] ProductoAgregarDTO dto)
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

                objBss = new ProductoSER(_configuration, mapper);
                var resultado = objBss.agregar(dto);

                if (resultado > 0)
                {
                    return Ok(new { message = "Producto creado exitosamente", id = resultado });
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

        [HttpPut("EliminarProducto/{id}")]
        public ActionResult EliminarProducto(int id)
        {
            try
            {
                objBss = new ProductoSER(_configuration, mapper);
                var resultado = objBss.deshabilitar(id, GetClientIP(), "web", GetCurrentUserId());

                if (resultado > 0)
                {
                    return Ok(new { message = "Producto deshabilitado exitosamente" });
                }
                else
                {
                    return BadRequest(new { message = "No se pudo deshabilitar el producto" });
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
