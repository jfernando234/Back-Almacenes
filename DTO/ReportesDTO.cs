using System;
using System.Collections.Generic;

namespace DTO
{
    /// <summary>
    /// DTO para datos de Valoración de Inventario
    /// </summary>
    public class ReporteValoracionInventarioDTO
    {
        public int ProductoId { get; set; }
        public string NombreProducto { get; set; }
        public int Stock { get; set; }
        public decimal PrecioEntrada { get; set; }
        public decimal PrecioSalida { get; set; }
        public decimal ValorTotal { get; set; }
        public decimal Rotacion { get; set; }
        public string Categoria { get; set; }
    }

    /// <summary>
    /// DTO para resumen de Valoración de Inventario
    /// </summary>
    public class ResumeValoracionInventarioDTO
    {
        public int TotalProductos { get; set; }
        public int TotalUnidades { get; set; }
        public decimal ValorInventarioTotal { get; set; }
        public decimal PromedioValorProducto { get; set; }
        public List<ReporteValoracionInventarioDTO> Detalles { get; set; }
    }

    /// <summary>
    /// DTO para Movimientos de Stock
    /// </summary>
    public class ReporteMovimientoStockDTO
    {
        public int MovimientoId { get; set; }
        public int ProductoId { get; set; }
        public string NombreProducto { get; set; }
        public string TipoMovimiento { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Total { get; set; }
        public DateTime FechaMovimiento { get; set; }
        public string Usuario { get; set; }
        public string Observacion { get; set; }
        public int StockResultante { get; set; }
    }

    /// <summary>
    /// DTO para filtros de Movimientos de Stock
    /// </summary>
    public class FiltroMovimientoStockDTO
    {
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public int? ProductoId { get; set; }
        public string TipoMovimiento { get; set; }
    }

    /// <summary>
    /// DTO para resumen de Movimientos de Stock
    /// </summary>
    public class ResumenMovimientoStockDTO
    {
        public int TotalMovimientos { get; set; }
        public int TotalEntradas { get; set; }
        public int TotalSalidas { get; set; }
        public int MovimientoNeto { get; set; }
        public decimal MontoTotal { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public List<ReporteMovimientoStockDTO> Movimientos { get; set; }
    }

    /// <summary>
    /// DTO para Rendimiento de Proveedores
    /// </summary>
    public class ReporteRendimientoProveedorDTO
    {
        public int ProveedorId { get; set; }
        public string NombreProveedor { get; set; }
        public string RUC { get; set; }
        public string Contacto { get; set; }
        public int TotalOrdenesCompra { get; set; }
        public decimal MontoTotalComprado { get; set; }
        public int ComprasOnTime { get; set; }
        public int ComprasRetrasadas { get; set; }
        public decimal TasaCumplimiento { get; set; }
        public decimal PromedioTiempoEntrega { get; set; }
        public string CalificacionGlobal { get; set; }
        public List<DetalleCompraProveedorDTO> UltimasCompras { get; set; }
    }

    /// <summary>
    /// DTO para detalles de compras por proveedor
    /// </summary>
    public class DetalleCompraProveedorDTO
    {
        public int OrdenCompraId { get; set; }
        public DateTime FechaOrden { get; set; }
        public DateTime FechaEntegaEsperada { get; set; }
        public DateTime? FechaEntregaReal { get; set; }
        public decimal Monto { get; set; }
        public string Estado { get; set; }
        public int DiasDiferencia { get; set; }
    }

    /// <summary>
    /// DTO para resumen de Rendimiento de Proveedores
    /// </summary>
    public class ResumenRendimientoProveedorDTO
    {
        public int TotalProveedores { get; set; }
        public int TotalOrdenesCompra { get; set; }
        public decimal MontoTotalComprado { get; set; }
        public decimal TasaCumplimientoPromedio { get; set; }
        public string ProveedorEstrella { get; set; }
        public List<ReporteRendimientoProveedorDTO> Proveedores { get; set; }
    }

    /// <summary>
    /// DTO para Historial de Clientes
    /// </summary>
    public class ReporteHistorialClienteDTO
    {
        public int ClienteId { get; set; }
        public string RazonSocial { get; set; }
        public string NumeroDocumento { get; set; }
        public string Contacto { get; set; }
        public string Correo { get; set; }
        public int TotalOrdenes { get; set; }
        public decimal MontoTotalComprado { get; set; }
        public DateTime UltimaCompra { get; set; }
        public string EstadoCliente { get; set; }
        public decimal PromedioCompra { get; set; }
        public List<DetalleCompraClienteDTO> Compras { get; set; }
    }

    /// <summary>
    /// DTO para detalles de compras por cliente
    /// </summary>
    public class DetalleCompraClienteDTO
    {
        public int OrdenId { get; set; }
        public DateTime FechaOrden { get; set; }
        public decimal Monto { get; set; }
        public string Estado { get; set; }
        public int Dias { get; set; }
    }

    /// <summary>
    /// DTO para resumen de Historial de Clientes
    /// </summary>
    public class ResumenHistorialClienteDTO
    {
        public int TotalClientes { get; set; }
        public int ClientesActivos { get; set; }
        public int ClientesInactivos { get; set; }
        public decimal MontoTotalVentas { get; set; }
        public decimal PromedioVentasCliente { get; set; }
        public string ClientePrincipal { get; set; }
        public List<ReporteHistorialClienteDTO> Clientes { get; set; }
    }

    /// <summary>
    /// DTO para Análisis ABC de Productos
    /// </summary>
    public class ReporteAnalisisABCDTO
    {
        public int ProductoId { get; set; }
        public string NombreProducto { get; set; }
        public int Stock { get; set; }
        public decimal ValorTotal { get; set; }
        public decimal PorcentajeValor { get; set; }
        public string Clasificacion { get; set; }
        public int Rotacion { get; set; }
    }

    /// <summary>
    /// DTO para resumen de Análisis ABC
    /// </summary>
    public class ResumenAnalisisABCDTO
    {
        public int TotalProductos { get; set; }
        public int ProductosA { get; set; }
        public int ProductosB { get; set; }
        public int ProductosC { get; set; }
        public decimal ValorProductosA { get; set; }
        public decimal ValorProductosB { get; set; }
        public decimal ValorProductosC { get; set; }
        public List<ReporteAnalisisABCDTO> Productos { get; set; }
    }

    /// <summary>
    /// DTO para Productos con Stock Crítico
    /// </summary>
    public class ReporteStockCriticoDTO
    {
        public int ProductoId { get; set; }
        public string NombreProducto { get; set; }
        public int StockActual { get; set; }
        public int StockMinimo { get; set; }
        public int Diferencia { get; set; }
        public string Urgencia { get; set; }
        public DateTime? UltimaCompra { get; set; }
    }

    /// <summary>
    /// DTO para resumen de Stock Crítico
    /// </summary>
    public class ResumenStockCriticoDTO
    {
        public int TotalProductosCriticos { get; set; }
        public int ProductosMuyCriticos { get; set; }
        public int ProductosCriticos { get; set; }
        public int ProductosAlerta { get; set; }
        public List<ReporteStockCriticoDTO> Productos { get; set; }
    }
}
