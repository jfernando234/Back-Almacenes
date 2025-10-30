using Dapper;
using Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
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
        public List<Entity.CompraListBE> listarAll()
        {
            sqlQuery = "SELECT tipo_documento_id AS TipoDocumentoId, " +
           "ruc AS Ruc, " +
           "razon_social AS RazonSocial, " +
           "observacion AS Observacion, " +
           "tipo_compra_id AS TipoCompraId, " +
           "total AS Total, " +
           "fecha_registro AS FechaRegistro " +
           "FROM " + bdEsquema + bdTabla + " " +
           "WHERE aud_es_eli_b = 0 " +
           "ORDER BY razon_social";


            using (SqlConnection sqlCn = new SqlConnection(this.cnBD))
            {
                try
                {
                    sqlCn.Open();
                    var lista = sqlCn.Query<Entity.CompraListBE>(sqlQuery).ToList();
                    return lista;
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine($"Error en CompraDA.listarAll(): {ex.Message}");
                    System.Console.WriteLine($"Query: {sqlQuery}");
                    System.Console.WriteLine($"Connection String: {this.cnBD}");
                    throw;
                }
            }
        }
        public int agregar(Entity.CompraBE entidad)
        {
            int value = 0;

            using (SqlConnection sqlCn = new SqlConnection(this.cnBD))
            {
                sqlCn.Open();
                using (var tran = sqlCn.BeginTransaction())
                {
                    try
                    {
                        string sqlQuery = "INSERT INTO " + bdEsquema + bdTabla +
                        "(tipo_documento_id, ruc, razon_social," +
                        "observacion," +
                        "tipo_compra_id, total,fecha_registro, " +
                        "aud_id_usu_cre_n, aud_fec_cre_f, aud_id_usu_mod_n, aud_fec_mod_f," +
                        "aud_pc_ip_c, aud_pc_host_c, aud_es_eli_b)" +
                        "VALUES" +
                        "(@tipo_documento_id, @ruc, @razon_social," +
                        "@observacion," +
                        "@tipo_compra_id,@total, @fecha_registro, " +
                        "@aud_id_usu_cre_n, @aud_fec_cre_f, @aud_id_usu_mod_n, @aud_fec_mod_f," +
                        "@aud_pc_ip_c, @aud_pc_host_c, 0)" +
                        "SELECT CAST(SCOPE_IDENTITY() as int)";

                        value = sqlCn.QuerySingle<int>(sqlQuery, new
                        {
                            tipo_documento_id = entidad.TipoDocumentoId,
                            ruc = entidad.Ruc,
                            razon_social = entidad.RazonSocial,
                            observacion = entidad.Observacion,
                            tipo_compra_id = entidad.TipoCompraId, 
                            total = entidad.Total,

                            fecha_registro = entidad.FechaRegistro,

                            aud_id_usu_cre_n = entidad.idUsuarioLogin,
                            aud_fec_cre_f = DateTime.Now,
                            aud_id_usu_mod_n = entidad.idUsuarioLogin,
                            aud_fec_mod_f = DateTime.Now,
                            aud_pc_ip_c = entidad.pcIp,
                            aud_pc_host_c = entidad.pcHost
                        }, tran);

                        // Insertar detalle de venta y actualizar stock
                        foreach (var det in entidad.Detalles)
                        {
                            string sqlDetalle = @"INSERT INTO detalle_compra
                            (compra_id, producto_id, cantidad, precio_unitario, total)
                            VALUES
                            (@compra_id, @producto_id, @cantidad, @precio_unitario, @total);";

                            sqlCn.Execute(sqlDetalle, new
                            {
                                compra_id = value,          // coincidir con @venta_id
                                producto_id = det.ProductoId,
                                cantidad = det.Cantidad,
                                precio_unitario = det.PrecioUnitario,
                                total = det.Total
                            }, tran);

                            string sqlStock = "UPDATE dbo.producto_mae SET stock = stock + @cantidad WHERE pk_prod_id = @producto_id";
                            sqlCn.Execute(sqlStock, new
                            {
                                cantidad = det.Cantidad,
                                producto_id = det.ProductoId
                            }, tran);
                        }

                        tran.Commit();
                        return value;
                    }
                    catch
                    {
                        tran.Rollback();
                        throw;
                    }
                }
            }
        }

    }
}
