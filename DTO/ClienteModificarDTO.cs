using System;
using System.ComponentModel.DataAnnotations;

namespace DTO
{
    public class ClienteModificarDTO : Auditoria
    {
        [Required(ErrorMessage = "El ID del cliente es requerido")]
        public int idCliente { get; set; }

        [Required(ErrorMessage = "El tipo de documento es requerido")]
        public int idTipoDocumento { get; set; }

        [Required(ErrorMessage = "El número de documento es requerido")]
        [StringLength(20, ErrorMessage = "El número de documento no puede exceder 20 caracteres")]
        public string numeroDocumento { get; set; }

        [Required(ErrorMessage = "La razón social es requerida")]
        [StringLength(200, ErrorMessage = "La razón social no puede exceder 200 caracteres")]
        public string razonSocial { get; set; }

    // nombreComercial eliminado según el cambio de esquema

        [StringLength(300, ErrorMessage = "La dirección no puede exceder 300 caracteres")]
        public string direccion { get; set; }

        [StringLength(20, ErrorMessage = "El teléfono no puede exceder 20 caracteres")]
        public string telefono { get; set; }

        [StringLength(100, ErrorMessage = "El correo no puede exceder 100 caracteres")]
        [EmailAddress(ErrorMessage = "El formato del correo no es válido")]
        public string correo { get; set; }

        [StringLength(100, ErrorMessage = "El contacto no puede exceder 100 caracteres")]
        public string contacto { get; set; }

        [Required(ErrorMessage = "El estado es requerido")]
        public int estado { get; set; }
    }
}