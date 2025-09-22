using AutoMapper;
using Data;
using DTO;
using Entity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace Service
{
    public class ProductoSER
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly string cnBD = "";

        public ProductoSER(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;
            cnBD = _configuration.GetConnectionString("cn_bd_sige");
        }

        public List<DTO.ProductoListarDTO> listarAll()
        {
            try
            {
                var da = new ProductoDA(cnBD);
                var lista = da.listarAll();
                var dto = _mapper.Map<List<DTO.ProductoListarDTO>>(lista);
                return dto;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al listar productos: {ex.Message}", ex);
            }
        }

        public ProductoBE obtener(int id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("El ID del producto debe ser mayor a 0");

                var da = new ProductoDA(cnBD);
                var producto = da.listar(id);
                if (producto == null)
                    throw new Exception("Producto no encontrado");

                return producto;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener producto: {ex.Message}", ex);
            }
        }

        public int agregar(ProductoAgregarDTO dto)
        {
            try
            {
                var da = new ProductoDA(cnBD);
                var entidad = _mapper.Map<ProductoBE>(dto);
                return da.agregar(entidad);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al agregar producto: {ex.Message}", ex);
            }
        }

        public int modificar(ProductoModificarDTO dto)
        {
            try
            {
                if (dto.ProductoId <= 0)
                    throw new ArgumentException("El ID del producto debe ser mayor a 0");

                var da = new ProductoDA(cnBD);
                var existente = da.listar(dto.ProductoId);
                if (existente == null)
                    throw new Exception("Producto no encontrado");

                var entidad = _mapper.Map<ProductoBE>(dto);
                return da.modificar(entidad);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al modificar producto: {ex.Message}", ex);
            }
        }

        public int deshabilitar(int id, string pcIp, string pcHost, int idUsuarioLogin)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("El ID del producto debe ser mayor a 0");

                var da = new ProductoDA(cnBD);
                var entidad = new ProductoBE
                {
                    ProductoId = id,
                    idUsuarioLogin = idUsuarioLogin,
                    pcIp = pcIp,
                    pcHost = pcHost
                };

                return da.deshabilitar(entidad);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al deshabilitar producto: {ex.Message}", ex);
            }
        }

        public List<DTO.ProductoListarDTO> filtro(DateTime? inicio, DateTime? fin, string nombre)
        {
            try
            {
                var da = new ProductoDA(cnBD);
                var lista = da.filtro(inicio, fin, nombre);
                var dto = _mapper.Map<List<DTO.ProductoListarDTO>>(lista);
                return dto;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al filtrar productos: {ex.Message}", ex);
            }
        }
    }
}
