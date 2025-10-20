using System;

namespace Entity
{
    public class OrdenCompraDetalleBE : Auditoria
    {
        public int idOrdenCompraDetalle { get; set; }
        public int idOrdenCompra { get; set; }
        public int idProducto { get; set; }
        public int cantidad { get; set; }
        public decimal valorVenta { get; set; }
        public decimal igv { get; set; }
        public decimal precioUnitario { get; set; }
        public decimal subTotal { get; set; }
        public int estado { get; set; }
    }
}
