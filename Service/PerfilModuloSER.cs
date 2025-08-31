using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class PerfilModuloSER
    {
        private Data.PerfilModuloDA objData;
        private readonly string cnBD = "";
        private readonly IMapper mapper;

        public PerfilModuloSER(string cnBD, IMapper mapper)
        {
            this.cnBD = cnBD;
            this.mapper = mapper;
        }

        public List<DTO.Result.PerfilModuloDTO> getAll()
        {
            objData = new Data.PerfilModuloDA(this.cnBD);
            List<Entity.PerfilModuloBE> lista = new List<Entity.PerfilModuloBE>();

            lista = objData.listar();

            return mapper.Map<List<DTO.Result.PerfilModuloDTO>>(lista);

        }

        public List<DTO.Result.PerfilModuloDTO> listarPerfilModulo(int idPerfil )
        {
            
            return getAll().Where(x => x.idPerfil == idPerfil).ToList();            

        }

        public DTO.Result.PerfilModuloDTO get(int id)
        {
            objData = new Data.PerfilModuloDA(this.cnBD);
            Entity.PerfilModuloBE fila = new Entity.PerfilModuloBE();
            fila = objData.listar(id);

            return mapper.Map<DTO.Result.PerfilModuloDTO>(fila);

        }

        private bool validarAgregar(DTO.PerfilModuloAgregarDTO dto, out string mensaje)
        {
            bool value = false;
            List<DTO.Result.PerfilModuloDTO> listaDTO = getAll();

            //validar, el id perfil no repita x id modulo
            if (listaDTO.Where(x => x.idPerfil == dto.idPerfil && x.idModulo==dto.idModulo).Count() > 0)
            {
                mensaje = "El codigo del módulo ya se encuentra asignado al perfil";
                return value;
            }            

            mensaje = "";
            return true;
        }

        public DTO.Result.Result<int> agregar(DTO.PerfilModuloAgregarDTO dto)
        {

            DTO.Result.Result<int> result = new DTO.Result.Result<int>();
            string mensaje = "";

            if (validarAgregar(dto, out mensaje) == false)
            {
                result.errorMensaje = mensaje;
                result.errorCodigo = "VAL";
                return result;
            }

            var entity = mapper.Map<Entity.PerfilModuloBE>(dto);

            //salida json
            int value = -1;            
            objData = new Data.PerfilModuloDA(this.cnBD);
            value = objData.agregar(entity);

            if (value < 1)
            {
                result.errorMensaje = "La operación no se realizo correctamente";
                result.errorCodigo = "ERROR";
                return result;
            }

            result.value = value;
            return result;

        }

        public DTO.Result.Result<int> agregarModificar(DTO.PerfilModuloModificarDTO dto)
        {

            DTO.Result.Result<int> result = new DTO.Result.Result<int>();            
            int value = -1;

            List<DTO.Result.PerfilModuloDTO> listaDTO = getAll();

            //validar, el id perfil no repita x id modulo
            DTO.Result.PerfilModuloDTO dtoModificar=null;
            if (listaDTO.Where(x => x.idPerfil == dto.idPerfil && x.idModulo == dto.idModulo).Count() > 0)
            {
                dtoModificar = listaDTO.Where(x => x.idPerfil == dto.idPerfil && x.idModulo == dto.idModulo
                                                                ).FirstOrDefault();
                dto.idPerfilModulo = dtoModificar.idPerfilModulo;
            }

            if (dtoModificar != null)
            {                
                var dtoPerfilModulo = mapper.Map<DTO.PerfilModuloModificarDTO>(dto);
                dtoPerfilModulo.pcHost = dto.pcHost;
                dtoPerfilModulo.pcIp = dto.pcIp;
                dtoPerfilModulo.idUsuarioLogin = dto.idUsuarioLogin;

                result = modificar(dtoPerfilModulo);
                value = result.value;

                //si no se modifico correctamente
                if (value < 1)
                {                  
                    return result;
                }
            }
            else
            {
                var entity = mapper.Map<Entity.PerfilModuloBE>(dto);

                //salida json                
                objData = new Data.PerfilModuloDA(this.cnBD);
                value = objData.agregar(entity);

                if (value < 1)
                {
                    result.errorMensaje = "La operación no se realizo correctamente";
                    result.errorCodigo = "ERROR";
                    return result;
                }
            }           

            result.value = value;
            return result;

        }

        public DTO.Result.Result<int> modificar(DTO.PerfilModuloModificarDTO dto)
        {
            objData = new Data.PerfilModuloDA(this.cnBD);

            var entity = mapper.Map<Entity.PerfilModuloBE>(dto);

            //salida json
            int value = -1;
            DTO.Result.Result<int> result = new DTO.Result.Result<int>();
            value = objData.modificar(entity);

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
            var entity = mapper.Map<Entity.PerfilModuloBE>(dto);

            //buscar el registro
            DTO.Result.PerfilModuloDTO registro = get(dto.id);

            if (registro == null)
            {
                result.errorMensaje = "El registro que desea eliminar no existe";
                result.errorCodigo = "VAL";
                return result;
            }

            //salida json
            int value = -1;
            objData = new Data.PerfilModuloDA(this.cnBD);
            entity.idPerfilModulo=dto.id;
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
    }
}
