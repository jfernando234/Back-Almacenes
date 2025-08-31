using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Map
{
    public  class UsuarioMAP : EntityMap<UsuarioBE>
    {
        public UsuarioMAP()
        {
            Map(m => m.idUsuario).ToColumn("pk_usu_id");
            Map(m => m.login).ToColumn("login_c");
            Map(m => m.clave).ToColumn("clave_v");
            Map(m => m.nuevaClave).ToColumn("clave_nue_b");
            Map(m => m.primerApellido).ToColumn("pri_ape_c");
            Map(m => m.segundoApellido).ToColumn("seg_ape_c");
            Map(m => m.nombres).ToColumn("pre_nom_c");
            Map(m => m.numeroDocumento).ToColumn("num_doc_c");
            Map(m => m.idArea).ToColumn("fk_are_id");
            Map(m => m.area).ToColumn("area_nom");
            Map(m => m.idCargo).ToColumn("fk_cgo_id");
            Map(m => m.cargo).ToColumn("cargo_nom");
            Map(m => m.correoInstitucional).ToColumn("correo_ins_c");
            Map(m => m.idEstado).ToColumn("estado_n");
            Map(m => m.idClientePerfil).ToColumn("idClientePerfil");
            Map(m => m.clientePerfil).ToColumn("clientePerfil");
        }
    }
}
