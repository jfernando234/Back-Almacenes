using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Entity;

namespace Data
{
    public class ReportesDA
    {
        private readonly string _connectionString;

        public ReportesDA(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Obtiene datos para Valoración de Inventario
        /// </summary>
        public List<Dictionary<string, object>> ObtenerValoracionInventario()
        {
            var resultado = new List<Dictionary<string, object>>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string query = @"
                        SELECT 
                            pk_prod_id as ProductoId,
                            nombre_producto as NombreProducto,
                            stock as Stock,
                            precio_entrada as PrecioEntrada,
                            precio_salida as PrecioSalida,
                            (stock * precio_entrada) as ValorTotal
                        FROM producto_mae
                        WHERE aud_es_eli_b = 0 AND estado = 1
                        ORDER BY (stock * precio_entrada) DESC";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.CommandTimeout = 300;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                resultado.Add(new Dictionary<string, object>
                                {
                                    { "ProductoId", Convert.ToInt32(reader[0]) },
                                    { "NombreProducto", reader[1] == DBNull.Value ? "" : Convert.ToString(reader[1]) },
                                    { "Stock", Convert.ToInt32(reader[2]) },
                                    { "PrecioEntrada", Convert.ToDecimal(reader[3]) },
                                    { "PrecioSalida", Convert.ToDecimal(reader[4]) },
                                    { "ValorTotal", Convert.ToDecimal(reader[5]) }
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener valoración de inventario: {ex.Message}", ex);
            }

            return resultado;
        }

        /// <summary>
        /// Obtiene datos de Movimientos de Stock con filtros
        /// </summary>
        public List<Dictionary<string, object>> ObtenerMovimientosStock(DateTime? fechaInicio = null, DateTime? fechaFin = null, 
            int? productoId = null, string tipoMovimiento = null)
        {
            var resultado = new List<Dictionary<string, object>>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string query = @"
                        SELECT 
                            dc.detalle_venta_id as MovimientoId,
                            p.pk_prod_id as ProductoId,
                            p.nombre_producto as NombreProducto,
                            CASE WHEN c.pk_com_id IS NOT NULL THEN 'Compra' ELSE 'Venta' END as TipoMovimiento,
                            CAST(dc.cantidad as INT) as Cantidad,
                            dc.precio_unitario as PrecioUnitario,
                            dc.total as Total,
                            COALESCE(c.fecha_registro, GETDATE()) as FechaMovimiento,
                            p.stock as StockResultante
                        FROM detalle_compra dc
                        LEFT JOIN compra c ON dc.compra_id = c.pk_com_id
                        LEFT JOIN producto_mae p ON dc.producto_id = p.pk_prod_id
                        WHERE 1=1";

                    if (fechaInicio.HasValue && fechaFin.HasValue)
                    {
                        query += $" AND c.fecha_registro BETWEEN '{fechaInicio:yyyy-MM-dd}' AND '{fechaFin:yyyy-MM-dd}'";
                    }

                    if (productoId.HasValue)
                    {
                        query += $" AND dc.producto_id = {productoId}";
                    }

                    query += " ORDER BY FechaMovimiento DESC";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.CommandTimeout = 300;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                resultado.Add(new Dictionary<string, object>
                                {
                                    { "MovimientoId", Convert.ToInt32(reader[0]) },
                                    { "ProductoId", reader[1] == DBNull.Value ? 0 : Convert.ToInt32(reader[1]) },
                                    { "NombreProducto", reader[2] == DBNull.Value ? "" : Convert.ToString(reader[2]) },
                                    { "TipoMovimiento", reader[3] == DBNull.Value ? "" : Convert.ToString(reader[3]) },
                                    { "Cantidad", Convert.ToInt32(reader[4]) },
                                    { "PrecioUnitario", Convert.ToDecimal(reader[5]) },
                                    { "Total", Convert.ToDecimal(reader[6]) },
                                    { "FechaMovimiento", Convert.ToDateTime(reader[7]) },
                                    { "StockResultante", reader[8] == DBNull.Value ? 0 : Convert.ToInt32(reader[8]) }
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener movimientos de stock: {ex.Message}", ex);
            }

            return resultado;
        }

        /// <summary>
        /// Obtiene datos de Rendimiento de Proveedores
        /// </summary>
        public List<Dictionary<string, object>> ObtenerRendimientoProveedores()
        {
            var resultado = new List<Dictionary<string, object>>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string query = @"
                        SELECT 
                            pm.pk_prov_id as ProveedorId,
                            pm.nombre as NombreProveedor,
                            pm.ruc as RUC,
                            pm.contacto as Contacto,
                            COUNT(DISTINCT coc.idOrdenCompra) as TotalOrdenesCompra,
                            COALESCE(SUM(coc.total), 0) as MontoTotalComprado,
                            pm.telefono as Telefono,
                            pm.correo as Correo
                        FROM proveedor_mae pm
                        LEFT JOIN cl_ordenes_compra coc ON pm.pk_prov_id = coc.idProveedor
                        WHERE pm.aud_es_eli_b = 0 AND pm.estado = 1
                        GROUP BY pm.pk_prov_id, pm.nombre, pm.ruc, pm.contacto, pm.telefono, pm.correo
                        ORDER BY MontoTotalComprado DESC";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.CommandTimeout = 300;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                resultado.Add(new Dictionary<string, object>
                                {
                                    { "ProveedorId", Convert.ToInt32(reader[0]) },
                                    { "NombreProveedor", reader[1] == DBNull.Value ? "" : Convert.ToString(reader[1]) },
                                    { "RUC", reader[2] == DBNull.Value ? "" : Convert.ToString(reader[2]) },
                                    { "Contacto", reader[3] == DBNull.Value ? "" : Convert.ToString(reader[3]) },
                                    { "TotalOrdenesCompra", Convert.ToInt32(reader[4]) },
                                    { "MontoTotalComprado", Convert.ToDecimal(reader[5]) },
                                    { "Telefono", reader[6] == DBNull.Value ? "" : Convert.ToString(reader[6]) },
                                    { "Correo", reader[7] == DBNull.Value ? "" : Convert.ToString(reader[7]) }
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener rendimiento de proveedores: {ex.Message}", ex);
            }

            return resultado;
        }

        /// <summary>
        /// Obtiene datos de Historial de Clientes
        /// </summary>
        public List<Dictionary<string, object>> ObtenerHistorialClientes()
        {
            var resultado = new List<Dictionary<string, object>>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string query = @"
                        SELECT 
                            cm.pk_cli_id as ClienteId,
                            cm.razon_social as RazonSocial,
                            cm.numero_documento as NumeroDocumento,
                            cm.contacto as Contacto,
                            cm.correo as Correo,
                            cm.estado as EstadoCliente,
                            COUNT(DISTINCT coc.idOrdenCompra) as TotalOrdenes,
                            COALESCE(SUM(coc.total), 0) as MontoTotalComprado,
                            MAX(coc.fechaRegistro) as UltimaCompra
                        FROM cliente_mae cm
                        LEFT JOIN cl_ordenes_compra coc ON cm.pk_cli_id = coc.idProveedor
                        WHERE cm.aud_es_eli_b = 0 AND cm.estado = 1
                        GROUP BY cm.pk_cli_id, cm.razon_social, cm.numero_documento, cm.contacto, 
                                 cm.correo, cm.estado
                        ORDER BY MontoTotalComprado DESC";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.CommandTimeout = 300;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                resultado.Add(new Dictionary<string, object>
                                {
                                    { "ClienteId", Convert.ToInt32(reader[0]) },
                                    { "RazonSocial", reader[1] == DBNull.Value ? "" : Convert.ToString(reader[1]) },
                                    { "NumeroDocumento", reader[2] == DBNull.Value ? "" : Convert.ToString(reader[2]) },
                                    { "Contacto", reader[3] == DBNull.Value ? "" : Convert.ToString(reader[3]) },
                                    { "Correo", reader[4] == DBNull.Value ? "" : Convert.ToString(reader[4]) },
                                    { "EstadoCliente", Convert.ToInt32(reader[5]) },
                                    { "TotalOrdenes", Convert.ToInt32(reader[6]) },
                                    { "MontoTotalComprado", Convert.ToDecimal(reader[7]) },
                                    { "UltimaCompra", reader[8] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader[8]) }
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener historial de clientes: {ex.Message}", ex);
            }

            return resultado;
        }

        /// <summary>
        /// Obtiene datos de Stock Crítico
        /// </summary>
        public List<Dictionary<string, object>> ObtenerStockCritico(int limiteStock = 20)
        {
            var resultado = new List<Dictionary<string, object>>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string query = $@"
                        SELECT 
                            pk_prod_id as ProductoId,
                            nombre_producto as NombreProducto,
                            stock as StockActual,
                            {limiteStock} as StockMinimo,
                            (stock - {limiteStock}) as Diferencia,
                            CASE 
                                WHEN stock <= {limiteStock / 2} THEN 'Muy Crítico'
                                WHEN stock <= {limiteStock} THEN 'Crítico'
                                ELSE 'Alerta'
                            END as Urgencia
                        FROM producto_mae
                        WHERE stock <= {limiteStock} AND aud_es_eli_b = 0 AND estado = 1
                        ORDER BY stock ASC";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.CommandTimeout = 300;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                resultado.Add(new Dictionary<string, object>
                                {
                                    { "ProductoId", Convert.ToInt32(reader[0]) },
                                    { "NombreProducto", reader[1] == DBNull.Value ? "" : Convert.ToString(reader[1]) },
                                    { "StockActual", Convert.ToInt32(reader[2]) },
                                    { "StockMinimo", Convert.ToInt32(reader[3]) },
                                    { "Diferencia", Convert.ToInt32(reader[4]) },
                                    { "Urgencia", reader[5] == DBNull.Value ? "" : Convert.ToString(reader[5]) }
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener stock crítico: {ex.Message}", ex);
            }

            return resultado;
        }

        /// <summary>
        /// Obtiene datos para Análisis ABC de Productos
        /// </summary>
        public List<Dictionary<string, object>> ObtenerAnalisisABC()
        {
            var resultado = new List<Dictionary<string, object>>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string query = @"
                        WITH ProductoValor AS (
                            SELECT 
                                pk_prod_id as ProductoId,
                                nombre_producto as NombreProducto,
                                stock as Stock,
                                (stock * precio_entrada) as ValorTotal,
                                SUM(stock * precio_entrada) OVER () as ValorTotalInventario
                            FROM producto_mae
                            WHERE aud_es_eli_b = 0 AND estado = 1
                        ),
                        ProductoConPorcentaje AS (
                            SELECT 
                                *,
                                (ValorTotal / ValorTotalInventario * 100) as PorcentajeValor,
                                SUM(ValorTotal / ValorTotalInventario * 100) OVER (ORDER BY ValorTotal DESC) as PorcentajeAcumulado
                            FROM ProductoValor
                        )
                        SELECT 
                            ProductoId,
                            NombreProducto,
                            Stock,
                            ValorTotal,
                            ROUND(PorcentajeValor, 2) as PorcentajeValor,
                            CASE 
                                WHEN PorcentajeAcumulado <= 80 THEN 'A'
                                WHEN PorcentajeAcumulado <= 95 THEN 'B'
                                ELSE 'C'
                            END as Clasificacion
                        FROM ProductoConPorcentaje
                        ORDER BY ValorTotal DESC";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.CommandTimeout = 300;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                resultado.Add(new Dictionary<string, object>
                                {
                                    { "ProductoId", Convert.ToInt32(reader[0]) },
                                    { "NombreProducto", reader[1] == DBNull.Value ? "" : Convert.ToString(reader[1]) },
                                    { "Stock", Convert.ToInt32(reader[2]) },
                                    { "ValorTotal", Convert.ToDecimal(reader[3]) },
                                    { "PorcentajeValor", Convert.ToDecimal(reader[4]) },
                                    { "Clasificacion", reader[5] == DBNull.Value ? "" : Convert.ToString(reader[5]) }
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener análisis ABC: {ex.Message}", ex);
            }

            return resultado;
        }
    }
}
