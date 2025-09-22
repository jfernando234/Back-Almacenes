using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Data
{
    public class ClienteDA
    {
        private readonly string cnBD = "";
        private string sqlQuery = "";
        private readonly string bdEsquema = "dbo.";
        private readonly string bdTabla = "cliente_mae";

        public ClienteDA(string cnBD)
        {
            this.cnBD = cnBD;
        }

    public List<Entity.ClienteBE> listarAll()
        {

            sqlQuery = "SELECT * FROM " + bdEsquema + bdTabla + " " +
                      "WHERE aud_es_eli_b = 0 " +
                      "ORDER BY razon_social";

            using (SqlConnection sqlCn = new SqlConnection(this.cnBD))
            {
                try
                {
                    sqlCn.Open();
                    var lista = sqlCn.Query<Entity.ClienteBE>(sqlQuery).ToList();
                    // No rellenar estadoTexto aquí: se eliminó del DTO de listado para evitar exponer texto derivado            
                    return lista;
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine($"Error en ClienteDA.listarAll(): {ex.Message}");
                    System.Console.WriteLine($"Query: {sqlQuery}");
                    System.Console.WriteLine($"Connection String: {this.cnBD}");
                    throw;
                }
            }
        }

        public Entity.ClienteBE listar(int id)
        {
            sqlQuery = "SELECT c.*, " +
                      "t.nom_c as tipoDocumentoNombre, " +
                      "t.abrev_c as tipoDocumentoAbrev, " +
                      "CASE WHEN c.estado = 1 THEN 'Activo' ELSE 'Inactivo' END as estadoTexto " +
                      "FROM " + bdEsquema + bdTabla + " c " +
                      "LEFT JOIN tipodocumento_mae t ON c.fk_tip_doc_id = t.pk_tip_doc_id " +
                      "WHERE c.pk_cli_id = @id AND c.aud_es_eli_b = 0";

            Entity.ClienteBE value = null;

            using (SqlConnection sqlCn = new SqlConnection(this.cnBD))
            {
                try
                {
                    value = sqlCn.QuerySingleOrDefault<Entity.ClienteBE>(sqlQuery, new { id = id });
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine($"Error en ClienteDA.listar(): {ex.Message}");
                    System.Console.WriteLine($"Query: {sqlQuery}");
                    value = null;
                }

                return value;
            }
        }

        public int agregar(Entity.ClienteBE entidad)
        {
            int value = 0;
            sqlQuery = "INSERT INTO " + bdEsquema + bdTabla +
                      "(fk_tip_doc_id, numero_documento, razon_social, direccion, " +
                      "telefono, correo, contacto, estado, " +
                      "aud_id_usu_cre_n, aud_fec_cre_f, aud_id_usu_mod_n, aud_fec_mod_f, " +
                      "aud_pc_ip_c, aud_pc_host_c, aud_es_eli_b) " +
                      "VALUES (@fk_tip_doc_id, @numero_documento, @razon_social, @direccion, " +
                      "@telefono, @correo, @contacto, @estado, " +
                      "@aud_id_usu_cre_n, @aud_fec_cre_f, @aud_id_usu_mod_n, @aud_fec_mod_f, " +
                      "@aud_pc_ip_c, @aud_pc_host_c, 0)";

            using (SqlConnection sqlCn = new SqlConnection(this.cnBD))
            {
                value = sqlCn.Execute(sqlQuery, new
                {
                    fk_tip_doc_id = entidad.idTipoDocumento,
                    numero_documento = entidad.numeroDocumento,
                    razon_social = entidad.razonSocial,
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

        public int modificar(Entity.ClienteBE entidad)
        {
            int value = 0;
            sqlQuery = "UPDATE " + bdEsquema + bdTabla + " SET " +
                      "fk_tip_doc_id = @fk_tip_doc_id, " +
                      "numero_documento = @numero_documento, " +
                      "razon_social = @razon_social, " +
                      "direccion = @direccion, " +
                      "telefono = @telefono, " +
                      "correo = @correo, " +
                      "contacto = @contacto, " +
                      "estado = @estado, " +
                      "aud_id_usu_mod_n = @aud_id_usu_mod_n, " +
                      "aud_fec_mod_f = @aud_fec_mod_f, " +
                      "aud_pc_ip_c = @aud_pc_ip_c, " +
                      "aud_pc_host_c = @aud_pc_host_c " +
                      "WHERE pk_cli_id = @id AND aud_es_eli_b = 0";

            using (SqlConnection sqlCn = new SqlConnection(this.cnBD))
            {
                value = sqlCn.Execute(sqlQuery, new
                {
                    id = entidad.idCliente,
                    fk_tip_doc_id = entidad.idTipoDocumento,
                    numero_documento = entidad.numeroDocumento,
                    razon_social = entidad.razonSocial,
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

        public int deshabilitar(Entity.ClienteBE entidad)
        {
            int value = 0;
            sqlQuery = "UPDATE " + bdEsquema + bdTabla + " SET " +
                      "aud_id_usu_eli_n = @aud_id_usu_eli_n, " +
                      "aud_fec_eli_f = @aud_fec_eli_f, " +
                      "aud_pc_ip_c = @aud_pc_ip_c, " +
                      "aud_pc_host_c = @aud_pc_host_c, " +
                      "aud_es_eli_b = 1 " +
                      "WHERE pk_cli_id = @id";

            using (SqlConnection sqlCn = new SqlConnection(this.cnBD))
            {
                value = sqlCn.Execute(sqlQuery, new
                {
                    id = entidad.idCliente,
                    // Auditoría
                    aud_id_usu_eli_n = entidad.idUsuarioLogin,
                    aud_fec_eli_f = DateTime.Now,
                    aud_pc_ip_c = entidad.pcIp,
                    aud_pc_host_c = entidad.pcHost
                });
                return value;
            }
        }

        public bool existeDocumento(int tipoDocumento, string numeroDocumento, int? idClienteExcluir = null)
        {
            sqlQuery = "SELECT COUNT(*) FROM " + bdEsquema + bdTabla + " " +
                      "WHERE fk_tip_doc_id = @tipoDocumento AND numero_documento = @numeroDocumento " +
                      "AND aud_es_eli_b = 0";

            if (idClienteExcluir.HasValue)
            {
                sqlQuery += " AND pk_cli_id != @idClienteExcluir";
            }

            using (SqlConnection sqlCn = new SqlConnection(this.cnBD))
            {
                var count = sqlCn.QuerySingle<int>(sqlQuery, new 
                { 
                    tipoDocumento = tipoDocumento, 
                    numeroDocumento = numeroDocumento,
                    idClienteExcluir = idClienteExcluir
                });
                return count > 0;
            }
        }

        public bool existeClientePorId(int idCliente)
        {
            sqlQuery = "SELECT COUNT(*) FROM " + bdEsquema + bdTabla + " WHERE pk_cli_id = @id AND aud_es_eli_b = 0";
            using (SqlConnection sqlCn = new SqlConnection(this.cnBD))
            {
                var count = sqlCn.QuerySingle<int>(sqlQuery, new { id = idCliente });
                return count > 0;
            }
        }

        public List<Entity.ClienteBE> filtro(DateTime? inicio, DateTime? fin, string nombre)
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
                whereClauses.Add("contacto LIKE @nombre");
                parameters.Add("nombre", "%" + nombre + "%");
            }

            sqlQuery = "SELECT * FROM " + bdEsquema + bdTabla + " WHERE " + string.Join(" AND ", whereClauses) + " ORDER BY razon_social";

            using (SqlConnection sqlCn = new SqlConnection(this.cnBD))
            {
                try
                {
                    sqlCn.Open();
                    var lista = sqlCn.Query<Entity.ClienteBE>(sqlQuery, parameters).ToList();
                    return lista;
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine($"Error en ClienteDA.filtro(): {ex.Message}");
                    System.Console.WriteLine($"Query: {sqlQuery}");
                    System.Console.WriteLine($"Connection String: {this.cnBD}");
                    throw;
                }
            }
        }
    }
}