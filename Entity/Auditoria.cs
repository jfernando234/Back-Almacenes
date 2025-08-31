using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class Auditoria
    {
        public int idUsuarioLogin { get; set; } 
        public int idUsuarioCreacion { get; set; }
        public string usuarioCreacion { get; set;}
        public DateTime fechaCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public string usuarioModificacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public int idUsuarioEliminacion { get; set; }
        public string usuarioEliminacion { get; set; }
        public DateTime fechaEliminacion { get; set; }
        public string  pcIp { get; set; }
        public string pcHost { get; set; }

    }
}
