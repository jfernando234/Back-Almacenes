using System;

namespace DTO
{
    public class OrdenCompraListarDTO
    {
        public int idOrdenCompra { get; set; }
        public DateTime fechaRegistro { get; set; }
        public int idTipoDocumento { get; set; }
        public string numeroDocumento { get; set; }
        public int idProveedor { get; set; }
        public string nombreProveedor { get; set; }
        public int idTipoCompra { get; set; }
        public decimal efectivo { get; set; }
        public DateTime? fechaVencimiento { get; set; }
        public string observacion { get; set; }
        public decimal total { get; set; }
        public int estado { get; set; }
    }
}
