using System;
using System.ComponentModel.DataAnnotations;

namespace DTO
{
    public class OrdenCompraAgregarDTO
    {
        [Required]
        public DateTime fechaRegistro { get; set; }

        [Required]
        public int idTipoDocumento { get; set; }

        [Required]
        [StringLength(20)]
        public string numeroDocumento { get; set; }

        [Required]
        public int idProveedor { get; set; }

        [Required]
        public int idTipoCompra { get; set; }

        [Required]
        public decimal efectivo { get; set; }

        public DateTime? fechaVencimiento { get; set; }

        [StringLength(500)]
        public string observacion { get; set; }

        [Required]
        public decimal total { get; set; }

        [Required]
        public int estado { get; set; }

        public string pcIp { get; set; }
        public string pcHost { get; set; }
        public int idUsuarioLogin { get; set; }
    }
}
