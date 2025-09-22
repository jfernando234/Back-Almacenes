using System;

namespace Entity
{
    public class ClienteBE : Auditoria
    {
        public int idCliente { get; set; }
        public int idTipoDocumento { get; set; }
        public string numeroDocumento { get; set; }
        public string razonSocial { get; set; }
    // nombreComercial eliminado según el cambio de esquema
        public string direccion { get; set; }
        public string telefono { get; set; }
        public string correo { get; set; }
        public string contacto { get; set; }
        public int estado { get; set; }
    public bool eliminado { get; set; }

        // Campos adicionales para joins
        public string tipoDocumentoNombre { get; set; }
        public string tipoDocumentoAbrev { get; set; }
        public string estadoTexto { get; set; }

        public ClienteBE()
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
            estado = 1; // Activo por defecto
            tipoDocumentoNombre = string.Empty;
            tipoDocumentoAbrev = string.Empty;
            estadoTexto = string.Empty;
            eliminado = false;
        }
    }
}