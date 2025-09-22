using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Data
{
    public class ProveedorDA
    {
        private readonly string cnBD = "";
        private string sqlQuery = "";
        private readonly string bdEsquema = "dbo.";
        private readonly string bdTabla = "proveedor_mae";

        public ProveedorDA(string cnBD)
        {
            this.cnBD = cnBD;
        }

        public List<Entity.ProveedorBE> listarAll()
        {
            sqlQuery = "SELECT * FROM " + bdEsquema + bdTabla + " " +
                      "WHERE aud_es_eli_b = 0 " +
                      "ORDER BY nombre";

            using (SqlConnection sqlCn = new SqlConnection(this.cnBD))
            {
                try
                {
                    sqlCn.Open();
                    var lista = sqlCn.Query<Entity.ProveedorBE>(sqlQuery).ToList();
                    return lista;
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine($"Error en ProveedorDA.listarAll(): {ex.Message}");
                    System.Console.WriteLine($"Query: {sqlQuery}");
                    System.Console.WriteLine($"Connection String: {this.cnBD}");
                    throw;
                }
            }
        }

        public Entity.ProveedorBE listar(int id)
        {
            sqlQuery = "SELECT * FROM " + bdEsquema + bdTabla + " WHERE pk_prov_id = @id AND aud_es_eli_b = 0";

            Entity.ProveedorBE value;

            using (SqlConnection sqlCn = new SqlConnection(this.cnBD))
            {
                try
                {
                    value = sqlCn.QuerySingle<Entity.ProveedorBE>(sqlQuery, new { id = id });
                }
                catch
                {
                    value = null;
                }

                return value;
            }
        }

        public int agregar(Entity.ProveedorBE entidad)
        {
            int value = 0;
            sqlQuery = "INSERT INTO " + bdEsquema + bdTabla +
                      "(ruc, nombre, direccion, telefono, correo, contacto, estado, " +
                      "aud_id_usu_cre_n, aud_fec_cre_f, aud_id_usu_mod_n, aud_fec_mod_f, " +
                      "aud_pc_ip_c, aud_pc_host_c, aud_es_eli_b) " +
                      "VALUES (@ruc, @nombre, @direccion, @telefono, @correo, @contacto, @estado, " +
                      "@aud_id_usu_cre_n, @aud_fec_cre_f, @aud_id_usu_mod_n, @aud_fec_mod_f, " +
                      "@aud_pc_ip_c, @aud_pc_host_c, 0)";

            using (SqlConnection sqlCn = new SqlConnection(this.cnBD))
            {
                value = sqlCn.Execute(sqlQuery, new
                {
                    ruc = entidad.ruc,
                    nombre = entidad.nombre,
                    direccion = entidad.direccion,
                    telefono = entidad.telefono,
                    correo = entidad.correo,
                    contacto = entidad.contacto,
                    estado = entidad.estado,
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

        public int modificar(Entity.ProveedorBE entidad)
        {
            int value = 0;
            sqlQuery = "UPDATE " + bdEsquema + bdTabla + " SET " +
                      "ruc = @ruc, " +
                      "nombre = @nombre, " +
                      "direccion = @direccion, " +
                      "telefono = @telefono, " +
                      "correo = @correo, " +
                      "contacto = @contacto, " +
                      "estado = @estado, " +
                      "aud_id_usu_mod_n = @aud_id_usu_mod_n, " +
                      "aud_fec_mod_f = @aud_fec_mod_f, " +
                      "aud_pc_ip_c = @aud_pc_ip_c, " +
                      "aud_pc_host_c = @aud_pc_host_c " +
                      "WHERE pk_prov_id = @id AND aud_es_eli_b = 0";

            using (SqlConnection sqlCn = new SqlConnection(this.cnBD))
            {
                value = sqlCn.Execute(sqlQuery, new
                {
                    id = entidad.idProveedor,
                    ruc = entidad.ruc,
                    nombre = entidad.nombre,
                    direccion = entidad.direccion,
                    telefono = entidad.telefono,
                    correo = entidad.correo,
                    contacto = entidad.contacto,
                    estado = entidad.estado,
                    aud_id_usu_mod_n = entidad.idUsuarioLogin,
                    aud_fec_mod_f = DateTime.Now,
                    aud_pc_ip_c = entidad.pcIp,
                    aud_pc_host_c = entidad.pcHost
                });
                return value;
            }
        }

        public int deshabilitar(Entity.ProveedorBE entidad)
        {
            int value = 0;
            sqlQuery = "UPDATE " + bdEsquema + bdTabla + " SET " +
                      "aud_id_usu_eli_n = @aud_id_usu_eli_n, " +
                      "aud_fec_eli_f = @aud_fec_eli_f, " +
                      "aud_pc_ip_c = @aud_pc_ip_c, " +
                      "aud_pc_host_c = @aud_pc_host_c, " +
                      "aud_es_eli_b = 1 " +
                      "WHERE pk_prov_id = @id";

            using (SqlConnection sqlCn = new SqlConnection(this.cnBD))
            {
                value = sqlCn.Execute(sqlQuery, new
                {
                    id = entidad.idProveedor,
                    aud_id_usu_eli_n = entidad.idUsuarioLogin,
                    aud_fec_eli_f = DateTime.Now,
                    aud_pc_ip_c = entidad.pcIp,
                    aud_pc_host_c = entidad.pcHost
                });
                return value;
            }
        }

        public List<Entity.ProveedorBE> filtro(DateTime? inicio, DateTime? fin, string nombre)
        {
            var whereClauses = new List<string>();
            var parameters = new DynamicParameters();

            whereClauses.Add("aud_es_eli_b = 0");

            if (inicio.HasValue && fin.HasValue)
            {
                whereClauses.Add("aud_fec_cre_f BETWEEN @inicio AND @fin");
                parameters.Add("inicio", inicio.Value);
                parameters.Add("fin", fin.Value);
            }

            if (!string.IsNullOrWhiteSpace(nombre))
            {
                whereClauses.Add("nombre LIKE @nombre");
                parameters.Add("nombre", "%" + nombre + "%");
            }

            sqlQuery = "SELECT * FROM " + bdEsquema + bdTabla + " WHERE " + string.Join(" AND ", whereClauses) + " ORDER BY nombre";

            using (SqlConnection sqlCn = new SqlConnection(this.cnBD))
            {
                try
                {
                    sqlCn.Open();
                    var lista = sqlCn.Query<Entity.ProveedorBE>(sqlQuery, parameters).ToList();
                    return lista;
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine($"Error en ProveedorDA.filtro(): {ex.Message}");
                    System.Console.WriteLine($"Query: {sqlQuery}");
                    System.Console.WriteLine($"Connection String: {this.cnBD}");
                    throw;
                }
            }
        }
    }
}