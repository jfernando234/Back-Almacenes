using Dapper.FluentMap.Mapping;
using Entity;

namespace Entity.Map
{
    public class ClienteMAP : EntityMap<ClienteBE>
    {
        public ClienteMAP()
        {
            Map(p => p.idCliente).ToColumn("pk_cli_id");
            Map(p => p.idTipoDocumento).ToColumn("fk_tip_doc_id");
            Map(p => p.numeroDocumento).ToColumn("numero_documento");
            Map(p => p.razonSocial).ToColumn("razon_social");
            // nombre_comercial eliminado de la tabla; ya no se mapea
            Map(p => p.direccion).ToColumn("direccion");
            Map(p => p.telefono).ToColumn("telefono");
            Map(p => p.correo).ToColumn("correo");
            Map(p => p.contacto).ToColumn("contacto");
            Map(p => p.estado).ToColumn("estado");

            // Campos de auditoría
            Map(p => p.idUsuarioLogin).ToColumn("aud_id_usu_cre_n");
            Map(p => p.fechaCreacion).ToColumn("aud_fec_cre_f");
            Map(p => p.idUsuarioModificacion).ToColumn("aud_id_usu_mod_n");
            Map(p => p.fechaModificacion).ToColumn("aud_fec_mod_f");
            Map(p => p.idUsuarioEliminacion).ToColumn("aud_id_usu_eli_n");
            Map(p => p.fechaEliminacion).ToColumn("aud_fec_eli_f");
            Map(p => p.pcIp).ToColumn("aud_pc_ip_c");
            Map(p => p.pcHost).ToColumn("aud_pc_host_c");
            Map(p => p.eliminado).ToColumn("aud_es_eli_b");

            // Campos adicionales (no mapear columnas que no existen en la tabla)
            Map(p => p.tipoDocumentoNombre).Ignore();
            Map(p => p.tipoDocumentoAbrev).Ignore();
            Map(p => p.estadoTexto).Ignore();
        }
    }
}