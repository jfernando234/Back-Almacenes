using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class PerfilBE :Auditoria
    {
        public int idPerfil { get; set; }
        public string nombrePerfil { get; set; }
        public string descripcionPerfil { get; set; }
    }
}
