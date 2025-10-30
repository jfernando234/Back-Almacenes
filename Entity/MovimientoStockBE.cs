using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class MovimientoStockBE
    {
        public int Id { get; set; }
        public int ProductoId { get; set; }
        public int AlmacenOrigenId { get; set; } // null si es entrada
        public int AlmacenDestinoId { get; set; } // null si es salida
        public int Cantidad { get; set; }
        public string TipoMovimiento { get; set; } // ENTRADA, SALIDA, TRANSFERENCIA
        public DateTime Fecha { get; set; }
        public string Observacion { get; set; }
    }
}
