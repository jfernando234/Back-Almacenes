using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class UsuarioRolAgregarDTO : Auditoria
    {        
        public int idUsuario { get; set; }
        public int idRol { get; set; }
    }
}
