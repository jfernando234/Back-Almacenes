using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class CompraBE : Auditoria
    {
        public int CompraId { get; set; }
        public int TipoDocumento { get; set; }
        public int ProveedorId { get; set; }
        public int TipoCompra { get; set; }
        public string Observacion { get; set; }
        public int ProductoId { get; set; }
        public double Igv { get; set; }
        public double PrecioUnitario { get; set; }
        public double SubTotal { get; set; }
        public DateTime FechaRegistro { get; set; }

        public CompraBE()
        {
            CompraId = 0;
            TipoDocumento = 0;
            ProveedorId = 0;
            TipoCompra = 0;
            Observacion = string.Empty;
            ProductoId = 0;
            Igv = 0;
            PrecioUnitario = 0;
            SubTotal = 0;
            FechaRegistro = DateTime.Now;
        }
    }
}
