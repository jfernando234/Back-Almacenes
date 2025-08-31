using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class UsuarioRestaurarClaveDTO:Auditoria
    {
        public string login { get; set; }
        public string numeroDocumento { get; set; }
        public string correoInstitucional { get; set; }
    }
}
