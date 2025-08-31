using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using DTO.Result;
using DTO;

namespace Service
{
    public class ModuloSER
    {
        private Data.ModuloDA objData;
        private readonly string cnBD = "";
        private readonly IMapper mapper;

        public ModuloSER(string cnBD, IMapper mapper)
        {
            this.cnBD = cnBD;
            this.mapper = mapper;
        }

        public List<DTO.Result.ModuloDTO> getAll()
        {
            objData = new Data.ModuloDA(this.cnBD);
            List<Entity.ModuloBE> lista = new List<Entity.ModuloBE>();

            lista = objData.listar();

            //quitar el ultimo nivel de la dependencia
            foreach (Entity.ModuloBE item in lista)
            {
                int total = item.nombreModuloPadre.Length;
                int quitar = item.nombreModulo.Length;
                int add = 0;

                if (total != quitar)
                {
                    add = 1;
                }

                item.nombreModuloPadre = item.nombreModuloPadre.Substring(0, total - quitar - add).ToUpper();
            }

            return mapper.Map<List<DTO.Result.ModuloDTO>>(lista);

        }

        public DTO.Result.ModuloDTO get(int id)
        {

            return getAll().Where(x => x.idModulo == id).FirstOrDefault();            

        }

        private bool validarAgregar(ModuloAgregarDTO dto, out string mensaje)
        {
            bool value = false;

            List<ModuloDTO> listDto = getAll();

            //validar el nombre
            if (listDto.Where(x => x.nombreModulo.ToUpper() == dto.nombreModulo.ToUpper() & x.idModuloPadre ==dto.idModuloPadre ).Count() > 0)
            {
                mensaje = "El registro: [" + dto.nombreModulo + "] ya existe en la base de datos.";
                return value;
            }

            mensaje = "";
            return true;
        }

        private bool validarModificar(ModuloModificarDTO dto, out string mensaje)
        {
            bool value = false;

            //validar si el registro existe
            ModuloDTO dtoExiste = get(dto.idModulo);

            if (dtoExiste == null)
            {
                mensaje = "El registro no existe en la Base de Datos.";                
                return value;
            }

            List<ModuloDTO> listaDto = getAll();

            //validar el nombre
            if (listaDto.Where(x => x.nombreModulo.ToUpper() == dto.nombreModulo.ToUpper() & x.idModuloPadre == dto.idModuloPadre  
                        & x.idModulo != dto.idModulo).Count() > 0)
            {
                mensaje = "El registro: [" + dto.nombreModulo + "] ya existe en la base de datos.";
                return value;
            }

            //validar el que el registro no sea padre e hijo al mismo tiempo
            if (dto.idModuloPadre == dto.idModulo)
            {
                mensaje = "El registro: [" + dto.nombreModulo + "] no puede generar una dependencia de si mismo.";
                return value;
            }

            //validar, que no exista el registro modificado, tenga niveles
            if (listaDto.Where(x => x.idModulo == dto.idModulo & x.idModuloPadre != dto.idModuloPadre).Count() > 0)
            {
                if (listaDto.Where(x => x.idModuloPadre == dto.idModulo).Count() > 0)
                {
                    mensaje = "El registro: [" + dto.nombreModulo + "] no se puede editar, porque tiene registros dependientes.";
                    return value;
                }
            }

            mensaje = "";
            return true;
        }

        public DTO.Result.Result<int> agregar(DTO.ModuloAgregarDTO dto)
        {
            DTO.Result.Result<int> result = new DTO.Result.Result<int>();
            
            //validar registro
            string mensaje = "";
            if (validarAgregar(dto, out mensaje) == false)
            {
                result.errorMensaje = mensaje;
                result.errorCodigo = "VAL";
                return result;
            }

            var entity = mapper.Map<Entity.ModuloBE>(dto);

            //salida json
            int value = -1;
            objData = new Data.ModuloDA(this.cnBD);

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

        public DTO.Result.Result<int> modificar(DTO.ModuloModificarDTO dto)
        {
            DTO.Result.Result<int> result = new DTO.Result.Result<int>();                     

            //validar modificacion del registro
            string mensaje = "";
            if (validarModificar(dto, out mensaje) == false)
            {
                result.errorMensaje = mensaje;
                result.errorCodigo = "VAL";
                return result;
            }

            var entity = mapper.Map<Entity.ModuloBE>(dto);

            //salida json
            int value = -1;
            objData = new Data.ModuloDA(this.cnBD);

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
            var entity = mapper.Map<Entity.ModuloBE>(dto);

            //buscar el registro
            DTO.Result.ModuloDTO registro = get(dto.id);

            if (registro == null)
            {
                result.errorMensaje = "El registro que desea eliminar no existe";
                result.errorCodigo = "VAL";
                return result;
            }

            //salida json
            int value = -1;
            objData = new Data.ModuloDA(this.cnBD);
            entity.idModulo = dto.id;
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
