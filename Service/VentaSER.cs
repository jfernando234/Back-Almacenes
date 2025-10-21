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
    public class VentaSER
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly string cnBD = "";

        public VentaSER(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;
            cnBD = _configuration.GetConnectionString("cn_bd_sige");
        }
        public int Agregar(VentaAgregarDTO dto)
        {
            try
            {
                var da = new VentaDA(cnBD);

                var entidad = new VentaBE
                {
                    TipoDocumentoId = dto.TipoDocumentoId,
                    FechaRegistro = dto.FechaRegistro,
                    TipoPagoId = dto.TipoPagoId,
                    Dni = dto.Dni,
                    NombreCliente = dto.NombreCliente,
                    ApellidosCliente = dto.ApellidosCliente,
                    Observacion = dto.Observacion,
                    TipoMetodoPagoId = dto.TipoMetodoPagoId,
                    TipoTarjetaId = dto.TipoTarjetaId,
                    MontoRecibido = dto.MontoRecibido,
                    Vuelto = dto.Vuelto,
                    SubTotal = dto.SubTotal,
                    Igv = dto.Igv,
                    Total = dto.Total,
                    idUsuarioLogin = dto.idUsuarioLogin,
                    pcIp = dto.pcIp,
                    pcHost = dto.pcHost,
                    Detalles = dto.Detalles.Select(d => new DetalleVentaBE
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
    }
}
