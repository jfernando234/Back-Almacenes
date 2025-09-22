using System;

namespace DTO
{
    public class ProductoListarDTO
    {
        public int ProductoId { get; set; }
        public string NombreProducto { get; set; }
        public double PrecioEntrada { get; set; }
        public double PrecioSalida { get; set; }
        public int Stock { get; set; }
        public int Estado { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
