using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Map
{
    public class ModuloMAP : EntityMap<ModuloBE>
    {
        public ModuloMAP()
        {
            Map(m => m.idModulo).ToColumn("pk_mod_id");
            Map(m => m.idModuloPadre).ToColumn("fk_mod_id");
            Map(m => m.nombreModuloPadre).ToColumn("nom_mod_pdr");
            Map(m => m.nivel).ToColumn("nivel_n");
            Map(m => m.esHijo).ToColumn("es_mod_hij_b");
            Map(m => m.nombreModulo).ToColumn("nombre_c");
            Map(m => m.nombreObjeto).ToColumn("nombre_obj_c");
            Map(m => m.codigoAccesoDirecto).ToColumn("codigo_acc_dir_c");
            Map(m => m.icono).ToColumn("icono_c");
            Map(m => m.url).ToColumn("url_c");
            Map(m => m.numeroOrden).ToColumn("num_ord");
            Map(m => m.esVisible).ToColumn("es_visible_b");
        }
    }
}
