using System;
using System.Collections.Generic;
using AutoMapper;
using DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Service;

namespace WebApiSeguridad.Controllers
{
    [Route("api/reportes")]
    [ApiController]
    public class ReportesController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private ReportesSER _reportesSER;

        public ReportesController(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtiene el reporte de Valoración de Inventario
        /// GET: api/reportes/valoracion-inventario
        /// </summary>
        [HttpGet("valoracion-inventario")]
        public ActionResult<ResumeValoracionInventarioDTO> ObtenerValoracionInventario()
        {
            try
            {
                _reportesSER = new ReportesSER(_configuration, _mapper);
                var reporte = _reportesSER.ObtenerValoracionInventario();
                return Ok(reporte);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al obtener valoración de inventario: {ex.Message}" });
            }
        }

        /// <summary>
        /// Obtiene el reporte de Movimientos de Stock
        /// GET: api/reportes/movimientos-stock?fechaInicio=2024-01-01&fechaFin=2024-12-31&productoId=1&tipoMovimiento=Compra
        /// </summary>
        [HttpGet("movimientos-stock")]
        public ActionResult<ResumenMovimientoStockDTO> ObtenerMovimientosStock(
            [FromQuery] string fechaInicio = null,
            [FromQuery] string fechaFin = null,
            [FromQuery] int? productoId = null,
            [FromQuery] string tipoMovimiento = null)
        {
            try
            {
                DateTime? inicio = null;
                DateTime? fin = null;

                if (!string.IsNullOrWhiteSpace(fechaInicio) && DateTime.TryParse(fechaInicio, out DateTime parsedInicio))
                    inicio = parsedInicio;

                if (!string.IsNullOrWhiteSpace(fechaFin) && DateTime.TryParse(fechaFin, out DateTime parsedFin))
                    fin = parsedFin;

                _reportesSER = new ReportesSER(_configuration, _mapper);
                var reporte = _reportesSER.ObtenerMovimientosStock(inicio, fin, productoId, tipoMovimiento);
                return Ok(reporte);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al obtener movimientos de stock: {ex.Message}" });
            }
        }

        /// <summary>
        /// Obtiene el reporte de Rendimiento de Proveedores
        /// GET: api/reportes/rendimiento-proveedores
        /// </summary>
        [HttpGet("rendimiento-proveedores")]
        public ActionResult<ResumenRendimientoProveedorDTO> ObtenerRendimientoProveedores()
        {
            try
            {
                _reportesSER = new ReportesSER(_configuration, _mapper);
                var reporte = _reportesSER.ObtenerRendimientoProveedores();
                return Ok(reporte);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al obtener rendimiento de proveedores: {ex.Message}" });
            }
        }

        /// <summary>
        /// Obtiene el reporte de Historial de Clientes
        /// GET: api/reportes/historial-clientes
        /// </summary>
        [HttpGet("historial-clientes")]
        public ActionResult<ResumenHistorialClienteDTO> ObtenerHistorialClientes()
        {
            try
            {
                _reportesSER = new ReportesSER(_configuration, _mapper);
                var reporte = _reportesSER.ObtenerHistorialClientes();
                return Ok(reporte);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al obtener historial de clientes: {ex.Message}" });
            }
        }

        /// <summary>
        /// Obtiene el reporte de Stock Crítico
        /// GET: api/reportes/stock-critico?limiteStock=20
        /// </summary>
        [HttpGet("stock-critico")]
        public ActionResult<ResumenStockCriticoDTO> ObtenerStockCritico([FromQuery] int limiteStock = 20)
        {
            try
            {
                if (limiteStock <= 0)
                    return BadRequest(new { message = "El límite de stock debe ser mayor a 0" });

                _reportesSER = new ReportesSER(_configuration, _mapper);
                var reporte = _reportesSER.ObtenerStockCritico(limiteStock);
                return Ok(reporte);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al obtener stock crítico: {ex.Message}" });
            }
        }

        /// <summary>
        /// Obtiene el reporte de Análisis ABC de Productos
        /// GET: api/reportes/analisis-abc
        /// </summary>
        [HttpGet("analisis-abc")]
        public ActionResult<ResumenAnalisisABCDTO> ObtenerAnalisisABC()
        {
            try
            {
                _reportesSER = new ReportesSER(_configuration, _mapper);
                var reporte = _reportesSER.ObtenerAnalisisABC();
                return Ok(reporte);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al obtener análisis ABC: {ex.Message}" });
            }
        }

        /// <summary>
        /// Obtiene todos los reportes disponibles (resumen rápido)
        /// GET: api/reportes/resumen-general
        /// </summary>
        [HttpGet("resumen-general")]
        public ActionResult<dynamic> ObtenerResumenGeneral()
        {
            try
            {
                _reportesSER = new ReportesSER(_configuration, _mapper);

                var valoracion = _reportesSER.ObtenerValoracionInventario();
                var stockCritico = _reportesSER.ObtenerStockCritico();
                var proveedores = _reportesSER.ObtenerRendimientoProveedores();
                var clientes = _reportesSER.ObtenerHistorialClientes();

                var resumen = new
                {
                    Inventario = new
                    {
                        TotalProductos = valoracion.TotalProductos,
                        TotalUnidades = valoracion.TotalUnidades,
                        ValorTotal = valoracion.ValorInventarioTotal,
                        ProductosCriticos = stockCritico.TotalProductosCriticos
                    },
                    Proveedores = new
                    {
                        Total = proveedores.TotalProveedores,
                        TotalCompras = proveedores.TotalOrdenesCompra,
                        MontoTotal = proveedores.MontoTotalComprado
                    },
                    Clientes = new
                    {
                        Total = clientes.TotalClientes,
                        Activos = clientes.ClientesActivos,
                        MontoTotalVentas = clientes.MontoTotalVentas
                    },
                    FechaGeneracion = DateTime.Now
                };

                return Ok(resumen);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al obtener resumen general: {ex.Message}" });
            }
        }
    }
}
