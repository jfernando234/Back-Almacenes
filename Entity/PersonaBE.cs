using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class PersonaBE : Auditoria
    {
        public int idPersona { get; set; }
        public int idTipoPersona { get; set; }
        public int idTipoDocumento { get; set; }
        public string numeroDocumento { get; set; }
        public string primerApellido { get; set; }
        public string segundoApellido { get; set; }
        public string nombres { get; set; }
        public string ApellidosyNombres
        {
            get
            {
                return primerApellido + " " + segundoApellido + ", " + nombres;
            }
        }
        public DateTime fechaNacimiento { get; set; }
        public bool sexo { get; set; }
        public string correo { get; set; }
        public string clave { get; set; }
        public string login { get; set; }
        public string contacto { get; set; }
        public string celular { get; set; }
    }
}
