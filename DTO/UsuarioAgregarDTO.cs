using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class UsuarioAgregarDTO:Auditoria
    {      
        public int idPersona { get; set; }                
        public string primerApellido { get; set; }
        public string segundoApellido { get; set; }
        public string nombres { get; set; }        
        public string numeroDocumento { get; set; }
        public int idArea { get; set; }
        public int idCargo { get; set; }
        public string correoInstitucional { get; set; }        
    }

}
