using System;
using System.Collections.Generic;
using AutoMapper;
using DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Service;

namespace WebApiSeguridad.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdenCompraController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper mapper;
        private OrdenCompraSER objBss;

        public OrdenCompraController(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            this.mapper = mapper;
        }

        private string GetClientIP()
        {
            return HttpContext.Connection.RemoteIpAddress?.ToString() ?? "0.0.0.0";
        }

        private int GetCurrentUserId()
        {
            return 1;
        }

        [HttpGet("ListarAllOrdenesCompra")]
        public ActionResult<List<OrdenCompraListarDTO>> ListarAllOrdenesCompra()
        {
            try
            {
                objBss = new Service.OrdenCompraSER(_configuration, mapper);
                var ordenes = objBss.listar();
                return Ok(ordenes);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("ObtenerOrdenCompra/{id}")]
        public ActionResult<OrdenCompraListarDTO> ObtenerOrdenCompra(int id)
        {
            try
            {
                objBss = new Service.OrdenCompraSER(_configuration, mapper);
                var orden = objBss.listar(id);

                if (orden == null)
                    return NotFound(new { message = "Orden de compra no encontrada" });

                return Ok(orden);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("FiltrarOrdenesCompra")]
        public ActionResult<List<OrdenCompraListarDTO>> FiltrarOrdenesCompra([FromQuery] DateTime inicio, [FromQuery] DateTime fin)
        {
            try
            {
                objBss = new Service.OrdenCompraSER(_configuration, mapper);
                var ordenes = objBss.filtrar(inicio, fin);
                return Ok(ordenes);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("RegistrarOrdenCompra")]
        public ActionResult RegistrarOrdenCompra([FromBody] OrdenCompraAgregarDTO ordenCompraDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                ordenCompraDTO.pcIp = GetClientIP();
                ordenCompraDTO.pcHost = "web";
                ordenCompraDTO.idUsuarioLogin = GetCurrentUserId();

                objBss = new Service.OrdenCompraSER(_configuration, mapper);
                int id = objBss.agregar(ordenCompraDTO);

                return Ok(new { message = "Orden de compra registrada exitosamente", id = id });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("ModificarOrdenCompra")]
        public ActionResult ModificarOrdenCompra([FromBody] OrdenCompraModificarDTO ordenCompraDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                ordenCompraDTO.pcIp = GetClientIP();
                ordenCompraDTO.pcHost = "web";
                ordenCompraDTO.idUsuarioLogin = GetCurrentUserId();

                objBss = new Service.OrdenCompraSER(_configuration, mapper);
                var resultado = objBss.modificar(ordenCompraDTO);

                if (resultado > 0)
                {
                    return Ok(new { message = "Orden de compra modificada exitosamente" });
                }
                else
                {
                    return BadRequest(new { message = "No se pudo modificar la orden de compra" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("EliminarOrdenCompra/{id}")]
        public ActionResult EliminarOrdenCompra(int id)
        {
            try
            {
                string pcIp = GetClientIP();
                string pcHost = "web";
                int idUsuarioLogin = GetCurrentUserId();

                objBss = new Service.OrdenCompraSER(_configuration, mapper);
                int resultado = objBss.deshabilitar(id, pcIp, pcHost, idUsuarioLogin);

                if (resultado > 0)
                {
                    return Ok(new { message = "Orden de compra deshabilitada exitosamente" });
                }
                else
                {
                    return BadRequest(new { message = "No se pudo deshabilitar la orden de compra" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
