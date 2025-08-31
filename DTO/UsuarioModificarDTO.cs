using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class UsuarioModificarDTO:Auditoria
    {
        public int idUsuario { get; set; }
        public string login { get; set; }
        public int idArea { get; set; }
        public int idCargo { get; set; }
        public string correoInstitucional { get; set; }
        public int idestado { get; set; }
    }
}
