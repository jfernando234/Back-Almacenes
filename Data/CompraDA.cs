using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class CompraDA
    {
        private readonly string cnBD = "";
        private string sqlQuery = "";
        private readonly string bdEsquema = "dbo.";
        private readonly string bdTabla = "compra";
        public CompraDA(string cnBD)
        {
            this.cnBD = cnBD;
        }
        public int agregar(Entity.CompraBE entidad)
        {
            int value = 0;
            string sqlQuery = "INSERT INTO " + bdEsquema + bdTabla  +
                               "(fecha_registro, tipo_documento, fk_id_proveedor, tipo_compra, observacion, pf_id_producto, igv, precio_unitario, sub_total, " +
                               "aud_id_usu_cre_n, aud_fec_cre_f, aud_id_usu_mod_n, aud_fec_mod_f, aud_pc_ip_c, aud_pc_host_c, aud_es_eli_b) " +
                               "VALUES " +
                               "(@fecha_registro, @tipo_documento, @fk_id_proveedor, @tipo_compra, @observacion, @pf_id_producto, @igv, @precio_unitario, @sub_total, " +
                               "@aud_id_usu_cre_n, @aud_fec_cre_f, @aud_id_usu_mod_n, @aud_fec_mod_f, @aud_pc_ip_c, @aud_pc_host_c, 0)";
            using (SqlConnection sqlCn = new SqlConnection(this.cnBD))
            {
                value = sqlCn.Execute(sqlQuery, new
                {
                    fecha_registro = entidad.FechaRegistro,
                    tipo_documento = entidad.TipoDocumento,
                    fk_id_proveedor = entidad.ProveedorId,
                    tipo_compra = entidad.TipoCompra,
                    observacion = entidad.Observacion,
                    pf_id_producto = entidad.ProductoId,
                    igv = entidad.Igv,
                    precio_unitario = entidad.PrecioUnitario,
                    sub_total = entidad.SubTotal,
                    aud_id_usu_cre_n = entidad.idUsuarioLogin,
                    aud_fec_cre_f = DateTime.Now,
                    aud_id_usu_mod_n = entidad.idUsuarioLogin,
                    aud_fec_mod_f = DateTime.Now,
                    aud_pc_ip_c = entidad.pcIp,
                    aud_pc_host_c = entidad.pcHost
                });
                return value;
            }

        }
    }
}
