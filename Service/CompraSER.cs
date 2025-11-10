using AutoMapper;
using Data;
using DTO;
using Entity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class CompraSer
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly string cnBD = "";

        public CompraSer(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;
            cnBD = _configuration.GetConnectionString("cn_bd_sige");
        }
        public List<DTO.CompraListarDTO> listarAll()
        {
            try
            {
                var da = new CompraDA(cnBD);
                var lista = da.listarAll();
                var dto = _mapper.Map<List<DTO.CompraListarDTO>>(lista);
                return dto;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al listar productos: {ex.Message}", ex);
            }
        }
        public int Agregar(CompraAgregarDTO dto)
        {
            try
            {
                var da = new CompraDA(cnBD);

                //var entidad = _mapper.Map<VentaBE>(dto);
                
                var entidad = new CompraBE
                {
                    TipoDocumentoId = dto.TipoDocumentoId,
                    Ruc = dto.Ruc,
                    RazonSocial = dto.RazonSocial,
                    Observacion = dto.Observacion,
                    TipoCompraId = dto.TipoCompraId,
                    Total = dto.Total,
                    FechaRegistro = dto.FechaRegistro,
                    idUsuarioLogin = dto.idUsuarioLogin,
                    pcIp = dto.pcIp,
                    pcHost = dto.pcHost,
                    Detalles = dto.Detalles.Select(d => new DetalleCompraBE
                    {
                        ProductoId = d.ProductoId,
                        Cantidad = d.Cantidad,
                        PrecioUnitario = d.PrecioUnitario,
                        Total = d.Total
                    }).ToList()
                };

                return da.agregar(entidad);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al agregar venta: {ex.Message}", ex);
            }
        }
        public List<DTO.CompraPorMesDTO> ObtenerComprasPorMes()
        {
            try
            {
                var da = new CompraDA(cnBD);
                var lista = da.ObtenerComprasPorMes(); // Lo implementamos en el DA
                var dto = _mapper.Map<List<DTO.CompraPorMesDTO>>(lista);
                return dto;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener compras por mes: {ex.Message}", ex);
            }
        }

    }
}
