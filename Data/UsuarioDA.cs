using Dapper;
using Dapper.FluentMap;
using Entity;
using Entity.Map;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Data
{
    public class UsuarioDA
    {
        private readonly string cnBD = "";
        private string sqlQuery = "";
        private readonly string bdEsquema = "sch_seguridad.";
        private readonly string bdTabla = "usuario_mae";
        private readonly string bdProcedure = "";
        public UsuarioDA(string cnBD)
        {
            this.cnBD = cnBD;
            bdProcedure = bdEsquema + "usp_usuario_mae_mant";

            //FluentMapper.EntityMaps.Clear();
            //FluentMapper.Initialize(config => { config.AddMap(new UsuarioMAP()); });
            //FluentMapper.Initialize(config => { config.AddMap(new ModuloMAP()); });
            //FluentMapper.Initialize(config => { config.AddMap(new PerfilModuloMAP()); });

        }

        public List<Entity.UsuarioBE> listarUsp()
        {
            sqlQuery = bdProcedure;

            var p = new DynamicParameters();
            p.Add("p_accion_c", "lst");
            p.Add("p_opcion_c", "lst_mant");

            using (SqlConnection sqlCn = new SqlConnection(this.cnBD))
            {
                var lista = sqlCn.Query<Entity.UsuarioBE>(sqlQuery, p, commandType: CommandType.StoredProcedure).ToList();
                return lista;
            }
        }

        public Entity.UsuarioBE listarUsp(int id)
        {
            sqlQuery = bdProcedure;
            Entity.UsuarioBE value;

            var p = new DynamicParameters();
            p.Add("p_accion_c", "lst");
            p.Add("p_opcion_c", "lst_mant");
            p.Add("p_pk_usu_id", id);

            using ( SqlConnection sqlCn = new SqlConnection(this.cnBD))
            {

                try
                {
                    value = sqlCn.QuerySingle<Entity.UsuarioBE>(sqlQuery, p, commandType: CommandType.StoredProcedure);
                }
                catch
                {
                    value = null;
                }

                return value;
            }

        }        

        public Entity.UsuarioModuloBE listarUsuarioModuloUsp(int id)
        {

            Entity.UsuarioModuloBE value = new Entity.UsuarioModuloBE();
            sqlQuery = bdProcedure;            
            var p = new DynamicParameters();
            p.Add("p_accion_c", "lst");
            p.Add("p_opcion_c", "lst_usuario_modulo");
            p.Add("p_pk_usu_id", id);

            using (SqlConnection sqlCn = new SqlConnection(this.cnBD))
            {
                using (var multi = sqlCn.QueryMultiple(sqlQuery, p, commandType: CommandType.StoredProcedure))
                {
                    var listaModulo = multi.Read<Entity.ModuloBE>().ToList();
                    var listaPerfilModulo = multi.Read<Entity.PerfilModuloBE>().ToList();                  

                    value.listaModulo = listaModulo;
                    value.listaPerfilModulo = listaPerfilModulo;
                    return value;
                }
            }
        }

        public int agregarUsp(Entity.UsuarioBE entidad)
        {

            int value = 0;
            sqlQuery = bdProcedure;

            using (SqlConnection sqlCn = new SqlConnection(this.cnBD))
            {
                value = sqlCn.Execute(sqlQuery, new
                {
                    p_accion_c = "ins",
                    p_opcion_c = "ins_mant",
                    p_fk_per_id = entidad.idPersona,
                    p_login_c = entidad.login,
                    p_clave_v = entidad.clave,
                    p_clave_nue_b = entidad.nuevaClave,
                    p_pri_ape_c = entidad.primerApellido,
                    p_seg_ape_c = entidad.segundoApellido,
                    p_pre_nom_c = entidad.nombres,
                    p_num_doc_c = entidad.numeroDocumento,
                    p_fk_are_id = entidad.idArea,
                    p_fk_cgo_id = entidad.idCargo,
                    p_correo_ins_c = entidad.correoInstitucional,
                    p_estado_n = entidad.idEstado,
                    /*auditoria*/
                    p_id_usu_login_n = entidad.idUsuarioLogin,
                    p_aud_pc_ip_c = entidad.pcIp,
                    p_aud_pc_host_c = entidad.pcHost,
                }, commandType: System.Data.CommandType.StoredProcedure);
                return value;
            }
        }

        public int agregarUspReturnOut(Entity.UsuarioBE entidad)
        {

            int value = 0;
            sqlQuery = bdProcedure;

            var p = new DynamicParameters();
            p.Add("p_accion_c", "ins");
            p.Add("p_opcion_c", "ins_mant");
            p.Add("p_fk_per_id", entidad.idPersona);
            p.Add("p_login_c", entidad.login);
            p.Add("p_clave_v", entidad.clave);
            p.Add("p_clave_nue_b", entidad.nuevaClave);
            p.Add("p_pri_ape_c", entidad.primerApellido);
            p.Add("p_seg_ape_c", entidad.segundoApellido);
            p.Add("p_pre_nom_c", entidad.nombres);
            p.Add("p_num_doc_c", entidad.numeroDocumento);
            p.Add("p_fk_are_id", entidad.idArea);
            p.Add("p_fk_cgo_id", entidad.idCargo);
            p.Add("p_correo_ins_c", entidad.correoInstitucional);
            p.Add("p_estado_n", entidad.idEstado);
            /*auditoria*/
            p.Add("p_id_usu_login_n", entidad.idUsuarioLogin);
            p.Add("p_aud_pc_ip_c", entidad.pcIp);
            p.Add("p_aud_pc_host_c", entidad.pcHost);
            p.Add("p_sal_n_out", dbType: DbType.Int32, direction: ParameterDirection.Output);

            using (SqlConnection sqlCn = new SqlConnection(this.cnBD))
            {
                value = sqlCn.Execute(sqlQuery, p, commandType: System.Data.CommandType.StoredProcedure);
                value = p.Get<int>("p_sal_n_out");
                return value;
            }
        }

        public int modificarUspReturn(string opcion, Entity.UsuarioBE entidad)
        {

            int value = 0;
            sqlQuery = bdProcedure;

            var p = new DynamicParameters();
            p.Add("p_accion_c", "upd");
            p.Add("p_opcion_c", opcion);
            p.Add("p_pk_usu_id", entidad.idUsuario);
            p.Add("p_fk_per_id", entidad.idPersona);
            //p.Add("p_login_c", entidad.login);
            if (entidad.clave !=null)
            { p.Add("p_clave_v", entidad.clave); }      
            p.Add("p_clave_nue_b", entidad.nuevaClave);
            p.Add("p_pri_ape_c", entidad.primerApellido);
            p.Add("p_seg_ape_c", entidad.segundoApellido);
            p.Add("p_pre_nom_c", entidad.nombres);
            p.Add("p_num_doc_c", entidad.numeroDocumento);
            p.Add("p_fk_are_id", entidad.idArea);
            p.Add("p_fk_cgo_id", entidad.idCargo);
            p.Add("p_correo_ins_c", entidad.correoInstitucional);
            p.Add("p_estado_n", entidad.idEstado);
            /*auditoria*/
            p.Add("p_id_usu_login_n", entidad.idUsuarioLogin);
            p.Add("p_aud_pc_ip_c", entidad.pcIp);
            p.Add("p_aud_pc_host_c", entidad.pcHost);
            p.Add("p_sal_n_out", dbType: DbType.Int32, direction: ParameterDirection.Output);

            using (SqlConnection sqlCn = new SqlConnection(this.cnBD))
            {
                value = sqlCn.Execute(sqlQuery, p, commandType: System.Data.CommandType.StoredProcedure);
                value = p.Get<int>("p_sal_n_out");
                return value;
            }
        }

        public int eliminar(Entity.UsuarioBE entidad)
        {

            int value = 0;
            sqlQuery = "update " + bdEsquema + bdTabla + " set " +
                       "aud_id_usu_eli_n=@aud_id_usu_eli_n," +
                       "aud_fec_eli_f=@aud_fec_eli_f," +
                       "aud_pc_ip_c=@aud_pc_ip_c," +
                       "aud_pc_host_c=@aud_pc_host_c," +
                       "aud_es_eli_b=1" +
                       " where " +
                       "pk_usu_id=@id";

            using (SqlConnection sqlCn = new SqlConnection(this.cnBD))
            {
                value = sqlCn.Execute(sqlQuery, new
                {
                    id = entidad.idUsuario,
                    /*auditoria*/
                    aud_id_usu_eli_n = entidad.idUsuarioLogin,
                    aud_fec_eli_f = DateTime.Now,
                    aud_pc_ip_c = entidad.pcIp,
                    aud_pc_host_c = entidad.pcHost,
                });
                return value;
            }
        }

        public int eliminarUsp(Entity.UsuarioBE entidad)
        {

            int value = 0;
            sqlQuery = bdProcedure;

            var p = new DynamicParameters();
            p.Add("p_accion_c", "del");
            p.Add("p_opcion_c", "del_mant");
            p.Add("p_pk_usu_id", entidad.idUsuario);
            p.Add("p_id_usu_login_n", entidad.idUsuarioLogin);
            p.Add("aud_pc_ip_c", entidad.pcIp);
            p.Add("aud_pc_host_c", entidad.pcHost);

            using (SqlConnection sqlCn = new SqlConnection(this.cnBD))
            {
                value = sqlCn.Execute(sqlQuery, p, commandType: CommandType.StoredProcedure);
                return value;
            }
        }
        public int registroCliente(Entity.PersonaBE entidad)
        {

            int value = 0;
            string sqlQueryIdentity = "select SCOPE_IDENTITY()";

            string bdTabla_Personsa = "persona_mae";
            sqlQuery = "insert into " + bdTabla_Personsa +
                        "(tip_per_id_n,fk_tip_doc_id,num_doc_c,pri_ape_c,seg_ape_c,pre_nom_c,fec_nac_f,sexo_b,correo_per_c," +
                            "aud_id_usu_cre_n,aud_fec_cre_f,aud_id_usu_mod_n,aud_fec_mod_f,aud_pc_ip_c,aud_pc_host_c,aud_es_eli_b,codigo_merchant) " +
                        " values (@tip_per_id_n,@fk_tip_doc_id,@num_doc_c,@pri_ape_c,@seg_ape_c,@pre_nom_c,@fec_nac_f,@sexo_b,@correo_per_c," +
                            "@aud_id_usu_cre_n,@aud_fec_cre_f,@aud_id_usu_mod_n,@aud_fec_mod_f,@aud_pc_ip_c,@aud_pc_host_c,0,@codigo_merchant)";

            sqlQuery = sqlQuery + ";" + sqlQueryIdentity;

            using (SqlConnection sqlCn = new SqlConnection(this.cnBD))
            {

                int idPersona = sqlCn.ExecuteScalar<int>(sqlQuery, new
                {
                    tip_per_id_n = 1,
                    fk_tip_doc_id = entidad.idTipoDocumento,
                    num_doc_c = entidad.numeroDocumento,
                    pri_ape_c = entidad.primerApellido,
                    seg_ape_c = entidad.segundoApellido,
                    pre_nom_c = entidad.nombres,
                    fec_nac_f = entidad.fechaNacimiento,
                    sexo_b = entidad.sexo,
                    correo_per_c = entidad.correo,
                    /*auditoria*/
                    aud_id_usu_cre_n = entidad.idUsuarioLogin,
                    aud_fec_cre_f = DateTime.Now,
                    aud_id_usu_mod_n = entidad.idUsuarioLogin,
                    aud_fec_mod_f = DateTime.Now,
                    aud_pc_ip_c = entidad.pcIp,
                    aud_pc_host_c = entidad.pcHost,
                    codigo_merchant=entidad.login
                });



                //int value = 0;
                sqlQuery = "usp_registro_cliente";

                var p = new DynamicParameters();
                p.Add("idPersona", idPersona);
                p.Add("clave", entidad.clave);
                
                value = sqlCn.Execute(sqlQuery, p, commandType: CommandType.StoredProcedure);
                return value;
              //return value;
            }
        }


        public int registroPerfilCli(Entity.PersonaBE entidad, int idUsuario)
        {

            int value = 0;
            string sqlQueryIdentity = "select SCOPE_IDENTITY()";

            string bdTabla_Personsa = "persona_mae";
            sqlQuery = "insert into " + bdTabla_Personsa +
                        "(tip_per_id_n,fk_tip_doc_id,num_doc_c,pri_ape_c,seg_ape_c,pre_nom_c,fec_nac_f,sexo_b,correo_per_c," +
                            "aud_id_usu_cre_n,aud_fec_cre_f,aud_id_usu_mod_n,aud_fec_mod_f,aud_pc_ip_c,aud_pc_host_c,aud_es_eli_b,contacto,celular) " +
                        " values (@tip_per_id_n,@fk_tip_doc_id,@num_doc_c,@pri_ape_c,@seg_ape_c,@pre_nom_c,@fec_nac_f,@sexo_b,@correo_per_c," +
                            "@aud_id_usu_cre_n,@aud_fec_cre_f,@aud_id_usu_mod_n,@aud_fec_mod_f,@aud_pc_ip_c,@aud_pc_host_c,0,@contacto,@celular)";

            sqlQuery = sqlQuery + ";" + sqlQueryIdentity;

            using (SqlConnection sqlCn = new SqlConnection(this.cnBD))
            {

                int idPersona = sqlCn.ExecuteScalar<int>(sqlQuery, new
                {
                    tip_per_id_n = 1,
                    fk_tip_doc_id = entidad.idTipoDocumento,
                    num_doc_c = entidad.numeroDocumento,
                    pri_ape_c = entidad.primerApellido,
                    seg_ape_c = entidad.segundoApellido,
                    pre_nom_c = entidad.nombres,
                    fec_nac_f = entidad.fechaNacimiento,
                    sexo_b = entidad.sexo,
                    correo_per_c = entidad.correo,                    
                    /*auditoria*/
                    aud_id_usu_cre_n = entidad.idUsuarioLogin,
                    aud_fec_cre_f = DateTime.Now,
                    aud_id_usu_mod_n = entidad.idUsuarioLogin,
                    aud_fec_mod_f = DateTime.Now,
                    aud_pc_ip_c = entidad.pcIp,
                    aud_pc_host_c = entidad.pcHost,
                    contacto = entidad.contacto,
                    celular = entidad.celular,
                });

                //int value = 0;
                sqlQuery = "usp_registro_perfil_cliente";

                var p = new DynamicParameters();
                p.Add("idPersona", idPersona);
                p.Add("idUsuario", idUsuario);

                value = sqlCn.Execute(sqlQuery, p, commandType: CommandType.StoredProcedure);
                return value;
                //return value;
            }
        }

        public int modificaPerfilCli(Entity.PersonaBE entidad)
        {

            int value = 0;
            sqlQuery = "update persona_mae set " +
                        "tip_per_id_n=@tip_per_id_n," +
                        "fk_tip_doc_id=@fk_tip_doc_id," +
                        "num_doc_c=@num_doc_c," +
                        "pri_ape_c=@pri_ape_c," +
                        "seg_ape_c=@seg_ape_c," +
                        "pre_nom_c=@pre_nom_c," +
                        "fec_nac_f=@fec_nac_f," +
                        "sexo_b=@sexo_b," +
                        "correo_per_c=@correo_per_c," +
                        "aud_id_usu_mod_n=@aud_id_usu_mod_n," +
                        "aud_fec_mod_f=@aud_fec_mod_f," +
                        "aud_pc_ip_c=@aud_pc_ip_c," +
                        "aud_pc_host_c=@aud_pc_host_c" +
                        "contacto=@contacto" +
                        "celular=@celular" +
                        " where " +
                        "pk_per_id=@id";

            using (SqlConnection sqlCn = new SqlConnection(this.cnBD))
            {
                value = sqlCn.Execute(sqlQuery, new
                {
                    id = entidad.idPersona,
                    tip_per_id_n = entidad.idTipoPersona,
                    fk_tip_doc_id = entidad.idTipoDocumento,
                    num_doc_c = entidad.numeroDocumento,
                    pri_ape_c = entidad.primerApellido,
                    seg_ape_c = entidad.segundoApellido,
                    pre_nom_c = entidad.nombres,
                    fec_nac_f = entidad.fechaNacimiento,
                    sexo_b = entidad.sexo,
                    correo_per_c = entidad.correo,
                    /*auditoria*/
                    aud_id_usu_mod_n = entidad.idUsuarioLogin,
                    aud_fec_mod_f = DateTime.Now,
                    aud_pc_ip_c = entidad.pcIp,
                    aud_pc_host_c = entidad.pcHost,
                    contacto=entidad.contacto,
                    celular = entidad.celular
                });
                return value;
            }
        }

        public PersonaBE listarPerfil(int id)
        {

            sqlQuery = "select * from persona_mae where aud_es_eli_b=0 and pk_per_id=@id";
            PersonaBE value;

            using (SqlConnection sqlCn = new SqlConnection(this.cnBD))
            {
                try
                {
                    value = sqlCn.Query<dynamic>(sqlQuery, new { id = id }).Select(t => new PersonaBE()
                    {
                        idPersona = t.pk_per_id,
                        idTipoPersona = t.tip_per_id_n,
                        idTipoDocumento = t.fk_tip_doc_id,
                        numeroDocumento = t.num_doc_c,
                        primerApellido = t.pri_ape_c,
                        segundoApellido = t.seg_ape_c,
                        nombres = t.pre_nom_c,
                        fechaNacimiento = t.fec_nac_f,
                        sexo = t.sexo_b,
                        correo = t.correo_per_c
                    }).FirstOrDefault();
                }
                catch
                {
                    value = null;
                }

                return value;
            }

        }

    }
}
