using System;
using System.Collections.Generic;
using AutoMapper;
using Data;
using DTO;
using Entity;
using Microsoft.Extensions.Configuration;

namespace Service
{
    public class OrdenCompraSER
    {
        private readonly string cnBD;
        private readonly IMapper _mapper;
        private OrdenCompraDA objDA;

        public OrdenCompraSER(IConfiguration configuration, IMapper mapper)
        {
            cnBD = configuration.GetConnectionString("cn_bd_sige");
            _mapper = mapper;
        }

        public List<OrdenCompraListarDTO> listar()
        {
            try
            {
                var ordenCompraDA = new OrdenCompraDA(cnBD);
                var ordenes = ordenCompraDA.listar();
                return _mapper.Map<List<OrdenCompraListarDTO>>(ordenes);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al listar órdenes de compra: {ex.Message}", ex);
            }
        }

        public OrdenCompraListarDTO listar(int id)
        {
            try
            {
                var ordenCompraDA = new OrdenCompraDA(cnBD);
                var orden = ordenCompraDA.listar(id);
                return _mapper.Map<OrdenCompraListarDTO>(orden);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener orden de compra: {ex.Message}", ex);
            }
        }

        public List<OrdenCompraListarDTO> filtrar(DateTime inicio, DateTime fin)
        {
            try
            {
                var ordenCompraDA = new OrdenCompraDA(cnBD);
                var ordenes = ordenCompraDA.filtrar(inicio, fin);
                return _mapper.Map<List<OrdenCompraListarDTO>>(ordenes);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al filtrar órdenes de compra: {ex.Message}", ex);
            }
        }

        public int agregar(OrdenCompraAgregarDTO ordenCompraDTO)
        {
            try
            {
                var ordenCompraDA = new OrdenCompraDA(cnBD);
                var ordenCompraBE = _mapper.Map<OrdenCompraBE>(ordenCompraDTO);
                return ordenCompraDA.agregar(ordenCompraBE);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al agregar orden de compra: {ex.Message}", ex);
            }
        }

        public int modificar(OrdenCompraModificarDTO ordenCompraDTO)
        {
            try
            {
                if (ordenCompraDTO.idOrdenCompra <= 0)
                    throw new ArgumentException("El ID de la orden de compra debe ser mayor a 0");

                var ordenCompraDA = new OrdenCompraDA(cnBD);

                var ordenExistente = ordenCompraDA.listar(ordenCompraDTO.idOrdenCompra);
                if (ordenExistente == null)
                {
                    throw new Exception("Orden de compra no encontrada");
                }

                var ordenCompraBE = _mapper.Map<OrdenCompraBE>(ordenCompraDTO);

                return ordenCompraDA.modificar(ordenCompraBE);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al modificar orden de compra: {ex.Message}", ex);
            }
        }

        public int deshabilitar(int id, string pcIp, string pcHost, int idUsuarioLogin)
        {
            try
            {
                var ordenCompraDA = new OrdenCompraDA(cnBD);

                var ordenExistente = ordenCompraDA.listar(id);
                if (ordenExistente == null)
                {
                    throw new Exception("Orden de compra no encontrada");
                }

                return ordenCompraDA.deshabilitar(id, pcIp, pcHost, idUsuarioLogin);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al deshabilitar orden de compra: {ex.Message}", ex);
            }
        }
    }
}
