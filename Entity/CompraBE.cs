using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class CompraBE : Auditoria
    {
        public int VentaId { get; set; }
        public int TipoDocumentoId { get; set; }
        public string Ruc { get; set; }
        public string RazonSocial { get; set; }
        public string Observacion { get; set; }
        public int TipoCompraId { get; set; }
        public decimal Total { get; set; }
        public DateTime FechaRegistro { get; set; }
        public List<DetalleCompraBE> Detalles { get; set; } = new List<DetalleCompraBE>();
    }
    public class DetalleCompraBE
    {
        public int ProductoId { get; set; }
        public decimal Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Total { get; set; }
    }
    public class CompraListBE
    {
        public int TipoDocumentoId { get; set; }
        public string Ruc { get; set; }
        public string RazonSocial { get; set; }
        public string Observacion { get; set; }
        public int TipoCompraId { get; set; }
        public decimal Total { get; set; }
        public DateTime FechaRegistro { get; set; }
    }

    public class CompraListMes
    {
        public string Mes { get; set; }
        public int TotalCompras { get; set; }
    }

    public class TotalProductos
    {
        public string NombreProducto { get; set; }
        public decimal TotalComprado { get; set; }
    }
}
