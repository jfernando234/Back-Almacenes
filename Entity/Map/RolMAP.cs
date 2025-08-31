using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Map
{
    public class RolMAP : EntityMap<RolBE>
    {
        public RolMAP()
        {
            Map(m => m.idRol).ToColumn("pk_rol_id");
            Map(m => m.idModulo).ToColumn("fk_mod_id");
            Map(m => m.nombreRol).ToColumn("nombre_c");
            Map(m => m.descripcionRol).ToColumn("descrip_c");
            Map(m => m.valorNumerico).ToColumn("val_numerico_n");
            Map(m => m.valorCadena).ToColumn("val_cadena_c");
            Map(m => m.valorLogico).ToColumn("val_logico_b");

        }
    }
}
