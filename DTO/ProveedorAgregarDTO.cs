using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DTO
{
    public class ProveedorAgregarDTO
    {
        [Required]
        [StringLength(11, ErrorMessage = "RUC inválido")] 
        public string ruc { get; set; }

        [Required]
        [StringLength(200)]
        public string nombre { get; set; }

        [StringLength(300)]
        public string direccion { get; set; }

        public int telefono { get; set; }

        [EmailAddress]
        public string correo { get; set; }

        [StringLength(100)]
        public string contacto { get; set; }

        [JsonIgnore]
        public int idUsuarioLogin { get; set; }
        [JsonIgnore]
        public string pcIp { get; set; }
        [JsonIgnore]
        public string pcHost { get; set; }
    }
}