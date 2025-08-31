using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Map
{
    public class PerfilMAP :EntityMap<PerfilBE>
    {
        public PerfilMAP()
        {
            Map(m => m.idPerfil).ToColumn("pk_per_id");
            Map(m => m.nombrePerfil).ToColumn("nombre_c");
            Map(m => m.descripcionPerfil).ToColumn("descrip_c");
        }
    }
}
