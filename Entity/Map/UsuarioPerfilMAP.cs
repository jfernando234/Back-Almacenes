using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Map
{
    public class UsuarioPerfilMAP : EntityMap<UsuarioPerfilBE>
    {
        public UsuarioPerfilMAP()
        {
            Map(m => m.idUsuarioPerfil).ToColumn("pk_usu_per_mod");
            Map(m => m.idUsuario).ToColumn("fk_usu_id");
            Map(m => m.idPerfil).ToColumn("fk_per_id");
            Map(m => m.nombrePerfil).ToColumn("perfil_nom");
        }
    }
}
