using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class UsuarioPerfilBE:Auditoria
    {
        public int idUsuarioPerfil { get; set; }
        public int idUsuario { get; set; }
        public int idPerfil { get; set; }
        public string nombrePerfil { get; set; }    
    }

    
}
