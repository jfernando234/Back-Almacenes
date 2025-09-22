using Dapper.FluentMap.Mapping;
using Entity;

namespace Entity.Map
{
    public class ProveedorMAP : EntityMap<ProveedorBE>
    {
        public ProveedorMAP()
        {
            Map(p => p.idProveedor).ToColumn("pk_prov_id");
            Map(p => p.ruc).ToColumn("ruc");
            Map(p => p.nombre).ToColumn("nombre");
            Map(p => p.direccion).ToColumn("direccion");
            Map(p => p.telefono).ToColumn("telefono");
            Map(p => p.correo).ToColumn("correo");
            Map(p => p.contacto).ToColumn("contacto");
            Map(p => p.estado).ToColumn("estado");

            // Auditoría
            Map(p => p.idUsuarioLogin).ToColumn("aud_id_usu_cre_n");
            Map(p => p.fechaCreacion).ToColumn("aud_fec_cre_f");
            Map(p => p.idUsuarioModificacion).ToColumn("aud_id_usu_mod_n");
            Map(p => p.fechaModificacion).ToColumn("aud_fec_mod_f");
            Map(p => p.idUsuarioEliminacion).ToColumn("aud_id_usu_eli_n");
            Map(p => p.fechaEliminacion).ToColumn("aud_fec_eli_f");
            Map(p => p.pcIp).ToColumn("aud_pc_ip_c");
            Map(p => p.pcHost).ToColumn("aud_pc_host_c");
            Map(p => p.eliminado).ToColumn("aud_es_eli_b");

        }
    }
}