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
    public class UsuarioPerfilDA
    {
        private readonly string cnBD = "";
        private string sqlQuery = "";
        private readonly string bdEsquema = "sch_seguridad.";
        private readonly string bdTabla = "usuario_perfil_mov";

        public UsuarioPerfilDA(string cnBD)
        {
            this.cnBD = cnBD;
            //FluentMapper.EntityMaps.Clear();
            //FluentMapper.Initialize(config => { config.AddMap(new UsuarioPerfilMAP()); });
        }

        public List<Entity.UsuarioPerfilBE> listar()
        {

            sqlQuery = "select *,p.nombre_c as perfil_nom from " + bdEsquema + bdTabla + " as up " +
                " inner join sch_seguridad.perfil_mae as p on up.fk_per_id=p.pk_per_id and p.aud_es_eli_b=0 " +
                " where up.aud_es_eli_b=0";

            using (SqlConnection sqlCn = new SqlConnection(this.cnBD))
            {
                var lista = sqlCn.Query<Entity.UsuarioPerfilBE>(sqlQuery).ToList();
                return lista;
            }

        }
        public Entity.UsuarioPerfilBE listar(int id)
        {

            sqlQuery = "select *,p.nombre_c as perfil_nom from " + bdEsquema + bdTabla + " as up" +
                        " inner join sch_seguridad.perfil_mae as p on up.fk_per_id=p.pk_per_id and p.aud_es_eli_b=0 " +
                        " where up.pk_usu_per_mod=@id";

            Entity.UsuarioPerfilBE value;

            using (SqlConnection sqlCn = new SqlConnection(this.cnBD))
            {
                try
                {
                    value = sqlCn.QuerySingle<Entity.UsuarioPerfilBE>(sqlQuery, new { id = id });
                }
                catch
                {
                    value = null;
                }

                return value;
            }

        }

        public int agregar(Entity.UsuarioPerfilBE entidad)
        {

            int value = 0;
            sqlQuery = "insert into " + bdEsquema + bdTabla +
                        "(fk_usu_id,fk_per_id," +
                            "aud_id_usu_cre_n,aud_fec_cre_f,aud_id_usu_mod_n,aud_fec_mod_f,aud_pc_ip_c,aud_pc_host_c,aud_es_eli_b) " +
                        " values (@fk_usu_id,@fk_per_id," +
                            "@aud_id_usu_cre_n,@aud_fec_cre_f,@aud_id_usu_mod_n,@aud_fec_mod_f,@aud_pc_ip_c,@aud_pc_host_c,0)";

            using (SqlConnection sqlCn = new SqlConnection(this.cnBD))
            {
                value = sqlCn.Execute(sqlQuery, new
                {
                    fk_usu_id = entidad.idUsuario,
                    fk_per_id = entidad.idPerfil,                    
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

        public int modificar(Entity.UsuarioPerfilBE entidad)
        {

            int value = 0;
            sqlQuery = "update " + bdEsquema + bdTabla + " set " +
                        "fk_usu_id=@fk_usu_id," +
                        "fk_per_id=@fk_per_id," +                        
                        "aud_id_usu_mod_n=@aud_id_usu_mod_n," +
                        "aud_fec_mod_f=@aud_fec_mod_f," +
                        "aud_pc_ip_c=@aud_pc_ip_c," +
                        "aud_pc_host_c=@aud_pc_host_c" +
                        " where " +
                        "pk_usu_per_mod=@id";

            using (SqlConnection sqlCn = new SqlConnection(this.cnBD))
            {
                value = sqlCn.Execute(sqlQuery, new
                {
                    id=entidad.idUsuarioPerfil,
                    fk_usu_id = entidad.idUsuario,
                    fk_per_id = entidad.idPerfil,
                    /*auditoria*/
                    aud_fec_cre_f = DateTime.Now,
                    aud_id_usu_mod_n = entidad.idUsuarioLogin,
                    aud_fec_mod_f = DateTime.Now,
                    aud_pc_ip_c = entidad.pcIp,
                    aud_pc_host_c = entidad.pcHost,
                });
                return value;
            }
        }

        public int eliminar(Entity.UsuarioPerfilBE entidad)
        {

            int value = 0;
            sqlQuery = "update " + bdEsquema + bdTabla + " set " +
                       "aud_id_usu_eli_n=@aud_id_usu_eli_n," +
                        "aud_fec_eli_f=@aud_fec_eli_f," +
                        "aud_pc_ip_c=@aud_pc_ip_c," +
                        "aud_pc_host_c=@aud_pc_host_c," +
                        "aud_es_eli_b=1" +
                        " where " +
                        "pk_usu_per_mod=@id";


            using (SqlConnection sqlCn = new SqlConnection(this.cnBD))
            {
                value = sqlCn.Execute(sqlQuery, new
                {
                    id = entidad.idUsuarioPerfil,
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
