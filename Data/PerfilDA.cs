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
    public  class PerfilDA
    {
        private readonly string cnBD = "";
        private string sqlQuery = "";
        private readonly string bdEsquema = "sch_seguridad.";
        private readonly string bdTabla = "perfil_mae";

        public PerfilDA(string cnBD)
        {
            this.cnBD = cnBD;
            //FluentMapper.EntityMaps.Clear();
            //FluentMapper.Initialize(config => { config.AddMap(new PerfilMAP()); });
        }

        public List<Entity.PerfilBE> listar()
        {
           
            sqlQuery = "select * from " + bdEsquema + bdTabla + " where aud_es_eli_b=0";

            using (SqlConnection sqlCn = new SqlConnection(this.cnBD))
            {
                var lista = sqlCn.Query<Entity.PerfilBE>(sqlQuery).ToList();
                return lista;
            }

        }
        public Entity.PerfilBE listar(int id )
        {            

            sqlQuery = "select * from " + bdEsquema + bdTabla + " where pk_per_id=@id";
            Entity.PerfilBE value;

            using (SqlConnection sqlCn = new SqlConnection(this.cnBD))
            {
                try
                {
                    value = sqlCn.QuerySingle<Entity.PerfilBE>(sqlQuery, new { id = id });
                }
                catch 
                {
                    value = null;
                }
                                
                return value;
            }

        }

        public int agregar(Entity.PerfilBE entidad)
        {
           
            int value = 0;

            string sqlQueryIdentity = "select SCOPE_IDENTITY()";
            
            sqlQuery = "insert into " + bdEsquema + bdTabla + 
                        "(nombre_c,descrip_c," +
                            "aud_id_usu_cre_n,aud_fec_cre_f,aud_id_usu_mod_n,aud_fec_mod_f,aud_pc_ip_c,aud_pc_host_c,aud_es_eli_b) " +
                        " values (@nombre_c,@descrip_c," +
                            "@aud_id_usu_cre_n,@aud_fec_cre_f,@aud_id_usu_mod_n,@aud_fec_mod_f,@aud_pc_ip_c,@aud_pc_host_c,0)";

            sqlQuery = sqlQuery + ";" + sqlQueryIdentity;

            using (SqlConnection sqlCn = new SqlConnection(this.cnBD))
            {
                value = sqlCn.ExecuteScalar<int>(sqlQuery, new
                {
                    nombre_c = entidad.nombrePerfil,
                    descrip_c = entidad.descripcionPerfil,
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

        public int modificar(Entity.PerfilBE entidad)
        {

            int value = 0;
            sqlQuery = "update " + bdEsquema + bdTabla + " set " +
                        "nombre_c=@nombre_c," +
                        "descrip_c=@descrip_c," +
                        "aud_id_usu_mod_n=@aud_id_usu_mod_n," +
                        "aud_fec_mod_f=@aud_fec_mod_f," +
                        "aud_pc_ip_c=@aud_pc_ip_c," +
                        "aud_pc_host_c=@aud_pc_host_c" +
                        " where " +
                        "pk_per_id=@id";            

            using (SqlConnection sqlCn = new SqlConnection(this.cnBD))
            {
                value = sqlCn.Execute(sqlQuery, new
                {
                    id=entidad.idPerfil,
                    nombre_c = entidad.nombrePerfil,
                    descrip_c = entidad.descripcionPerfil,
                    /*auditoria*/
                    aud_id_usu_mod_n = entidad.idUsuarioLogin,
                    aud_fec_mod_f = DateTime.Now,
                    aud_pc_ip_c = entidad.pcIp,
                    aud_pc_host_c = entidad.pcHost,                    
                });
                return value;
            }
        }

        public int eliminar(Entity.PerfilBE entidad)
        {

            int value = 0;
            sqlQuery = "update " + bdEsquema + bdTabla + " set " +
                        "aud_id_usu_eli_n=@aud_id_usu_eli_n," +
                        "aud_fec_eli_f=@aud_fec_eli_f," +
                        "aud_pc_ip_c=@aud_pc_ip_c," +
                        "aud_pc_host_c=@aud_pc_host_c," +
                        "aud_es_eli_b=1" +
                        " where " +
                        "pk_per_id=@id";

            using (SqlConnection sqlCn = new SqlConnection(this.cnBD))
            {
                value = sqlCn.Execute(sqlQuery, new
                {
                    id = entidad.idPerfil,
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
