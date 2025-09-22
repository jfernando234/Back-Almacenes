using AutoMapper;
using Data;
using DTO;
using Entity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace Service
{
    public class ProveedorSER
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly string cnBD = "";

        public ProveedorSER(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;
            cnBD = _configuration.GetConnectionString("cn_bd_sige");
        }

        public List<DTO.ProveedorListarDTO> listarAll()
        {
            try
            {
                var proveedorDA = new ProveedorDA(cnBD);
                var listaEntity = proveedorDA.listarAll();
                var listaDto = _mapper.Map<List<DTO.ProveedorListarDTO>>(listaEntity);
                return listaDto;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al listar todos los proveedores: {ex.Message}", ex);
            }
        }

        public ProveedorBE obtener(int id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("El ID del proveedor debe ser mayor a 0");

                var proveedorDA = new ProveedorDA(cnBD);
                var proveedor = proveedorDA.listar(id);

                if (proveedor == null)
                    throw new Exception("Proveedor no encontrado");

                return proveedor;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener proveedor: {ex.Message}", ex);
            }
        }

        public int agregar(ProveedorAgregarDTO proveedorDTO)
        {
            try
            {
                var proveedorDA = new ProveedorDA(cnBD);

                // Aquí podrías agregar validaciones específicas, e.g., RUC único

                var proveedorBE = _mapper.Map<ProveedorBE>(proveedorDTO);

                return proveedorDA.agregar(proveedorBE);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al agregar proveedor: {ex.Message}", ex);
            }
        }

        public int modificar(ProveedorModificarDTO proveedorDTO)
        {
            try
            {
                if (proveedorDTO.idProveedor <= 0)
                    throw new ArgumentException("El ID del proveedor debe ser mayor a 0");

                var proveedorDA = new ProveedorDA(cnBD);

                var proveedorExistente = proveedorDA.listar(proveedorDTO.idProveedor);
                if (proveedorExistente == null)
                    throw new Exception("Proveedor no encontrado");

                var proveedorBE = _mapper.Map<ProveedorBE>(proveedorDTO);

                return proveedorDA.modificar(proveedorBE);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al modificar proveedor: {ex.Message}", ex);
            }
        }

        public int deshabilitar(int id, string pcIp, string pcHost, int idUsuarioLogin)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("El ID del proveedor debe ser mayor a 0");

                var proveedorDA = new ProveedorDA(cnBD);

                var proveedorExistente = proveedorDA.listar(id);
                if (proveedorExistente == null)
                    throw new Exception("Proveedor no encontrado");

                var proveedorBE = new ProveedorBE
                {
                    idProveedor = id,
                    idUsuarioLogin = idUsuarioLogin,
                    pcIp = pcIp,
                    pcHost = pcHost
                };

                return proveedorDA.deshabilitar(proveedorBE);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al deshabilitar proveedor: {ex.Message}", ex);
            }
        }

        public List<DTO.ProveedorListarDTO> filtro(DateTime? inicio, DateTime? fin, string nombre)
        {
            try
            {
                var proveedorDA = new ProveedorDA(cnBD);
                var listaEntity = proveedorDA.filtro(inicio, fin, nombre);
                var listaDto = _mapper.Map<List<DTO.ProveedorListarDTO>>(listaEntity);
                return listaDto;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al aplicar filtro de proveedores: {ex.Message}", ex);
            }
        }
    }
}
