using AutoMapper;
using DTO;
using Entity;

namespace WebApiSeguridad
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Cliente mappings
            CreateMap<ClienteAgregarDTO, ClienteBE>();
            CreateMap<ClienteModificarDTO, ClienteBE>();
            CreateMap<ClienteBE, ClienteListarDTO>();

            // Proveedor mappings
            CreateMap<ProveedorAgregarDTO, ProveedorBE>();
            CreateMap<ProveedorModificarDTO, ProveedorBE>();
            CreateMap<ProveedorBE, ProveedorListarDTO>();

            // Producto mappings
            CreateMap<ProductoAgregarDTO, ProductoBE>();
            CreateMap<ProductoModificarDTO, ProductoBE>();
            CreateMap<ProductoBE, ProductoListarDTO>();
        }
    }
}