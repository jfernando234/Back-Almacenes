using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Map
{
    public class UsuarioRolMAP : EntityMap<UsuarioRolBE>
    {
        public UsuarioRolMAP()
        {
            Map(m => m.idUsuarioRol).ToColumn("pk_rolmod_usuper_id");
            Map(m => m.idUsuarioPerfil).ToColumn("pk_usu_per_mod");
            Map(m => m.idRolModulo).ToColumn("pk_rol_mod_id");
            Map(m => m.nombreRol).ToColumn("nom_rol_c");
            Map(m => m.descripRol).ToColumn("descrip_rol_c");
            Map(m => m.nombreModulo).ToColumn("modulo");
            Map(m => m.sel).ToColumn("sel");
            Map(m => m.tiene_detalle).ToColumn("tiene_detalle_b");
        }
    }
}
