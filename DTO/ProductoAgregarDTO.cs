using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DTO
{
    public class ProductoAgregarDTO
    {
        [Required]
        [StringLength(200)]
        public string NombreProducto { get; set; }

        [Required]
        public double PrecioEntrada { get; set; }

        [Required]
        public double PrecioSalida { get; set; }

        public int Stock { get; set; }

        [JsonIgnore]
        public int idUsuarioLogin { get; set; }
        [JsonIgnore]
        public string pcIp { get; set; }
        [JsonIgnore]
        public string pcHost { get; set; }
    }
}
