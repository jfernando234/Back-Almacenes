using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class VentaBE : Auditoria
    {
        public int VentaId { get; set; }
        public int TipoDocumentoId { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int TipoPagoId { get; set; }
        public string Dni { get; set; }
        public string NombreCliente { get; set; }
        public string ApellidosCliente { get; set; }
        public string Observacion { get; set; }
        public int TipoMetodoPagoId { get; set; }
        public int? TipoTarjetaId { get; set; }
        public decimal MontoRecibido { get; set; }
        public decimal Vuelto { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Igv { get; set; }
        public decimal Total { get; set; }

        public List<DetalleVentaBE> Detalles { get; set; } = new List<DetalleVentaBE>();
    }
    public class DetalleVentaBE
    {
        public int ProductoId { get; set; }
        public decimal Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Total { get; set; }
    }

}
