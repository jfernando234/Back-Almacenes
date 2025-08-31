using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class UsuarioPerfilAgregarDTO : Auditoria
    {        
        public int idUsuario { get; set; }
        public int idPerfil { get; set; }
    }
}
