using System;

namespace DTO
{
    public class ClienteListarDTO
    {
        public int idCliente { get; set; }
        public int idTipoDocumento { get; set; }
        public string numeroDocumento { get; set; }
        public string razonSocial { get; set; }
        public string direccion { get; set; }
        public string telefono { get; set; }
        public string correo { get; set; }
        public string contacto { get; set; }
        public int estado { get; set; }
        public string estadoTexto { get; set; }
        public DateTime? fechaCreacion { get; set; }

        public ClienteListarDTO()
        {
            idCliente = 0;
            idTipoDocumento = 0;
            numeroDocumento = string.Empty;
            razonSocial = string.Empty;
        // nombreComercial eliminado según el cambio de esquema
            direccion = string.Empty;
            telefono = string.Empty;
            correo = string.Empty;
            contacto = string.Empty;
            estado = 0;
            estadoTexto = string.Empty;
            fechaCreacion = null;
        }
    }
}