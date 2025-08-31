using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using DTO;
using DTO.Result;

namespace Service
{
    public class RolService
    {
        private Data.RolDA objData;
        private readonly string cnBD = "";
        private readonly IMapper mapper;

        public RolService(string cnBD, IMapper mapper)
        {
            this.cnBD = cnBD;
            this.mapper = mapper;
        }

        public List<DTO.Result.RolDTO> getAll()
        {
            objData = new Data.RolDA(this.cnBD);
            List<Entity.RolBE> lista = new List<Entity.RolBE>();

            lista = objData.listar();

            return mapper.Map<List<DTO.Result.RolDTO>>(lista);

        }

        public DTO.Result.RolDTO get(int id)
        {
            objData = new Data.RolDA(this.cnBD);
            Entity.RolBE lista = new Entity.RolBE();
            lista = objData.listar(id);

            return mapper.Map<DTO.Result.RolDTO>(lista);
        }

        private bool validarAgregar(DTO.RolAgregarDTO dto, out string mensaje)
        {
            bool value = false;
            List<DTO.Result.RolDTO> listaDTO = getAll();

            //validar, nombre del rol que no se repita x modulo
            if (listaDTO.Where(x => x.idModulo == dto.idModulo && x.nombreRol.ToUpper() == dto.nombreRol.ToUpper()).Count() > 0)
            {
                mensaje = "El nombre del rol ya se encuentra asignado al Módulo";
                return value;
            }

            mensaje = "";
            return true;
        }

        private bool validarModificar(RolModificarDTO dto, out string mensaje)
        {
            bool value = false;
            List<RolDTO> listaDTO = getAll();

            //validar, nombre del rol que no se repita x modulo
            if (listaDTO.Where(x => x.idModulo == dto.idModulo && x.nombreRol.ToUpper() == dto.nombreRol.ToUpper() 
                & x.idRol != dto.idRol).Count() > 0)
            {
                mensaje = "El nombre del rol ya se encuentra asignado al Módulo";
                return value;
            }

            mensaje = "";
            return true;
        }

        public DTO.Result.Result<int> agregar(DTO.RolAgregarDTO dto)
        {

            DTO.Result.Result<int> result = new DTO.Result.Result<int>();
            string mensaje = "";

            //validar registro
            if (validarAgregar(dto, out mensaje) == false)
            {
                result.errorMensaje = mensaje;
                result.errorCodigo = "VAL";
                return result;
            }

            var entity = mapper.Map<Entity.RolBE>(dto);

            //salida json
            int value = -1;            
            objData = new Data.RolDA(this.cnBD);
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

        public DTO.Result.Result<int> modificar(DTO.RolModificarDTO dto)
        {

            DTO.Result.Result<int> result = new DTO.Result.Result<int>();

            //validar si el registro existe
            RolDTO dtoExiste = get(dto.idRol);

            if (dtoExiste == null)
            {
                result.errorMensaje = "El registro no existe en la Base de Datos.";
                result.errorCodigo = "VAL";
                return result;
            }

            //validar modificacion del registro           
            string mensaje = "";
            if (validarModificar(dto, out mensaje) == false)
            {
                result.errorMensaje = mensaje;
                result.errorCodigo = "VAL";
                return result;
            }

            //salida json
            int value = -1;
            objData = new Data.RolDA(this.cnBD);
            var entity = mapper.Map<Entity.RolBE>(dto);

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
            var entity = mapper.Map<Entity.RolBE>(dto);

            //buscar el registro            
            DTO.Result.RolDTO registro = get(dto.id);

            if (registro == null)
            {
                result.errorMensaje = "El registro que desea eliminar no existe";
                result.errorCodigo = "VAL";
                return result;
            }

            //salida json
            int value = -1;
            objData = new Data.RolDA(this.cnBD);
            entity.idRol=dto.id;
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
