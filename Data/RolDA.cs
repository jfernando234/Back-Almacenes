using Dapper;
using Dapper.FluentMap;
using Entity.Map;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public  class RolDA
    {
        private readonly string cnBD = "";
        private string sqlQuery = "";
        private readonly string bdEsquema = "sch_seguridad.";
        private readonly string bdTabla = "rol_mov";

        public RolDA(string cnBD)
        {
            this.cnBD = cnBD;
            //FluentMapper.EntityMaps.Clear();
            //FluentMapper.Initialize(config => { config.AddMap(new RolMAP()); });
        }

        public List<Entity.RolBE> listar()
        {

            sqlQuery = "select * from " + bdEsquema + bdTabla + " where aud_es_eli_b=0";

            using (SqlConnection sqlCn = new SqlConnection(this.cnBD))
            {
                var lista = sqlCn.Query<Entity.RolBE>(sqlQuery).ToList();
                return lista;
            }
        }
        public Entity.RolBE listar(int id)
        {

            sqlQuery = "select * from " + bdEsquema + bdTabla + " where pk_rol_id=@id";
            Entity.RolBE value;

            using (SqlConnection sqlCn = new SqlConnection(this.cnBD))
            {
                try
                {
                    value = sqlCn.QuerySingle<Entity.RolBE>(sqlQuery, new { id = id });
                }
                catch
                {
                    value = null;
                }

                return value;
            }
        }

        public int agregar(Entity.RolBE entidad)
        {

            int value = 0;
            sqlQuery = "insert into " + bdEsquema + bdTabla +
                        "(fk_mod_id,nombre_c,descrip_c,val_numerico_n,val_cadena_c,val_logico_b," +
                            "aud_id_usu_cre_n,aud_fec_cre_f,aud_id_usu_mod_n,aud_fec_mod_f,aud_pc_ip_c,aud_pc_host_c,aud_es_eli_b) " +
                        " values (@fk_mod_id,@nombre_c,@descrip_c,@val_numerico_n,@val_cadena_c,@val_logico_b," +
                            "@aud_id_usu_cre_n,@aud_fec_cre_f,@aud_id_usu_mod_n,@aud_fec_mod_f,@aud_pc_ip_c,@aud_pc_host_c,0)";

            using (SqlConnection sqlCn = new SqlConnection(this.cnBD))
            {
                value = sqlCn.Execute(sqlQuery, new
                {
                    fk_mod_id=entidad.idModulo,
                    nombre_c = entidad.nombreRol,
                    descrip_c = entidad.descripcionRol,
                    val_numerico_n=entidad.valorNumerico,
                    val_cadena_c=entidad.valorCadena,
                    val_logico_b=entidad.valorLogico,
                    /*auditoria*/
                    aud_id_usu_cre_n = entidad.idUsuarioLogin,
                    aud_fec_cre_f = DateTime.Now,
                    aud_id_usu_mod_n = entidad.idUsuarioLogin,
                    aud_fec_mod_f = DateTime.Now,
                    aud_pc_ip_c = entidad.pcIp,
                    aud_pc_host_c = entidad.pcHost,
                });
                return value;
            }
        }

        public int modificar(Entity.RolBE entidad)
        {

            int value = 0;
            sqlQuery = "update " + bdEsquema + bdTabla + " set " +
                        "fk_mod_id=@fk_mod_id," +
                        "nombre_c=@nombre_c," +
                        "descrip_c=@descrip_c," +
                        "val_numerico_n=@val_numerico_n," +
                        "val_cadena_c=@val_cadena_c," +
                        "val_logico_b=@val_logico_b," +                        
                        "aud_id_usu_mod_n=@aud_id_usu_mod_n," +
                        "aud_fec_mod_f=@aud_fec_mod_f," +
                        "aud_pc_ip_c=@aud_pc_ip_c," +
                        "aud_pc_host_c=@aud_pc_host_c" +
                        " where " +
                        "pk_rol_id=@id";

            using (SqlConnection sqlCn = new SqlConnection(this.cnBD))
            {
                value = sqlCn.Execute(sqlQuery, new
                {
                    id = entidad.idRol,
                    fk_mod_id = entidad.idModulo,
                    nombre_c = entidad.nombreRol,
                    descrip_c = entidad.descripcionRol,
                    val_numerico_n = entidad.valorNumerico,
                    val_cadena_c = entidad.valorCadena,
                    val_logico_b = entidad.valorLogico,
                    /*auditoria*/
                    aud_id_usu_mod_n = entidad.idUsuarioLogin,
                    aud_fec_mod_f = DateTime.Now,
                    aud_pc_ip_c = entidad.pcIp,
                    aud_pc_host_c = entidad.pcHost,
                });
                return value;
            }
        }

        public int eliminar(Entity.RolBE entidad)
        {

            int value = 0;
            sqlQuery = "update " + bdEsquema + bdTabla + " set " +
                        "aud_id_usu_eli_n=@aud_id_usu_eli_n," +
                        "aud_fec_eli_f=@aud_fec_eli_f," +
                        "aud_pc_ip_c=@aud_pc_ip_c," +
                        "aud_pc_host_c=@aud_pc_host_c," +
                        "aud_es_eli_b=1" +
                        " where " +
                        "pk_rol_id=@id";

            using (SqlConnection sqlCn = new SqlConnection(this.cnBD))
            {
                value = sqlCn.Execute(sqlQuery, new
                {
                    id = entidad.idRol,
                    /*auditoria*/
                    aud_id_usu_eli_n = entidad.idUsuarioLogin,
                    aud_fec_eli_f = DateTime.Now,
                    aud_pc_ip_c = entidad.pcIp,
                    aud_pc_host_c = entidad.pcHost,
                });
                return value;
            }
        }

    }
}
