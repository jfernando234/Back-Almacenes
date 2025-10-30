using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class CompraListarDTO
    {
        public int TipoDocumentoId { get; set; }
        public string Ruc { get; set; }
        public string RazonSocial { get; set; }
        public string Observacion { get; set; }
        public int TipoCompraId { get; set; }
        public decimal Total { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
