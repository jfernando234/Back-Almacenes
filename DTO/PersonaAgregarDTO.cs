using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class PersonaAgregarDTO
    {
        public int idTipoPersona { get; set; }
        public int idTipoDocumento { get; set; }
        public string numeroDocumento { get; set; }
        public string primerApellido { get; set; }
        public string segundoApellido { get; set; }
        public string nombres { get; set; }
        public DateTime fechaNacimiento { get; set; }
        public bool sexo { get; set; }
        public string correo { get; set; }
        public string login { get; set; }
        public string clave { get; set; }
        public string contacto { get; set; }
        public string celular { get; set; }
    }
    public class PersonaModificarDTO : PersonaAgregarDTO
    {
        public int idPersona { get; set; }
    }
}
