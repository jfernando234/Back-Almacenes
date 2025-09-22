using System;

namespace DTO
{
    public class ProveedorListarDTO
    {
        public int idProveedor { get; set; }
        public string ruc { get; set; }
        public string nombre { get; set; }
        public string direccion { get; set; }
        public int telefono { get; set; }
        public string correo { get; set; }
        public string contacto { get; set; }
        public int estado { get; set; }
        public DateTime fechaRegistro { get; set; }
    }
}