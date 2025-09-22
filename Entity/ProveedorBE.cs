using System;

namespace Entity
{
    public class ProveedorBE : Auditoria
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
    public bool eliminado { get; set; }

        public ProveedorBE()
        {
            idProveedor = 0;
            ruc = string.Empty;
            nombre = string.Empty;
            direccion = string.Empty;
            telefono = 0;
            correo = string.Empty;
            contacto = string.Empty;
            estado = 1; 
            fechaRegistro = DateTime.Now;
            eliminado = false;
        }
    }
}