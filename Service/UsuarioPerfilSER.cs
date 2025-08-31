using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public  class UsuarioPerfilSER
    {
        private Data.UsuarioPerfilDA objData;
        private readonly string cnBD = "";
        private readonly IMapper mapper;

        public UsuarioPerfilSER(string cnBD, IMapper mapper)
        {
            this.cnBD = cnBD;
            this.mapper = mapper;
        }

        public List<DTO.Result.UsuarioPerfilDTO> getAll()
        {
            objData = new Data.UsuarioPerfilDA(this.cnBD);
            List<Entity.UsuarioPerfilBE> lista = new List<Entity.UsuarioPerfilBE>();

            lista = objData.listar();

            return mapper.Map<List<DTO.Result.UsuarioPerfilDTO>>(lista);

        }

        public List<DTO.Result.UsuarioPerfilDTO> listarxUsuario(int idUsuario)
        {
            
            return getAll().Where(x=>x.idUsuario== idUsuario).ToList();            

        }

        public DTO.Result.UsuarioPerfilDTO get(int id)
        {
            objData = new Data.UsuarioPerfilDA(this.cnBD);
            Entity.UsuarioPerfilBE fila = new Entity.UsuarioPerfilBE();
            fila = objData.listar(id);

            return mapper.Map<DTO.Result.UsuarioPerfilDTO>(fila);

        }

        private bool validar(DTO.UsuarioPerfilAgregarDTO dto, out string mensaje)
        {
            bool value = false;
            List<DTO.Result.UsuarioPerfilDTO> listaDTO = getAll();

            //validar, el id del usuario no repita x id del perfil
            if (listaDTO.Where(x => x.idUsuario == dto.idUsuario && x.idPerfil == dto.idPerfil).Count() > 0)
            {
                mensaje = "El codigo del perfil ya se encuentra asignado al Usuario";
                return value;
            }

            mensaje = "";
            return true;
        }

        public DTO.Result.Result<int> agregar(DTO.UsuarioPerfilAgregarDTO dto)
        {

            DTO.Result.Result<int> result = new DTO.Result.Result<int>();
            string mensaje = "";

            if (validar(dto, out mensaje) == false)
            {
                result.errorMensaje = mensaje;
                result.errorCodigo = "VAL";
                return result;
            }

            var entity = mapper.Map<Entity.UsuarioPerfilBE>(dto);

            //salida json
            int value = -1;            
            objData = new Data.UsuarioPerfilDA(this.cnBD);
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

        public DTO.Result.Result<int> modificar(DTO.UsuarioPerfilModificarDTO dto)
        {
            objData = new Data.UsuarioPerfilDA(this.cnBD);

            var entity = mapper.Map<Entity.UsuarioPerfilBE>(dto);

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
            var entity = mapper.Map<Entity.UsuarioPerfilBE>(dto);

            //buscar el registro
            DTO.Result.UsuarioPerfilDTO registro = get(dto.id);

            if (registro == null)
            {
                result.errorMensaje = "El registro que desea eliminar no existe";
                result.errorCodigo = "VAL";
                return result;
            }

            //salida json
            int value = -1;
            objData = new Data.UsuarioPerfilDA(this.cnBD);
            entity.idUsuarioPerfil = dto.id;
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
