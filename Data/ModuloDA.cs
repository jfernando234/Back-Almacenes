using Dapper;
using Dapper.FluentMap;
using Entity.Map;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class ModuloDA
    {
        private readonly string cnBD = "";
        private string sqlQuery = "";
        private readonly string bdEsquema  = "sch_seguridad.";
        private readonly string bdTabla = "modulo_mae";        
        private readonly string bdProcedure = "usp_modulo_mae_list";

        public ModuloDA(string cnBD)
        {
            this.cnBD = cnBD;
            //FluentMapper.EntityMaps.Clear();
            //FluentMapper.Initialize(config => { config.AddMap(new ModuloMAP()); });
        }

        public List<Entity.ModuloBE> listar()
        {

            sqlQuery = bdEsquema + bdProcedure;

            var p = new DynamicParameters();
            p.Add("p_accion_c", "lst");
            p.Add("p_opcion_c", "lst_grilla_recursivo");            

            using (SqlConnection sqlCn = new SqlConnection(this.cnBD))
            {
                var lista = sqlCn.Query<Entity.ModuloBE>(sqlQuery, p, commandType: CommandType.StoredProcedure).ToList();
                return lista;
            }
        }        

        public int agregar(Entity.ModuloBE entidad)
        {

            int value = 0;
            sqlQuery = "insert into " + bdEsquema + bdTabla +
                        "(fk_mod_id,nivel_n,es_mod_hij_b,nombre_c,nombre_obj_c,codigo_acc_dir_c,icono_c,url_c,num_ord,es_visible_b," +
                            "aud_id_usu_cre_n,aud_fec_cre_f,aud_id_usu_mod_n,aud_fec_mod_f,aud_pc_ip_c,aud_pc_host_c,aud_es_eli_b) " +
                        " values (@fk_mod_id,@nivel_n,@es_mod_hij_b,@nombre_c,@nombre_obj_c,@codigo_acc_dir_c,@icono_c,@url_c,@num_ord,@es_visible_b," +
                            "@aud_id_usu_cre_n,@aud_fec_cre_f,@aud_id_usu_mod_n,@aud_fec_mod_f,@aud_pc_ip_c,@aud_pc_host_c,0)";

            using (SqlConnection sqlCn = new SqlConnection(this.cnBD))
            {
                value = sqlCn.Execute(sqlQuery, new
                {
                    fk_mod_id = entidad.idModuloPadre,
                    nivel_n = entidad.nivel,
                    es_mod_hij_b = entidad.esHijo,
                    nombre_c=entidad.nombreModulo,
                    nombre_obj_c=entidad.nombreObjeto,
                    codigo_acc_dir_c=entidad.codigoAccesoDirecto,
                    icono_c=entidad.icono,
                    url_c=entidad.url,
                    num_ord=entidad.numeroOrden,
                    es_visible_b=entidad.esVisible,
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

        public int modificar(Entity.ModuloBE entidad)
        {

            int value = 0;         
            sqlQuery = "update " + bdEsquema + bdTabla + " set " +
                        "fk_mod_id=@fk_mod_id," +
                        "nivel_n=@nivel_n," +
                        "es_mod_hij_b=@es_mod_hij_b," +
                        "nombre_c=@nombre_c," +
                        "nombre_obj_c=@nombre_obj_c," +
                        "codigo_acc_dir_c=@codigo_acc_dir_c," +
                        "icono_c=@icono_c," +
                        "url_c=@url_c," +
                        "num_ord=@num_ord," +
                        "es_visible_b=@es_visible_b," +
                        "aud_id_usu_mod_n=@aud_id_usu_mod_n," +
                        "aud_fec_mod_f=@aud_fec_mod_f," +
                        "aud_pc_ip_c=@aud_pc_ip_c," +
                        "aud_pc_host_c=@aud_pc_host_c" +
                        " where " +
                        "pk_mod_id=@id";

            using (SqlConnection sqlCn = new SqlConnection(this.cnBD))
            {
                value = sqlCn.Execute(sqlQuery, new
                {
                    id=entidad.idModulo,
                    fk_mod_id = entidad.idModuloPadre,
                    nivel_n = entidad.nivel,
                    es_mod_hij_b = entidad.esHijo,
                    nombre_c = entidad.nombreModulo,
                    nombre_obj_c = entidad.nombreObjeto,
                    codigo_acc_dir_c = entidad.codigoAccesoDirecto,
                    icono_c = entidad.icono,
                    url_c = entidad.url,
                    num_ord = entidad.numeroOrden,
                    es_visible_b = entidad.esVisible,
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

        public int eliminar(Entity.ModuloBE entidad)
        {

            int value = 0;
            sqlQuery = "update " + bdEsquema + bdTabla + " set " +
                       "aud_id_usu_eli_n=@aud_id_usu_eli_n," +
                        "aud_fec_eli_f=@aud_fec_eli_f," +
                        "aud_pc_ip_c=@aud_pc_ip_c," +
                        "aud_pc_host_c=@aud_pc_host_c," +
                        "aud_es_eli_b=1" +
                        " where " +
                        "pk_mod_id=@id";
            

            using (SqlConnection sqlCn = new SqlConnection(this.cnBD))
            {
                value = sqlCn.Execute(sqlQuery, new
                {
                    id = entidad.idModulo,
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
