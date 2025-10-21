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
    public class VentaDA
    {
        private readonly string cnBD = "";
        private string sqlQuery = "";
        private readonly string bdEsquema = "dbo.";
        private readonly string bdTabla = "venta";

        public VentaDA(string cnBD)
        {
            this.cnBD = cnBD;
        }
        public int qweqe(Entity.VentaBE entidad)
        {
            int value = 0;
            sqlQuery = "INSERT INTO " + bdEsquema + bdTabla +
                       "(tipo_documento_id, fecha_registro, tipo_pago_id, dni, nombre_cliente, apellidos_cliente," +
                       "observacion, tipo_metodo_pago_id, tipo_tarjeta_id, monto_recibido, vuelto," +
                       "subtotal, igv, total, aud_id_usu_cre_n, aud_fec_cre_f, aud_id_usu_mod_n, aud_fec_mod_f," +
                       "aud_pc_ip_c, aud_pc_host_c, aud_es_eli_b)" +
                       "VALUES" +
                       "(@TipoDocumentoId, @FechaRegistro, @TipoPagoId, @Dni, @NombreCliente, @ApellidosCliente," +
                       "@Observacion, @TipoMetodoPagoId, @TipoTarjetaId, @MontoRecibido, @Vuelto," +
                       "@SubTotal, @Igv, @Total" +
                       "@aud_id_usu_cre_n, @aud_fec_cre_f, @aud_id_usu_mod_n, @aud_fec_mod_f, " +
                      "@aud_pc_ip_c, @aud_pc_host_c, 0)";
            using (SqlConnection sqlCn = new SqlConnection(this.cnBD))
            {
                value = value = sqlCn.Execute(sqlQuery, new
                {
                    tipo_documento_id = entidad.TipoDocumentoId,
                    fecha_registro = entidad.FechaRegistro,
                    tipo_pago_id =entidad.TipoPagoId,
                    dni= entidad.Dni,
                    nombre_cliente =entidad.NombreCliente,
                    apellidos_cliente = entidad.ApellidosCliente,
                    observacion = entidad.Observacion,
                    tipo_metodo_pago_id = entidad.TipoMetodoPagoId,
                    tipo_tarjeta_id = entidad.TipoTarjetaId,
                    monto_recibido = entidad.MontoRecibido,
                    vuelto = entidad.Vuelto,
                    subtotal = entidad.SubTotal,
                    igv =entidad.Igv,
                    total =entidad.Total,


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
        public int agregar(Entity.VentaBE entidad)
        {
            int ventaId = 0;

            using (SqlConnection sqlCn = new SqlConnection(this.cnBD))
            {
                sqlCn.Open();
                using (var tran = sqlCn.BeginTransaction())
                {
                    try
                    {
                        string sqlQuery = "INSERT INTO " + bdEsquema + bdTabla + " (tipo_documento_id, fecha_registro, tipo_pago_id, dni, nombre_cliente, apellidos_cliente,"+
                        "observacion, tipo_metodo_pago_id, tipo_tarjeta_id, monto_recibido, vuelto,"+
                        "subtotal, igv, total, aud_id_usu_cre_n, aud_fec_cre_f, aud_id_usu_mod_n, aud_fec_mod_f," +
                        "aud_pc_ip_c, aud_pc_host_c, aud_es_eli_b"+
                        ") VALUES ("+
                        "@TipoDocumentoId, @FechaRegistro, @TipoPagoId, @Dni, @NombreCliente, @ApellidosCliente,"+
                        "@Observacion, @TipoMetodoPagoId, @TipoTarjetaId, @MontoRecibido, @Vuelto,"+
                        "@SubTotal, @Igv, @Total, @aud_id_usu_cre_n, @aud_fec_cre_f, @aud_id_usu_mod_n, @aud_fec_mod_f,"+
                        "@aud_pc_ip_c, @aud_pc_host_c, 0"+
                        ")"+
                        "SELECT CAST(SCOPE_IDENTITY() as int);";

                        ventaId = sqlCn.QuerySingle<int>(sqlQuery, new
                        {
                            TipoDocumentoId = entidad.TipoDocumentoId,
                            FechaRegistro = entidad.FechaRegistro,
                            TipoPagoId = entidad.TipoPagoId,
                            Dni = entidad.Dni,
                            NombreCliente = entidad.NombreCliente,
                            ApellidosCliente = entidad.ApellidosCliente,
                            Observacion = entidad.Observacion,
                            TipoMetodoPagoId = entidad.TipoMetodoPagoId,
                            TipoTarjetaId = entidad.TipoTarjetaId,
                            MontoRecibido = entidad.MontoRecibido,
                            Vuelto = entidad.Vuelto,
                            SubTotal = entidad.SubTotal,
                            Igv = entidad.Igv,
                            Total = entidad.Total,

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
                            string sqlDetalle = @"
                        INSERT INTO detalle_venta
                        (venta_id, producto_id, cantidad, precio_unitario, total)
                        VALUES
                        (@VentaId, @ProductoId, @Cantidad, @PrecioUnitario, @Total);";

                            sqlCn.Execute(sqlDetalle, new
                            {
                                VentaId = ventaId,
                                det.ProductoId,
                                det.Cantidad,
                                det.PrecioUnitario,
                                det.Total
                            }, tran);

                            string sqlStock = "UPDATE producto SET stock = stock - @Cantidad WHERE producto_id = @ProductoId";
                            sqlCn.Execute(sqlStock, new { det.Cantidad, det.ProductoId }, tran);
                        }

                        tran.Commit();
                        return ventaId;
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
