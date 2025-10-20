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
    public class CompraSER
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly string cnBD = "";

        public CompraSER(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;
            cnBD = _configuration.GetConnectionString("cn_bd_sige");
        }
        public int agregar (CompraAgregarDTO dto)
        {
            try
            {
                var da = new CompraDA(cnBD);
                // Crear manualmente la entidad
                var entidad = new CompraBE
                {
                    TipoDocumento = dto.TipoDocumento,
                    ProveedorId = dto.ProveedorId,
                    TipoCompra = dto.TipoCompra,
                    Observacion = dto.Observacion,
                    ProductoId = dto.ProductoId,
                    Igv = dto.Igv,
                    PrecioUnitario = dto.PrecioUnitario,
                    SubTotal = dto.SubTotal,
                    FechaRegistro = dto.Fecharegistro,
                    idUsuarioLogin = dto.idUsuarioLogin,
                    pcIp = dto.pcIp,
                    pcHost = dto.pcHost
                };
                return da.agregar(entidad);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al agregar compra: {ex.Message}", ex);
            }
        }
    }
}
