using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class UsuarioCambiarClaveDTO:Auditoria
    {
        public int idUsuario { get; set; }
        public string login { get; set; }
        public string claveAnterior { get; set; }
        public string claveNueva { get; set; }

    }
}
