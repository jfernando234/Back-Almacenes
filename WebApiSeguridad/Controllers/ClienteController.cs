using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using AutoMapper;
using DTO;
using Entity;
using Service;

namespace WebApiSeguridad.Controllers
{
    [Route("api/cliente")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper mapper;
        private readonly string cnBD = "";
        private Service.ClienteSER objBss;

        public ClienteController(IConfiguration configuration, IMapper mapper)
        {
            this._configuration = configuration;
            this.mapper = mapper;
            this.cnBD = this._configuration.GetConnectionString("cn_bd_sige");
        }

        /// <summary>
        /// Obtiene todos los clientes activos
        /// </summary>
        /// <returns>Lista de clientes</returns>
        [HttpGet("Listar")]
        public ActionResult<List<DTO.ClienteListarDTO>> Listar()
        {
            try
            {
                objBss = new Service.ClienteSER(_configuration, mapper);
                var clientes = objBss.listar();
                return Ok(clientes);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // El endpoint Obtener por ID fue eliminado según petición del usuario.
        // Ahora se provee ListarPorFecha para filtrar clientes por rango de fecha de creación.

        /// <summary>
        /// Lista clientes filtrando por fecha de creación (inclusive)
        /// </summary>
        /// <param name="inicio">Fecha inicio (yyyy-MM-dd) o datetime válido</param>
        /// <param name="fin">Fecha fin (yyyy-MM-dd) o datetime válido</param>
        [HttpGet("ListarPorFecha")]
        public ActionResult<List<DTO.ClienteListarDTO>> ListarPorFecha([FromQuery] string inicio, [FromQuery] string fin, [FromQuery] string nombre = "")
        {
            try
            {
                DateTime fechaInicio = DateTime.MinValue;
                DateTime fechaFin = DateTime.MinValue;

                bool tieneInicio = !string.IsNullOrWhiteSpace(inicio);
                bool tieneFin = !string.IsNullOrWhiteSpace(fin);

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

                    // Ajustar hora de fin para incluir todo el día
                    fechaFin = fechaFin.Date.AddDays(1).AddTicks(-1);
                }

                objBss = new Service.ClienteSER(_configuration, mapper);
                var clientes = objBss.listarPorFecha(tieneInicio ? fechaInicio : (DateTime?)null, tieneFin ? fechaFin : (DateTime?)null, nombre);
                return Ok(clientes);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Crea un nuevo cliente
        /// </summary>
        /// <param name="clienteDTO">Datos del cliente a crear</param>
        /// <returns>Resultado de la operación</returns>
        [HttpPost("Crear")]
        public ActionResult Crear([FromBody] ClienteAgregarDTO clienteDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Agregar datos de auditoría
                clienteDTO.pcIp = GetClientIP();
                clienteDTO.pcHost = "web";
                clienteDTO.idUsuarioLogin = GetCurrentUserId(); // Implementar según tu sistema de autenticación

                objBss = new Service.ClienteSER(_configuration, mapper);
                var resultado = objBss.agregar(clienteDTO);
                
                if (resultado > 0)
                {
                    return Ok(new { message = "Cliente creado exitosamente", id = resultado });
                }
                else
                {
                    return BadRequest(new { message = "No se pudo crear el cliente" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Actualiza un cliente existente
        /// </summary>
        /// <param name="id">ID del cliente</param>
        /// <param name="clienteDTO">Datos del cliente a actualizar</param>
        /// <returns>Resultado de la operación</returns>
        [HttpPut("Actualizar/{id}")]
        public ActionResult Actualizar(int id, [FromBody] ClienteModificarDTO clienteDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != clienteDTO.idCliente)
                {
                    return BadRequest(new { message = "El ID del parámetro no coincide con el ID del objeto" });
                }

                // Agregar datos de auditoría
                clienteDTO.pcIp = GetClientIP();
                clienteDTO.pcHost = "web";
                clienteDTO.idUsuarioLogin = GetCurrentUserId();

                objBss = new Service.ClienteSER(_configuration, mapper);
                var resultado = objBss.modificar(clienteDTO);
                
                if (resultado > 0)
                {
                    return Ok(new { message = "Cliente actualizado exitosamente" });
                }
                else
                {
                    return BadRequest(new { message = "No se pudo actualizar el cliente" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Deshabilita un cliente (eliminación lógica)
        /// </summary>
        /// <param name="id">ID del cliente</param>
        /// <returns>Resultado de la operación</returns>
        [HttpPut("Deshabilitar/{id}")]
        public ActionResult Deshabilitar(int id)
        {
            try
            {
                objBss = new Service.ClienteSER(_configuration, mapper);
                var resultado = objBss.deshabilitar(
                    id, 
                    GetClientIP(), 
                    "web", 
                    GetCurrentUserId()
                );
                
                if (resultado > 0)
                {
                    return Ok(new { message = "Cliente deshabilitado exitosamente" });
                }
                else
                {
                    return BadRequest(new { message = "No se pudo deshabilitar el cliente" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene la IP del cliente
        /// </summary>
        /// <returns>IP del cliente</returns>
        private string GetClientIP()
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            if (string.IsNullOrEmpty(ipAddress) || ipAddress == "::1")
            {
                ipAddress = "127.0.0.1";
            }
            return ipAddress;
        }

        /// <summary>
        /// Obtiene el ID del usuario actual (implementar según tu sistema de autenticación)
        /// </summary>
        /// <returns>ID del usuario actual</returns>
        private int GetCurrentUserId()
        {
            // TODO: Implementar según tu sistema de autenticación
            // Por ejemplo, desde JWT token o session
            return 1; // Por ahora retorna 1 como usuario por defecto
        }
    }
}