using AutoMapper;
using Data;
using DTO;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class ReportesSER
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly string _cnBD;

        public ReportesSER(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;
            _cnBD = _configuration.GetConnectionString("cn_bd_sige");
        }

        /// <summary>
        /// Obtiene el resumen de Valoración de Inventario
        /// </summary>
        public ResumeValoracionInventarioDTO ObtenerValoracionInventario()
        {
            try
            {
                var da = new ReportesDA(_cnBD);
                var datos = da.ObtenerValoracionInventario();

                var detalles = datos.Select(d => new ReporteValoracionInventarioDTO
                {
                    ProductoId = Convert.ToInt32(d["ProductoId"]),
                    NombreProducto = d["NombreProducto"]?.ToString() ?? "",
                    Stock = Convert.ToInt32(d["Stock"]),
                    PrecioEntrada = Convert.ToDecimal(d["PrecioEntrada"]),
                    PrecioSalida = Convert.ToDecimal(d["PrecioSalida"]),
                    ValorTotal = Convert.ToDecimal(d["ValorTotal"]),
                    Rotacion = 0 // Se podría calcular si tenemos datos históricos
                }).ToList();

                var totalValor = detalles.Sum(d => d.ValorTotal);

                return new ResumeValoracionInventarioDTO
                {
                    TotalProductos = detalles.Count,
                    TotalUnidades = detalles.Sum(d => d.Stock),
                    ValorInventarioTotal = totalValor,
                    PromedioValorProducto = detalles.Count > 0 ? totalValor / detalles.Count : 0,
                    Detalles = detalles
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener valoración de inventario: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Obtiene el resumen de Movimientos de Stock
        /// </summary>
        public ResumenMovimientoStockDTO ObtenerMovimientosStock(DateTime? fechaInicio = null, 
            DateTime? fechaFin = null, int? productoId = null, string tipoMovimiento = null)
        {
            try
            {
                // Establecer rango de fechas por defecto si no se proporciona
                if (!fechaInicio.HasValue)
                    fechaInicio = DateTime.Now.AddMonths(-1);
                if (!fechaFin.HasValue)
                    fechaFin = DateTime.Now;

                var da = new ReportesDA(_cnBD);
                var datos = da.ObtenerMovimientosStock(fechaInicio, fechaFin, productoId, tipoMovimiento);

                var movimientos = datos.Select(d => new ReporteMovimientoStockDTO
                {
                    MovimientoId = Convert.ToInt32(d["MovimientoId"]),
                    ProductoId = Convert.ToInt32(d["ProductoId"]),
                    NombreProducto = d["NombreProducto"]?.ToString() ?? "",
                    TipoMovimiento = d["TipoMovimiento"]?.ToString() ?? "",
                    Cantidad = Convert.ToInt32(d["Cantidad"]),
                    PrecioUnitario = Convert.ToDecimal(d["PrecioUnitario"]),
                    Total = Convert.ToDecimal(d["Total"]),
                    FechaMovimiento = Convert.ToDateTime(d["FechaMovimiento"]),
                    StockResultante = Convert.ToInt32(d["StockResultante"])
                }).ToList();

                var entradas = movimientos.Where(m => m.TipoMovimiento == "Compra").Sum(m => m.Cantidad);
                var salidas = movimientos.Where(m => m.TipoMovimiento == "Venta").Sum(m => m.Cantidad);

                return new ResumenMovimientoStockDTO
                {
                    TotalMovimientos = movimientos.Count,
                    TotalEntradas = entradas,
                    TotalSalidas = salidas,
                    MovimientoNeto = entradas - salidas,
                    MontoTotal = movimientos.Sum(m => m.Total),
                    FechaInicio = fechaInicio.Value,
                    FechaFin = fechaFin.Value,
                    Movimientos = movimientos
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener movimientos de stock: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Obtiene el resumen de Rendimiento de Proveedores
        /// </summary>
        public ResumenRendimientoProveedorDTO ObtenerRendimientoProveedores()
        {
            try
            {
                var da = new ReportesDA(_cnBD);
                var datos = da.ObtenerRendimientoProveedores();

                var proveedores = datos.Select(d => new ReporteRendimientoProveedorDTO
                {
                    ProveedorId = Convert.ToInt32(d["ProveedorId"]),
                    NombreProveedor = d["NombreProveedor"]?.ToString() ?? "",
                    RUC = d["RUC"]?.ToString() ?? "",
                    Contacto = d["Contacto"]?.ToString() ?? "",
                    TotalOrdenesCompra = Convert.ToInt32(d["TotalOrdenesCompra"]),
                    MontoTotalComprado = Convert.ToDecimal(d["MontoTotalComprado"]),
                    ComprasOnTime = 0,
                    ComprasRetrasadas = 0,
                    TasaCumplimiento = 100,
                    PromedioTiempoEntrega = 0,
                    CalificacionGlobal = ObtenerCalificacionProveedor(Convert.ToDecimal(d["MontoTotalComprado"]), Convert.ToInt32(d["TotalOrdenesCompra"]))
                }).ToList();

                var montoTotal = proveedores.Sum(p => p.MontoTotalComprado);

                return new ResumenRendimientoProveedorDTO
                {
                    TotalProveedores = proveedores.Count,
                    TotalOrdenesCompra = proveedores.Sum(p => p.TotalOrdenesCompra),
                    MontoTotalComprado = montoTotal,
                    TasaCumplimientoPromedio = 100,
                    ProveedorEstrella = proveedores.OrderByDescending(p => p.MontoTotalComprado).FirstOrDefault()?.NombreProveedor,
                    Proveedores = proveedores
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener rendimiento de proveedores: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Obtiene el resumen de Historial de Clientes
        /// </summary>
        public ResumenHistorialClienteDTO ObtenerHistorialClientes()
        {
            try
            {
                var da = new ReportesDA(_cnBD);
                var datos = da.ObtenerHistorialClientes();

                var clientes = datos.Select(d => new ReporteHistorialClienteDTO
                {
                    ClienteId = Convert.ToInt32(d["ClienteId"]),
                    RazonSocial = d["RazonSocial"]?.ToString() ?? "",
                    NumeroDocumento = d["NumeroDocumento"]?.ToString() ?? "",
                    Contacto = d["Contacto"]?.ToString() ?? "",
                    Correo = d["Correo"]?.ToString() ?? "",
                    TotalOrdenes = Convert.ToInt32(d["TotalOrdenes"]),
                    MontoTotalComprado = Convert.ToDecimal(d["MontoTotalComprado"]),
                    UltimaCompra = Convert.ToDateTime(d["UltimaCompra"]),
                    EstadoCliente = Convert.ToInt32(d["EstadoCliente"]) == 1 ? "Activo" : "Inactivo",
                    PromedioCompra = Convert.ToInt32(d["TotalOrdenes"]) > 0 ? Convert.ToDecimal(d["MontoTotalComprado"]) / Convert.ToInt32(d["TotalOrdenes"]) : 0
                }).ToList();

                var montoTotal = clientes.Sum(c => c.MontoTotalComprado);
                var clientesActivos = clientes.Count(c => c.EstadoCliente == "Activo");

                return new ResumenHistorialClienteDTO
                {
                    TotalClientes = clientes.Count,
                    ClientesActivos = clientesActivos,
                    ClientesInactivos = clientes.Count - clientesActivos,
                    MontoTotalVentas = montoTotal,
                    PromedioVentasCliente = clientes.Count > 0 ? montoTotal / clientes.Count : 0,
                    ClientePrincipal = clientes.OrderByDescending(c => c.MontoTotalComprado).FirstOrDefault()?.RazonSocial,
                    Clientes = clientes
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener historial de clientes: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Obtiene el resumen de Stock Crítico
        /// </summary>
        public ResumenStockCriticoDTO ObtenerStockCritico(int limiteStock = 20)
        {
            try
            {
                var da = new ReportesDA(_cnBD);
                var datos = da.ObtenerStockCritico(limiteStock);

                var productos = datos.Select(d => new ReporteStockCriticoDTO
                {
                    ProductoId = Convert.ToInt32(d["ProductoId"]),
                    NombreProducto = d["NombreProducto"]?.ToString() ?? "",
                    StockActual = Convert.ToInt32(d["StockActual"]),
                    StockMinimo = Convert.ToInt32(d["StockMinimo"]),
                    Diferencia = Convert.ToInt32(d["Diferencia"]),
                    Urgencia = d["Urgencia"]?.ToString() ?? ""
                }).ToList();

                return new ResumenStockCriticoDTO
                {
                    TotalProductosCriticos = productos.Count,
                    ProductosMuyCriticos = productos.Count(p => p.Urgencia == "Muy Crítico"),
                    ProductosCriticos = productos.Count(p => p.Urgencia == "Crítico"),
                    ProductosAlerta = productos.Count(p => p.Urgencia == "Alerta"),
                    Productos = productos
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener stock crítico: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Obtiene el análisis ABC de productos
        /// </summary>
        public ResumenAnalisisABCDTO ObtenerAnalisisABC()
        {
            try
            {
                var da = new ReportesDA(_cnBD);
                var datos = da.ObtenerAnalisisABC();

                var productos = datos.Select(d => new ReporteAnalisisABCDTO
                {
                    ProductoId = Convert.ToInt32(d["ProductoId"]),
                    NombreProducto = d["NombreProducto"]?.ToString() ?? "",
                    Stock = Convert.ToInt32(d["Stock"]),
                    ValorTotal = Convert.ToDecimal(d["ValorTotal"]),
                    PorcentajeValor = Convert.ToDecimal(d["PorcentajeValor"]),
                    Clasificacion = d["Clasificacion"]?.ToString() ?? ""
                }).ToList();

                var productosA = productos.Where(p => p.Clasificacion == "A").ToList();
                var productosB = productos.Where(p => p.Clasificacion == "B").ToList();
                var productosC = productos.Where(p => p.Clasificacion == "C").ToList();

                return new ResumenAnalisisABCDTO
                {
                    TotalProductos = productos.Count,
                    ProductosA = productosA.Count,
                    ProductosB = productosB.Count,
                    ProductosC = productosC.Count,
                    ValorProductosA = productosA.Sum(p => p.ValorTotal),
                    ValorProductosB = productosB.Sum(p => p.ValorTotal),
                    ValorProductosC = productosC.Sum(p => p.ValorTotal),
                    Productos = productos
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener análisis ABC: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Calcula la calificación del proveedor basada en monto y número de compras
        /// </summary>
        private string ObtenerCalificacionProveedor(decimal monto, int ordenes)
        {
            if (ordenes >= 10 && monto >= 50000)
                return "Excelente";
            else if (ordenes >= 5 && monto >= 20000)
                return "Muy Bueno";
            else if (ordenes >= 2 && monto >= 5000)
                return "Bueno";
            else if (ordenes > 0)
                return "Regular";
            else
                return "Sin Historial";
        }
    }
}
