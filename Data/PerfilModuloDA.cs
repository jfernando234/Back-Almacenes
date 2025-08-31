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
    public class PerfilModuloDA
    {
        private readonly string cnBD = "";
        private string sqlQuery = "";
        private readonly string bdEsquema = "sch_seguridad.";
        private readonly string bdTabla = "perfil_modulo_mae";

        public PerfilModuloDA(string cnBD)
        {
            this.cnBD = cnBD;
            //FluentMapper.EntityMaps.Clear();
            //FluentMapper.Initialize(config => { config.AddMap(new PerfilModuloMAP()); });
        }

        public List<Entity.PerfilModuloBE> listar()
        {

            sqlQuery = "select pm.*,m.nombre_c as nombre_modulo from " + bdEsquema + bdTabla + " as pm " +
                " inner join " + bdEsquema + "modulo_mae as m on pm.fk_mod_id=m.pk_mod_id" +
                " where pm.aud_es_eli_b=0";
                
            using (SqlConnection sqlCn = new SqlConnection(this.cnBD))
            {
                var lista = sqlCn.Query<Entity.PerfilModuloBE>(sqlQuery).ToList();
                return lista;
            }

        }
        public Entity.PerfilModuloBE listar(int id)
        {

            sqlQuery = "select * from " + bdEsquema + bdTabla + " where pk_per_mod_id=@id";
            Entity.PerfilModuloBE value;

            using (SqlConnection sqlCn = new SqlConnection(this.cnBD))
            {
                try
                {
                    value = sqlCn.QuerySingle<Entity.PerfilModuloBE>(sqlQuery, new { id = id });
                }
                catch
                {
                    value = null;
                }

                return value;
            }
        }

        public int agregar(Entity.PerfilModuloBE entidad)
        {

            int value = 0;
            sqlQuery = "insert into " + bdEsquema + bdTabla +
                        "(fk_per_id,fk_mod_id,consultar_b,crear_b,modificar_b,eliminar_b,imprimir_b,descargar_pdf_b,descagar_excel_b," +
                            "aud_id_usu_cre_n,aud_fec_cre_f,aud_id_usu_mod_n,aud_fec_mod_f,aud_pc_ip_c,aud_pc_host_c,aud_es_eli_b) " +
                        " values (@fk_per_id,@fk_mod_id,@consultar_b,@crear_b,@modificar_b,@eliminar_b,@imprimir_b,@descargar_pdf_b,@descagar_excel_b," +
                            "@aud_id_usu_cre_n,@aud_fec_cre_f,@aud_id_usu_mod_n,@aud_fec_mod_f,@aud_pc_ip_c,@aud_pc_host_c,0)";

            using (SqlConnection sqlCn = new SqlConnection(this.cnBD))
            {
                value = sqlCn.Execute(sqlQuery, new
                {
                    fk_per_id = entidad.idPerfil,
                    fk_mod_id = entidad.idModulo,
                    consultar_b = entidad.consultar,
                    crear_b = entidad.agregar,
                    modificar_b = entidad.modificar,
                    eliminar_b = entidad.eliminar,
                    imprimir_b = entidad.imprimir,
                    descargar_pdf_b = entidad.descargarPDF,
                    descagar_excel_b = entidad.descargarExcel,
                    /*auditotia*/
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

        public int modificar(Entity.PerfilModuloBE entidad)
        {

            int value = 0;
            sqlQuery = "update " + bdEsquema + bdTabla + " set " +
                        "fk_per_id=@fk_per_id," +
                        "fk_mod_id=@fk_mod_id," +
                        "consultar_b=@consultar_b," +
                        "crear_b=@crear_b," +
                        "modificar_b=@modificar_b," +
                        "eliminar_b=@eliminar_b," +
                        "imprimir_b=@imprimir_b," +
                        "descargar_pdf_b=@descargar_pdf_b," +
                        "descagar_excel_b=@descagar_excel_b," + 
                        "aud_id_usu_mod_n=@aud_id_usu_mod_n," +
                        "aud_fec_mod_f=@aud_fec_mod_f," +
                        "aud_pc_ip_c=@aud_pc_ip_c," +
                        "aud_pc_host_c=@aud_pc_host_c" +
                        " where " +
                        "pk_per_mod_id=@id";

            using (SqlConnection sqlCn = new SqlConnection(this.cnBD))
            {
                value = sqlCn.Execute(sqlQuery, new
                {
                    id = entidad.idPerfilModulo,
                    fk_per_id = entidad.idPerfil,
                    fk_mod_id = entidad.idModulo,
                    consultar_b = entidad.consultar,
                    crear_b = entidad.agregar,
                    modificar_b = entidad.modificar,
                    eliminar_b = entidad.eliminar,
                    imprimir_b = entidad.imprimir,
                    descargar_pdf_b = entidad.descargarPDF,
                    descagar_excel_b = entidad.descargarExcel,
                    /*auditotia*/
                    aud_fec_cre_f = DateTime.Now,
                    aud_id_usu_mod_n = entidad.idUsuarioLogin,
                    aud_fec_mod_f = DateTime.Now,
                    aud_pc_ip_c = entidad.pcIp,
                    aud_pc_host_c = entidad.pcHost,
                });
                return value;
            }
        }

        public int eliminar(Entity.PerfilModuloBE entidad)
        {

            int value = 0;
            sqlQuery = "update " + bdEsquema + bdTabla + " set " +
                       "aud_id_usu_eli_n=@aud_id_usu_eli_n," +
                        "aud_fec_eli_f=@aud_fec_eli_f," +
                        "aud_pc_ip_c=@aud_pc_ip_c," +
                        "aud_pc_host_c=@aud_pc_host_c," +
                        "aud_es_eli_b=1" +
                        " where " +
                        "pk_per_mod_id=@id";


            using (SqlConnection sqlCn = new SqlConnection(this.cnBD))
            {
                value = sqlCn.Execute(sqlQuery, new
                {
                    id = entidad.idPerfilModulo,
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
