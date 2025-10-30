using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DTO
{
    public class CompraAgregarDTO
    {

        public int TipoDocumentoId { get; set; }
        [Required]
        public string Ruc { get; set; }
        [StringLength(300)]
        public string RazonSocial { get; set; }
        public string Observacion { get; set; }
        public int TipoCompraId { get; set; }
        [Required]
        public decimal Total { get; set; }

        public DateTime FechaRegistro { get; set; }
        // Detalles de productos
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
