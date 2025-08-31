using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class UsuarioRolSER
    {
        private Data.UsuarioRolDA objData;
        private readonly string cnBD = "";
        private readonly IMapper mapper;

        public UsuarioRolSER(string cnBD, IMapper mapper)
        {
            this.cnBD = cnBD;
            this.mapper = mapper;
        }

        public List<DTO.Result.UsuarioRolDTO> getAll(int idUsuario)
        {
            objData = new Data.UsuarioRolDA(this.cnBD);
            List<Entity.UsuarioRolBE> lista = new List<Entity.UsuarioRolBE>();

            lista = objData.listar(idUsuario);

            return mapper.Map<List<DTO.Result.UsuarioRolDTO>>(lista);

        }

        public List<DTO.Result.UsuarioRolDetDTO> getDetalleAll(int idRol, int idRolUsuario)
        {
            objData = new Data.UsuarioRolDA(this.cnBD);
            List<Entity.UsuarioRolDetBE> lista = new List<Entity.UsuarioRolDetBE>();

            lista = objData.listarDetalle(idRol, idRolUsuario);

            return mapper.Map<List<DTO.Result.UsuarioRolDetDTO>>(lista);

        }

        public DTO.Result.Result<int> agregar(string opcion, DTO.Result.UsuarioRolNuevoDTO dto)
        {
            DTO.Result.Result<int> result = new DTO.Result.Result<int>();
            string mensaje = "";
            int idUsuarioRol = 0;
           
            var entity = mapper.Map<Entity.UsuarioRolBE>(dto);
            //var entityDet = mapper.Map<Entity.UsuarioRolDetBE>(dto.rolDetalle);            
            entity.idUsuarioRol = dto.idUsuarioRol;
            entity.idUsuarioPerfil = dto.idUsuarioPerfil;
            entity.idRolModulo = dto.idRolModulo;
            entity.idUsuarioLogin = dto.idUsuarioLogin;
            entity.pcIp = dto.pcIp;
            entity.pcHost = dto.pcHost;
            //salida json
            int value = -1;
            objData = new Data.UsuarioRolDA(this.cnBD);
            value = objData.mantenimientoUsp(entity);
            idUsuarioRol = value;
            if (value < 1)
            {
                result.errorMensaje = "La operación no se realizo correctamente";
                result.errorCodigo = "ERROR";
                return result;
            }
            if (dto.rolDetalle != null) { 
            if (dto.rolDetalle.Count>0)
            {
                foreach (DTO.Result.UsuarioRolDetDTO item in dto.rolDetalle){
                    var entityDet = mapper.Map<Entity.UsuarioRolDetBE>(item);

                    entityDet.idUsuarioRolDet = item.idUsuarioRolDet;
                    entityDet.sel = item.sel;
                    entityDet.idUsuarioLogin = dto.idUsuarioLogin;
                    entityDet.pcIp = dto.pcIp;
                    entityDet.pcHost = dto.pcHost;
                    //salida json
                    value = -1;
                    objData = new Data.UsuarioRolDA(this.cnBD);
                    value = objData.mantenimientoDetUsp(idUsuarioRol, entityDet);
                    if (value < 0)
                    {
                        result.errorMensaje = "La operación detalle no se realizo correctamente";
                        result.errorCodigo = "ERROR";
                        return result;
                    }
                }
            }
            }
            /*entity.idProducto = idProducto;
            value = objData.altaProductoUsp(entity.idProducto);
            if (value < 1)
            {
                result.errorMensaje = "La alta de Producto no se realizó correctamente";
                result.errorCodigo = "ERROR";
                return result;
            }*/

            result.value = value;
            return result;
        }

    }
}
