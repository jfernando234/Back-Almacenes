using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Service
{
    public class UsuarioSER
    {
        private Data.UsuarioDA objData;
        private readonly string cnBD = "";
        private readonly IMapper mapper;        

        public UsuarioSER(string cnBD, IMapper mapper)
        {
            this.cnBD = cnBD;
            this.mapper = mapper;                 
        }

        public List<DTO.Result.UsuarioDTO> getAll()
        {
            objData = new Data.UsuarioDA(this.cnBD);
            List<Entity.UsuarioBE> lista = new List<Entity.UsuarioBE>();

            lista = objData.listarUsp();

            return mapper.Map<List<DTO.Result.UsuarioDTO>>(lista);

        }

        public DTO.Result.UsuarioDTO get(int id)
        {
            objData = new Data.UsuarioDA(this.cnBD);
            Entity.UsuarioBE lista = new Entity.UsuarioBE();

            lista = objData.listarUsp(id);

            return mapper.Map<DTO.Result.UsuarioDTO>(lista);

        }

        public DTO.Result.UsuarioyPerfilDTO getUsuarioPerfil(int idUsuario)
        {
            //listar los datos del usuario x codigo
            objData = new Data.UsuarioDA(this.cnBD);
            Entity.UsuarioBE entity = new Entity.UsuarioBE();
            DTO.Result.UsuarioDTO dtoUsuario = new DTO.Result.UsuarioDTO();

            entity = objData.listarUsp(idUsuario);
            dtoUsuario = mapper.Map<DTO.Result.UsuarioDTO>(entity);

            //consultar los perfile del usuario
            Service.UsuarioPerfilSER servicio = new Service.UsuarioPerfilSER(this.cnBD, this.mapper);
            List<DTO.Result.UsuarioPerfilDTO> dtoUsuarioPerfil = new List<DTO.Result.UsuarioPerfilDTO>();
            dtoUsuarioPerfil = servicio.listarxUsuario(idUsuario);
            //************************************************

            DTO.Result.UsuarioyPerfilDTO dtoValue = new DTO.Result.UsuarioyPerfilDTO();
            List<DTO.Result.UsuarioPerfilDTO> dtoValueHijo = new List<DTO.Result.UsuarioPerfilDTO>();

            dtoValue = mapper.Map<DTO.Result.UsuarioyPerfilDTO>(dtoUsuario);
            dtoValueHijo = mapper.Map<List<DTO.Result.UsuarioPerfilDTO>>(dtoUsuarioPerfil);

            dtoValue.listaUsuarioPerfil = dtoValueHijo;

            return dtoValue;

        }

        private bool validarAgregar(DTO.UsuarioAgregarDTO dto, out string mensaje)
        {
            bool value = false;
            //validar, x dni
            List<DTO.Result.UsuarioDTO> listaUsuario = getAll();

            if (listaUsuario.Where(x => x.numeroDocumento == dto.numeroDocumento).Count() > 0)
            {
                mensaje = "La persona seleccionada ya cuenta con un usuario asignado";
                return value;
            }

            //validar, correo
            if (listaUsuario.Where(x => x.correoInstitucional.ToUpper() == dto.correoInstitucional.ToUpper()).Count() > 0)
            {
                mensaje = "El correo institucional ya existe en la base de datos";
                return value;
            }

            //validar, apellidos y nombres
            if (listaUsuario.Where(x => x.primerApellido.ToUpper() == dto.primerApellido.ToUpper() &&
                                        x.segundoApellido.ToUpper() == dto.segundoApellido.ToUpper() &&
                                        x.nombres.ToUpper() == dto.nombres.ToUpper()
                                                    ).Count() > 0)
            {
                mensaje = "Los datos del usuario (apellidos y nombres) ya existe en la base de datos";
                return value;
            }

            mensaje = "";
            return true;
        }

        public string getLogin(DTO.UsuarioAgregarDTO dto)
        {
            //1er caso login
            string loginInicial = dto.nombres.Substring(0, 1).ToString() + dto.primerApellido.ToString();
            string login = loginInicial;

            List<DTO.Result.UsuarioDTO> listaUsuario = getAll();

            if (listaUsuario.Where(x => x.login.ToUpper() == login.ToUpper()).Count() > 0)
            {
                //2do caso         
                login = login + dto.segundoApellido.Substring(0, 1).ToString();
            }

            int contador = 2;
            bool existeLogin = true;
            do
            {

                listaUsuario = getAll();
                if (listaUsuario.Where(x => x.login.ToUpper() == login.ToUpper()).Count() > 0)
                {
                    existeLogin = true;
                    //3er caso
                    login = loginInicial + dto.segundoApellido.Substring(0, contador).ToString();
                    contador++;
                }
                else
                {
                    existeLogin = false;
                }

            } while (existeLogin == true);

            return login;
        }
        public DTO.Result.Result<int> agregar(DTO.UsuarioAgregarDTO dto)
        {
            DTO.Result.Result<int> result = new DTO.Result.Result<int>();
            string mensaje = "";

            if (validarAgregar(dto, out mensaje) == false)
            {
                result.errorMensaje = mensaje;
                result.errorCodigo = "VAL";
                return result;
            }

            var entity = mapper.Map<Entity.UsuarioBE>(dto);

            //generar del usuario login            
            entity.login = getLogin(dto).ToUpper();
            //generar la clave del usuario
            string claveGenerada = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
            byte[] claveBytes = Common.Cifrado.encriptarSHA256(claveGenerada);
            entity.clave = claveGenerada;

            entity.nuevaClave = true;
            entity.idEstado = (short)Type.Usuario.Estado.Activo;

            //salida json
            int value = -1;
            objData = new Data.UsuarioDA(this.cnBD);
            value = objData.agregarUspReturnOut(entity);

            if (value < 1)
            {
                result.errorMensaje = "La operación no se realizo correctamente";
                result.errorCodigo = "ERROR";
                return result;
            }

            //enviar correo al usuario, para notificar su cuenta de usuario
            Task.Run(async () =>
            {
                CorreoSER correo = new CorreoSER();
                Task tareaEnviarCorreo = correo.crearUsuario(getLinkApp(), getAmbiente(),
                    entity.ApellidosyNombres.ToUpper(), entity.login, claveGenerada, entity.correoInstitucional);
            });

            result.value = value;
            return result;

        }

        public DTO.Result.Result<int> agregarUsuarioPerfil(DTO.UsuarioAgregarPerfilDTO dto)
        {
            int value = -1;
            DTO.Result.Result<int> result = new DTO.Result.Result<int>();
            result = agregar(dto);
            value = result.value;

            if (value < 1)
            {
                return result;
            }

            //grabar datos del perfil
            foreach (DTO.UsuarioAgregarPerfilDTO.UsuarioPerfilDTO fila in dto.listaPerfil)
            {

                Service.UsuarioPerfilSER service = new Service.UsuarioPerfilSER(this.cnBD, this.mapper);
                DTO.UsuarioPerfilAgregarDTO dtoPerfil = new DTO.UsuarioPerfilAgregarDTO();

                dtoPerfil.idPerfil = fila.idPerfil;
                dtoPerfil.idUsuario = value;
                dtoPerfil.idUsuarioLogin = dto.idUsuarioLogin;
                dtoPerfil.pcIp = dto.pcIp;
                dtoPerfil.pcHost = dto.pcHost;

                DTO.Result.Result<int> value2 = service.agregar(dtoPerfil);

                if (value2.value < 1)
                {
                    result.errorMensaje = value2.errorMensaje;
                    result.errorCodigo = value2.errorCodigo;
                    return result;
                }

            }

            result.value = value;
            return result;

        }

        public DTO.Result.Result<int> modificar(DTO.UsuarioModificarDTO dto)
        {
            DTO.Result.Result<int> result = new DTO.Result.Result<int>();

            //validar si el el usuario existe
            DTO.Result.UsuarioDTO dtoUsuario = get(dto.idUsuario);

            if (dtoUsuario == null)
            {
                result.errorMensaje = "El usuario no existe en la Base de Datos.";
                result.errorCodigo = "VAL";
                return result;
            }

            var entity = mapper.Map<Entity.UsuarioBE>(dto);

            //salida json
            int value = -1;
            objData = new Data.UsuarioDA(this.cnBD);
            value = objData.modificarUspReturn("upd_mant", entity);

            if (value < 1)
            {
                result.errorMensaje = "La operación no se realizo correctamente";
                result.errorCodigo = "ERROR";
                return result;
            }

            result.value = value;
            return result;

        }

        public DTO.Result.Result<int> eliminar(DTO.EliminarDTO dto)
        {
            DTO.Result.Result<int> result = new DTO.Result.Result<int>();

            //buscar el usuario, si existe
            objData = new Data.UsuarioDA(this.cnBD);
            var entity = mapper.Map<Entity.UsuarioBE>(dto);

            DTO.Result.UsuarioDTO usuario = get(dto.id);
            if (usuario == null)
            {
                result.errorMensaje = "El registro que desea eliminar no existe";
                result.errorCodigo = "VAL";
                return result;
            }

            //salida json
            int value = -1;
            entity.idUsuario = dto.id;
            value = objData.eliminar(entity);

            if (value < 1)
            {
                result.errorMensaje = "La operación no se realizo correctamente";
                result.errorCodigo = "ERROR";
                return result;
            }

            result.value = value;
            return result;

        }

        public DTO.Result.Result<int> restaurarClave(DTO.UsuarioRestaurarClaveDTO dto)
        {

            int value = -1;
            DTO.Result.Result<int> result = new DTO.Result.Result<int>();
            //buscar el id el usuario
            int idUsuario = -1;
            DTO.Result.UsuarioDTO usuario = getAll().Where(u => u.login.ToUpper() == dto.login.ToUpper() &&
                                                                    u.numeroDocumento == dto.numeroDocumento &&
                                                                    u.correoInstitucional.ToUpper() == dto.correoInstitucional.ToUpper()
                                                                    ).FirstOrDefault();

            if (usuario != null)
            {
                idUsuario = usuario.idUsuario;
            }
            else
            {
                result.errorMensaje = "Los datos ingresados no se encuentran en la BD";
                result.errorCodigo = "VAL";
                return result;
            }

            //generar la clave del usuario            
            string claveGenerada = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
            byte[] claveBytes = Common.Cifrado.encriptarSHA256(claveGenerada);

            Entity.UsuarioBE entity = new Entity.UsuarioBE();
            entity.idUsuario = idUsuario;
            entity.clave = claveGenerada;
            entity.nuevaClave = true;
            entity.pcIp = dto.pcIp;
            entity.pcHost = dto.pcHost;

            //grabar en la bd, la nueva contraseña
            objData = new Data.UsuarioDA(this.cnBD);
            value = objData.modificarUspReturn("upd_cambiar_clave", entity);

            if (value < 1)
            {
                result.errorMensaje = "La operación no se realizo correctamente";
                result.errorCodigo = "ERROR";
                return result;
            }

            //enviar correo al usuario, para notificar su clave restaurada
            Task.Run(async () =>
            {
                CorreoSER correo = new CorreoSER();
                Task tareaEnviarCorreo = correo.restaurarClaveUsuario(getLinkApp(), getAmbiente(), 
                    usuario.ApellidosyNombres, usuario.login, claveGenerada, usuario.correoInstitucional);
            });

            result.value = value;
            return result;

        }

        public DTO.Result.Result<int> cambiarClave(DTO.UsuarioCambiarClaveDTO dto)
        {

            DTO.Result.Result<int> result = new DTO.Result.Result<int>();

            string usuarioLogin = dto.login;
            string claveAnterior = dto.claveAnterior;
            string ClaveNueva = dto.claveNueva;

            //listar la clave del usuario guardado en bd en bytes
            objData = new Data.UsuarioDA(this.cnBD);
            Entity.UsuarioBE usuarioBD = objData.listarUsp(dto.idUsuario);

            if (usuarioBD == null || usuarioBD.login.ToUpper() != usuarioLogin.ToUpper())
            {
                result.errorMensaje = "El usuario no existe en la Base de Datos.";
                result.errorCodigo = "VAL";
                return result;
            }

            //byte[] claveBDBytes = usuarioBD.clave;

            //convertir la clave anterior en bytes
            string claveGenerada = claveAnterior;
            byte[] claveBytes = Common.Cifrado.encriptarSHA256(claveGenerada);

            if (usuarioBD.clave != claveGenerada)
            {
                result.errorMensaje = "La contraseña anterior ingresada, es incorrecto.";
                result.errorCodigo = "VAL";
                return result;
            }

            //registrar la nueva clave
            Entity.UsuarioBE entity = new Entity.UsuarioBE();
            claveBytes = Common.Cifrado.encriptarSHA256(ClaveNueva);

            //grabar en la bd
            entity.idUsuario = dto.idUsuario;
            entity.clave = ClaveNueva;
            entity.nuevaClave = false;
            entity.pcIp = dto.pcIp;
            entity.pcHost = dto.pcHost;
            objData = new Data.UsuarioDA(this.cnBD);

            int value = -1;
            value = objData.modificarUspReturn("upd_cambiar_clave", entity);

            //salida json          
            if (value < 1)
            {
                result.errorMensaje = "La operación no se realizo correctamente";
                result.errorCodigo = "ERROR";
                return result;
            }

            result.value = value;
            return result;

        }

        public DTO.Result.Result<DTO.Result.UsuarioLoginDTO> validarLogin(DTO.UsuarioDatosLoginDTO dto)
        {
            DTO.Result.Result<DTO.Result.UsuarioLoginDTO> result = new DTO.Result.Result<DTO.Result.UsuarioLoginDTO>();

            try
            {
                
                //string usuarioLogin = Common.Cadena.FromBase64String(dto.login);
                //string usuarioClave = Common.Cadena.FromBase64String(dto.clave);

                string usuarioLogin = dto.login;
                string usuarioClave = dto.clave;


                //obtener el id del usuario x medio del login
                int idUsuario = -1;

                DTO.Result.UsuarioDTO usuario = getAll().Where(u => u.login.ToUpper() == usuarioLogin.ToUpper()).FirstOrDefault();

                if (usuario != null)
                {
                    idUsuario = usuario.idUsuario;
                }

                //listar la clave del usuario guardado en bd en bytes
                Entity.UsuarioBE usuarioBD = objData.listarUsp(idUsuario);
                byte[] claveBDBytes = null;

                /*if (usuarioBD != null)
                {
                    claveBDBytes = usuarioBD.clave;
                }*/

                //convertir la clave en bytes
                string claveGenerada = usuarioClave;
                byte[] claveBytes = Common.Cifrado.encriptarSHA256(claveGenerada);
                bool value = Common.Cifrado.compararBytes(claveBDBytes, claveBytes);

                //salida json           
                if (usuarioBD.clave != claveGenerada)
                {
                    result.errorMensaje = "Usuario o contraseña incorrecta";
                    result.errorCodigo = "VAL";
                    return result;
                }

                DTO.Result.UsuarioLoginDTO dtoValue = mapper.Map<DTO.Result.UsuarioLoginDTO>(usuarioBD);
                result.value = dtoValue;

                return result;
            }
            catch (Exception ex)
            {
                result.errorMensaje = ex.Message.ToString();
                result.errorCodigo = "VAL";
                return result;
            }

        }

        private Entity.UsuarioModuloBE listarUsuarioAcceso(int idUsuario)
        {

            Entity.UsuarioModuloBE value = new Entity.UsuarioModuloBE();
            Entity.UsuarioModuloBE usuarioPermisos = new Entity.UsuarioModuloBE();

            objData = new Data.UsuarioDA(this.cnBD);
            usuarioPermisos = objData.listarUsuarioModuloUsp(idUsuario);

            //obtener la lista total de modulos            
            Data.ModuloDA dataModulo = new Data.ModuloDA(this.cnBD);
            var listaModuloAll = dataModulo.listar();
            /***/

            List<Entity.ModuloBE> listaUsuarioPermisos = new List<Entity.ModuloBE>();

            foreach (var listUsuario in usuarioPermisos.listaModulo)
            {
                listaUsuarioPermisos.Add(listUsuario);
                if (listUsuario.idModuloPadre != 0)
                {
                    listarModuloPadre(listaModuloAll, listaUsuarioPermisos, listUsuario.idModuloPadre);
                }
            }

            value.listaModulo = listaUsuarioPermisos.OrderBy(x => x.idModulo).OrderBy(x => x.numeroOrden).ToList();
            value.listaPerfilModulo = usuarioPermisos.listaPerfilModulo;
            return value;

        }

        private void listarModuloPadre(List<Entity.ModuloBE> listaModulo, List<Entity.ModuloBE> listModuloUsuario, int idModuloPadre)
        {
            foreach (var listUsuario in listaModulo)
            {
                if (listUsuario.idModulo == idModuloPadre)
                {
                    if (listModuloUsuario.Where(x => x.idModulo == listUsuario.idModulo).Count() == 0)
                    {
                        listModuloUsuario.Add(listUsuario);
                    }
                    if (listUsuario.idModuloPadre != 0)
                    {
                        listarModuloPadre(listaModulo, listModuloUsuario, listUsuario.idModuloPadre);
                    }
                }
            }
        }

        public List<DTO.Result.UsuarioModuloDTO> listarUsuarioModulos(int idUsuario)
        {
            List<DTO.Result.UsuarioModuloDTO> valueLista = new List<DTO.Result.UsuarioModuloDTO>();

            //listar los permisos del usuario
            Entity.UsuarioModuloBE usuarioModuloBE = new Entity.UsuarioModuloBE();
            usuarioModuloBE = listarUsuarioAcceso(idUsuario);

            //mapear lo objetos devueltos
            var lista1 = mapper.Map<List<DTO.Result.ModuloDTO>>(usuarioModuloBE.listaModulo);
            var lista2 = mapper.Map<List<DTO.Result.UsuarioModuloDTO.ModuloAccesoDTO>>(usuarioModuloBE.listaPerfilModulo);

            //recorrer la clase modulo y perfilmodulo
            foreach (var fila in lista1)
            {

                DTO.Result.UsuarioModuloDTO item = new DTO.Result.UsuarioModuloDTO();

                item = mapper.Map<DTO.Result.UsuarioModuloDTO>(fila);

                //permiso modulo
                DTO.Result.UsuarioModuloDTO.ModuloAccesoDTO newfila = new DTO.Result.UsuarioModuloDTO.ModuloAccesoDTO();
                newfila = lista2.Where(x => x.idModulo == fila.idModulo).FirstOrDefault();

                item.moduloAcceso = newfila;

                valueLista.Add(item);
            }

            return valueLista;
        }

        private string getAmbiente()
        {
            string value="";
            string cnBD = "";
            cnBD = this.cnBD.Substring(12, 11).ToString();     

            switch (cnBD)
            {
                case "10.10.10.40":
                    value = "DESARROLLO";
                    break;

                case "10.10.10.41":
                    value = "QAS";
                    break;

                case "10.10.10.43":
                    value = "PRODUCCION";
                    break;
                default:                   
                    break;
            }
            return value;

        }

        private string getLinkApp()
        {
            string value = "";
            string cnBD = "";
            cnBD = this.cnBD.Substring(12, 11).ToString();

            switch (cnBD)
            {
                case "10.10.10.40"://dev
                    value = "http://10.10.10.40:8090/";
                    break;

                case "10.10.10.41"://qua
                    value = "http://10.10.10.41:8090/";
                    break;

                case "10.10.10.43"://prod
                    value = "http://10.10.10.44:8090/";
                    break;
                default:
                    break;
            }
            return value;

        }

        public DTO.Result.Result<int> registroCliente(DTO.PersonaAgregarDTO dto)
        {
            DTO.Result.Result<int> result = new DTO.Result.Result<int>();
            string mensaje = "";

            /*if (validarAgregar(dto, out mensaje) == false)
            {
                result.errorMensaje = mensaje;
                result.errorCodigo = "VAL";
                return result;
            }*/

            DTO.Result.UsuarioDTO usuario = getAll().Where(u => u.login.ToUpper() == dto.login.ToUpper()).FirstOrDefault();

            if (usuario != null)
            {
                result.errorMensaje = "El código ingresado ya se encuentra registrado";
                result.errorCodigo = "VAL";
                return result;
            }

            usuario = getAll().Where(u => u.numeroDocumento.ToUpper() == dto.numeroDocumento.ToUpper()).FirstOrDefault();

            if (usuario != null)
            {
                result.errorMensaje = "El documento ingresado ya se encuentra registrado";
                result.errorCodigo = "VAL";
                return result;
            }

            var entity = mapper.Map<Entity.PersonaBE>(dto);
            //salida json
            int value = -1;
            objData = new Data.UsuarioDA(this.cnBD);
            value = objData.registroCliente(entity);

            if (value < 1)
            {
                result.errorMensaje = "La operación no se realizo correctamente";
                result.errorCodigo = "ERROR";
                return result;
            }

            result.value = value;
            return result;

        }

        public DTO.Result.Result<int> registroPerfilCli(DTO.PersonaAgregarDTO dto, int idUsuario)
        {
            DTO.Result.Result<int> result = new DTO.Result.Result<int>();
            string mensaje = "";

            /*if (validarAgregar(dto, out mensaje) == false)
            {
                result.errorMensaje = mensaje;
                result.errorCodigo = "VAL";
                return result;
            }*/

            var entity = mapper.Map<Entity.PersonaBE>(dto);
            //salida json
            int value = -1;
            objData = new Data.UsuarioDA(this.cnBD);
            value = objData.registroPerfilCli(entity, idUsuario);

            if (value < 1)
            {
                result.errorMensaje = "La operación no se realizo correctamente";
                result.errorCodigo = "ERROR";
                return result;
            }

            result.value = value;
            return result;

        }

        public DTO.Result.Result<int> modificaPerfilCli(DTO.PersonaModificarDTO dto)
        {
            DTO.Result.Result<int> result = new DTO.Result.Result<int>();
            objData = new Data.UsuarioDA(this.cnBD);

            var entity = mapper.Map<Entity.PersonaBE>(dto);

            string mensaje = "";

            /*if (validarModificar(dto, out mensaje) == false)
            {
                result.errorMensaje = mensaje;
                result.errorCodigo = "VAL";
                return result;
            }*/
            //salida json
            int value = -1;
            value = objData.modificaPerfilCli(entity);


            if (value < 1)
            {
                result.errorMensaje = "La operación no se realizo correctamente";
                result.errorCodigo = "ERROR";
                return result;
            }

            result.value = value;
            return result;

        }

        public DTO.PersonaAgregarDTO getPerfil(int id)
        {
            objData = new Data.UsuarioDA(this.cnBD);
            Entity.PersonaBE lista = new Entity.PersonaBE();
            lista = objData.listarPerfil(id);

            return mapper.Map<DTO.PersonaAgregarDTO>(lista);

        }

    }
}
