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
    [HttpGet("ListarAllClientes")]
    public ActionResult<List<DTO.ClienteListarDTO>> ListarAllClientes()
        {
            try
            {
                objBss = new Service.ClienteSER(_configuration, mapper);
                var clientes = objBss.listarAll();
                return Ok(clientes);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Filtra clientes por fecha de creación y/o nombre de contacto
        /// </summary>
        /// <param name="inicio">Fecha inicio (yyyy-MM-dd) o datetime válido</param>
        /// <param name="fin">Fecha fin (yyyy-MM-dd) o datetime válido</param>
        /// <param name="nombre">Nombre de contacto para filtrar (búsqueda parcial)</param>
    [HttpGet("FiltrarClientes")]
    public ActionResult<List<DTO.ClienteListarDTO>> FiltrarClientes([FromQuery] string inicio, [FromQuery] string fin, [FromQuery] string nombre = "")
        {
            try
            {
                DateTime fechaInicio = DateTime.MinValue;
                DateTime fechaFin = DateTime.MinValue;

                bool tieneInicio = !string.IsNullOrWhiteSpace(inicio);
                bool tieneFin = !string.IsNullOrWhiteSpace(fin);
                bool tieneNombre = !string.IsNullOrWhiteSpace(nombre);

                // Validar que se proporcione al menos un criterio de filtro
                if (!tieneInicio && !tieneFin && !tieneNombre)
                {
                    return BadRequest(new { message = "Debe proporcionar al menos un criterio de filtro: fecha (inicio y fin) o nombre." });
                }

                // Si se proporciona fecha, ambos parámetros son requeridos
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
                var clientes = objBss.filtro(tieneInicio ? fechaInicio : (DateTime?)null, tieneFin ? fechaFin : (DateTime?)null, nombre);
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
    [HttpPost("RegistrarCliente")]
    public ActionResult RegistrarCliente([FromBody] ClienteAgregarDTO clienteDTO)
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
        /// Deshabilita un cliente (eliminación lógica)
        /// </summary>
        /// <param name="id">ID del cliente</param>
        /// <returns>Resultado de la operación</returns>
    [HttpPut("EliminarCliente/{id}")]
    public ActionResult EliminarCliente(int id)
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