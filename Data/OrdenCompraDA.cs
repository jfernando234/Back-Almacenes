using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Entity;

namespace Data
{
    public class OrdenCompraDA
    {
        private readonly string cnBD;

        public OrdenCompraDA(string connectionString)
        {
            cnBD = connectionString;
        }

        public List<OrdenCompraBE> listar()
        {
            using (IDbConnection db = new SqlConnection(cnBD))
            {
                string query = @"
                    SELECT idOrdenCompra, fechaRegistro, idTipoDocumento, numeroDocumento, 
                           idProveedor, idTipoCompra, efectivo, fechaVencimiento, 
                           observacion, total, estado
                    FROM cl_ordenes_compra 
                    WHERE estado = 1
                    ORDER BY fechaRegistro DESC";

                return db.Query<OrdenCompraBE>(query).AsList();
            }
        }

        public OrdenCompraBE listar(int id)
        {
            using (IDbConnection db = new SqlConnection(cnBD))
            {
                string query = @"
                    SELECT idOrdenCompra, fechaRegistro, idTipoDocumento, numeroDocumento, 
                           idProveedor, idTipoCompra, efectivo, fechaVencimiento, 
                           observacion, total, estado
                    FROM cl_ordenes_compra 
                    WHERE idOrdenCompra = @Id";

                return db.QueryFirstOrDefault<OrdenCompraBE>(query, new { Id = id });
            }
        }

        public List<OrdenCompraBE> filtrar(DateTime inicio, DateTime fin)
        {
            using (IDbConnection db = new SqlConnection(cnBD))
            {
                string query = @"
                    SELECT idOrdenCompra, fechaRegistro, idTipoDocumento, numeroDocumento, 
                           idProveedor, idTipoCompra, efectivo, fechaVencimiento, 
                           observacion, total, estado
                    FROM cl_ordenes_compra 
                    WHERE fechaRegistro BETWEEN @Inicio AND @Fin
                    AND estado = 1
                    ORDER BY fechaRegistro DESC";

                return db.Query<OrdenCompraBE>(query, new { Inicio = inicio, Fin = fin }).AsList();
            }
        }

        public int agregar(OrdenCompraBE ordenCompra)
        {
            using (IDbConnection db = new SqlConnection(cnBD))
            {
                string query = @"
                    INSERT INTO cl_ordenes_compra 
                    (fechaRegistro, idTipoDocumento, numeroDocumento, idProveedor, idTipoCompra, 
                     efectivo, fechaVencimiento, observacion, total, estado, 
                     aud_pc_ip_c, aud_pc_host_c, aud_id_usu_cre_n, aud_fec_cre_f, aud_es_eli_b)
                    VALUES 
                    (@fechaRegistro, @idTipoDocumento, @numeroDocumento, @idProveedor, @idTipoCompra, 
                     @efectivo, @fechaVencimiento, @observacion, @total, @estado, 
                     @pcIp, @pcHost, @idUsuarioLogin, GETDATE(), 0);
                    SELECT CAST(SCOPE_IDENTITY() as int)";

                return db.QuerySingle<int>(query, ordenCompra);
            }
        }

        public int modificar(OrdenCompraBE ordenCompra)
        {
            using (IDbConnection db = new SqlConnection(cnBD))
            {
                string query = @"
                    UPDATE cl_ordenes_compra 
                    SET fechaRegistro = @fechaRegistro,
                        idTipoDocumento = @idTipoDocumento,
                        numeroDocumento = @numeroDocumento,
                        idProveedor = @idProveedor,
                        idTipoCompra = @idTipoCompra,
                        efectivo = @efectivo,
                        fechaVencimiento = @fechaVencimiento,
                        observacion = @observacion,
                        total = @total,
                        estado = @estado,
                        aud_pc_ip_c = @pcIp,
                        aud_pc_host_c = @pcHost,
                        aud_id_usu_mod_n = @idUsuarioLogin,
                        aud_fec_mod_f = GETDATE()
                    WHERE idOrdenCompra = @idOrdenCompra";

                return db.Execute(query, ordenCompra);
            }
        }

        public int deshabilitar(int id, string pcIp, string pcHost, int idUsuarioLogin)
        {
            using (IDbConnection db = new SqlConnection(cnBD))
            {
                string query = @"
                    UPDATE cl_ordenes_compra 
                    SET estado = 0,
                        aud_pc_ip_c = @pcIp,
                        aud_pc_host_c = @pcHost,
                        aud_id_usu_eli_n = @idUsuarioLogin,
                        aud_fec_eli_f = GETDATE(),
                        aud_es_eli_b = 1
                    WHERE idOrdenCompra = @Id";

                return db.Execute(query, new { Id = id, pcIp, pcHost, idUsuarioLogin });
            }
        }
    }
}
