using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class PerfilAgregarDTO:Auditoria
    {        
        public string nombrePerfil { get; set; }
        public string descripcionPerfil { get; set; }
    }
}
