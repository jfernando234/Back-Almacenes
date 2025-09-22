using Dapper.FluentMap.Mapping;
using Entity;

namespace Entity.Map
{
    public class ProductoMAP : EntityMap<ProductoBE>
    {
        public ProductoMAP()
        {
            Map(p => p.ProductoId).ToColumn("pk_prod_id");
            Map(p => p.NombreProducto).ToColumn("nombre_producto");
            Map(p => p.PrecioEntrada).ToColumn("precio_entrada");
            Map(p => p.PrecioSalida).ToColumn("precio_salida");
            Map(p => p.Stock).ToColumn("stock");
            Map(p => p.Estado).ToColumn("estado");
            Map(p => p.FechaRegistro).ToColumn("aud_fec_cre_f");

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
