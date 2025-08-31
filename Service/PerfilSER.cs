using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using DTO;
using DTO.Result;

namespace Service
{
    public  class PerfilSER
    {
        private Data.PerfilDA objData;        
        private readonly string cnBD = "";
        private readonly IMapper mapper;

        public PerfilSER(string cnBD, IMapper mapper)
        {
            this.cnBD = cnBD;
            this.mapper = mapper;
        }

        public List<DTO.Result.PerfilDTO> getAll()
        {
            objData = new Data.PerfilDA(this.cnBD);
            List<Entity.PerfilBE> lista = new List<Entity.PerfilBE>();

            lista = objData.listar();

            return mapper.Map<List<DTO.Result.PerfilDTO>>(lista);
            
        }

        public DTO.Result.PerfilDTO get(int id)
        {
            objData = new Data.PerfilDA(this.cnBD);
            Entity.PerfilBE lista = new Entity.PerfilBE();
            lista = objData.listar(id);            

            return mapper.Map<DTO.Result.PerfilDTO>(lista);
            
        }

        private bool validarAgregar(DTO.PerfilAgregarDTO dto, out string mensaje)
        {
            bool value = false;            
            List<DTO.Result.PerfilDTO> listaDTO = getAll();
   
            //validar nombre
            if (listaDTO.Where(x => x.nombrePerfil.ToUpper() == dto.nombrePerfil.ToUpper()).Count() > 0)
            {
                mensaje = "El nombre del perfil ya existe en la base de datos";
                return value;
            }            

            mensaje = "";
            return true;
        }

        private bool validarModificar(PerfilModificarDTO dto, out string mensaje)
        {
            bool value = false;

            List<PerfilDTO> listaDto = getAll();

            //validar el nombre del area
            if (listaDto.Where(x => x.nombrePerfil.ToUpper() == dto.nombrePerfil.ToUpper() & x.idPerfil != dto.idPerfil).Count() > 0)
            {
                mensaje = "El registro: [" + dto.nombrePerfil + "] ya existe en la base de datos.";
                return value;
            }

            mensaje = "";
            return true;
        }

        public DTO.Result.Result<int> agregar(DTO.PerfilAgregarDTO dto)
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

            var entity = mapper.Map<Entity.PerfilBE>(dto);

            //salida json
            int value = -1;            
            objData = new Data.PerfilDA(this.cnBD);
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

        public DTO.Result.Result<int> agregarPerfilModulo(DTO.PerfilAgregarPerfilModuloDTO dto)
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

            var entity = mapper.Map<Entity.PerfilBE>(dto);

            //salida json
            int value = -1;
            objData = new Data.PerfilDA(this.cnBD);
            value = objData.agregar(entity);

            if (value < 1)
            {
                result.errorMensaje = "La operación no se realizo correctamente";
                result.errorCodigo = "ERROR";
                return result;
            }

            /*grabar los registros de perfilModulo*/
            foreach (DTO.PerfilAgregarPerfilModuloDTO.PerfilModuloDTO fila in dto.listaPerfilModulo)
            {

                Service.PerfilModuloSER service = new Service.PerfilModuloSER(this.cnBD, this.mapper);
                DTO.PerfilModuloAgregarDTO dtoFila = new DTO.PerfilModuloAgregarDTO();

                dtoFila.idPerfil = value;
                dtoFila.idModulo = fila.idModulo;
                dtoFila.consultar = fila.consultar;
                dtoFila.agregar = fila.agregar;
                dtoFila.modificar = fila.modificar;
                dtoFila.eliminar = fila.eliminar;
                dtoFila.imprimir = fila.imprimir;
                dtoFila.descargarPDF=fila.descargarPDF;
                dtoFila.descargarExcel=fila.descargarExcel;
                /*auditoria*/
                dtoFila.idUsuarioLogin = dto.idUsuarioLogin;
                dtoFila.pcIp = dto.pcIp;
                dtoFila.pcHost = dto.pcHost;

                DTO.Result.Result<int> value2 = service.agregar(dtoFila);

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

        public DTO.Result.Result<int> modificar(DTO.PerfilModificarDTO dto)
        {

            DTO.Result.Result<int> result = new DTO.Result.Result<int>();

            //validar si el registro existe
            PerfilDTO dtoExiste = get(dto.idPerfil);

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

            var entity = mapper.Map<Entity.PerfilBE>(dto);            
            //salida json
            int value = -1;
            
            objData = new Data.PerfilDA(this.cnBD);
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
            var entity = mapper.Map<Entity.PerfilBE>(dto);

            //buscar el registro            
            DTO.Result.PerfilDTO registro = get(dto.id);

            if (registro == null)
            {
                result.errorMensaje = "El registro que desea eliminar no existe";
                result.errorCodigo = "VAL";
                return result;
            }

            //salida json
            int value = -1;
            objData = new Data.PerfilDA(this.cnBD);
            entity.idPerfil=dto.id;
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
