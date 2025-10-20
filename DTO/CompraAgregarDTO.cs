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
        [Required]
        public int TipoDocumento { get; set; }
        [Required]
        public int ProveedorId { get; set; }
        [Required]
        public int TipoCompra { get; set; }
        [StringLength(200)]
        public string Observacion { get; set; }
        public double Igv { get; set; }
        [Required]
        public double PrecioUnitario { get; set; }
        public double SubTotal { get; set; }
        public double Total { get; set; }
        [Required]
        public DateTime Fecharegistro { get; set; }
        [Required]
        public int ProductoId { get; set; }
        [JsonIgnore]
        public int idUsuarioLogin { get; set; }
        [JsonIgnore]
        public string pcIp { get; set; }
        [JsonIgnore]
        public string pcHost { get; set; }
    }
}
