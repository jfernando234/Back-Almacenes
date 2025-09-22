using System;

namespace Entity
{
    public class ProductoBE : Auditoria
    {
        public int ProductoId { get; set; }
        public string NombreProducto { get; set; }
        public double PrecioEntrada { get; set; }
        public double PrecioSalida { get; set; }
        public int Stock { get; set; }
        public int Estado { get; set; }
        public DateTime FechaRegistro { get; set; }
        public bool eliminado { get; set; }

        public ProductoBE()
        {
            ProductoId = 0;
            NombreProducto = string.Empty;
            PrecioEntrada = 0;
            PrecioSalida = 0;
            Stock = 0;
            Estado = 1;
            FechaRegistro = DateTime.Now;
            eliminado = false;
        }
    }
}
