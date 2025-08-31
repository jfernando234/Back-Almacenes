using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class UsuarioRolBE:Auditoria
    {
        public int idUsuarioRol { get; set; }
        public string descPerfil { get; set; }
        public int idUsuarioPerfil { get; set; }
        public int idRolModulo { get; set; }
        public int idRol{ get; set; }
        public string nombreRol { get; set; }
        public string descripRol { get; set; }
        public string nombreModulo { get; set; }
        public Boolean sel { get; set; }
        public Boolean tiene_detalle { get; set; }
    }

    public class UsuarioRolDetBE : Auditoria
    {
        public int idUsuarioRolDet { get; set; }
        public string descOpcion { get; set; }
        public Boolean sel { get; set; }
    }

}
