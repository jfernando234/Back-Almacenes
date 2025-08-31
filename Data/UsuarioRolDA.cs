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
    public class UsuarioRolDA
    {
        private readonly string cnBD = "";
        private string sqlQuery = "";
        private readonly string bdEsquema = "sch_seguridad.";
        private readonly string bdTabla = "usuario_rol_mov";

        public UsuarioRolDA(string cnBD)
        {
            this.cnBD = cnBD;
            //FluentMapper.EntityMaps.Clear();
            //FluentMapper.Initialize(config => { config.AddMap(new UsuarioRolMAP()); });
            //se reliazo el mapeo de la bd con la clase entity
        }

        public List<Entity.UsuarioRolBE> listar(int idUsuario)
        {

            sqlQuery = bdEsquema + "usp_rol_usuario_list";

            var p = new DynamicParameters();
            p.Add("pk_usu_id", idUsuario);

            using (SqlConnection sqlCn = new SqlConnection(this.cnBD))
            {
                var lista = sqlCn.Query<Entity.UsuarioRolBE>(sqlQuery, p, commandType: CommandType.StoredProcedure).ToList();
                return lista;
            }

        }

        public List<Entity.UsuarioRolDetBE> listarDetalle(int idRol, int idRolUsuario)
        {

            sqlQuery = bdEsquema + "usp_rol_usuario_det_list";

            var p = new DynamicParameters();
            p.Add("pk_rol_id", idRol);
            p.Add("fk_rolmod_usuper_id", idRolUsuario);

            using (SqlConnection sqlCn = new SqlConnection(this.cnBD))
            {
                var lista = sqlCn.Query<Entity.UsuarioRolDetBE>(sqlQuery, p, commandType: CommandType.StoredProcedure).ToList();
                return lista;
            }

        }

        public int mantenimientoUsp(Entity.UsuarioRolBE entidad)
        {

            int value = 0;
            sqlQuery = bdEsquema + "usp_rolmod_usuper_mae_mant";

            using (SqlConnection sqlCn = new SqlConnection(this.cnBD))
            {
                value = sqlCn.ExecuteScalar<int>(sqlQuery, new
                {
                    p_opcion_c = "ins_mant",
                    pk_rolmod_usuper_id = entidad.idUsuarioRol,
                    fk_rol_mod_id = entidad.idRolModulo,
                    fk_usu_per_mod = entidad.idUsuarioPerfil,
                    sel = entidad.sel,
                    /*auditoria*/
                    p_id_usu_login_n = entidad.idUsuarioLogin,
                    p_aud_pc_ip_c = entidad.pcIp,
                    p_aud_pc_host_c = entidad.pcHost,
                }, commandType: System.Data.CommandType.StoredProcedure);
                //value = sqlCn.ExecuteScalar<int>("select max(pk_per_id) from producto_mae");

                return value;
            }
        }

        public int mantenimientoDetUsp(int idUsuarioRol, Entity.UsuarioRolDetBE entidad)
        {

            int value = 0;
            sqlQuery = bdEsquema + "usp_rolmod_usuper_det_mant";

            using (SqlConnection sqlCn = new SqlConnection(this.cnBD))
            {
                value = sqlCn.ExecuteScalar<int>(sqlQuery, new
                {
                    p_opcion_c = "ins_mant",
                    fk_rolmod_usuper_id = idUsuarioRol,
                    id_det = entidad.idUsuarioRolDet,
                    sel = entidad.sel,
                    /*auditoria*/
                    p_id_usu_login_n = entidad.idUsuarioLogin,
                    p_aud_pc_ip_c = entidad.pcIp,
                    p_aud_pc_host_c = entidad.pcHost,
                }, commandType: System.Data.CommandType.StoredProcedure);
                //value = sqlCn.ExecuteScalar<int>("select max(pk_per_id) from producto_mae");

                return value;
            }
        }

    }
}
