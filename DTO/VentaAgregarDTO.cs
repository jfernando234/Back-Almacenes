using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DTO
{
    public class VentaAgregarDTO
    {
        
        public int TipoDocumentoId { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int TipoPagoId { get; set; }

        // Cliente directo
        [Required]
        public string Dni { get; set; }
        [StringLength(300)]
        public string NombreCliente { get; set; }
        [StringLength(300)]
        public string ApellidosCliente { get; set; }
        [StringLength(300)]
        public string Observacion { get; set; }

        // Métodos de pago
        public int TipoMetodoPagoId { get; set; }
        public int? TipoTarjetaId { get; set; }

        public decimal MontoRecibido { get; set; }
        public decimal Vuelto { get; set; }

        // Totales
        public decimal SubTotal { get; set; }
        public decimal Igv { get; set; }
        [Required]
        public decimal Total { get; set; }

        // Detalles de productos
        [Required]
        public List<DetalleVentaDTO> Detalles { get; set; } = new List<DetalleVentaDTO>();

        // Auditoría
        [JsonIgnore]
        public int idUsuarioLogin { get; set; }
        [JsonIgnore]
        public string pcIp { get; set; }
        [JsonIgnore]
        public string pcHost { get; set; }
    }
    public class DetalleVentaDTO
    {
        public int ProductoId { get; set; }
        public decimal Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Total { get; set; }
    }
}
