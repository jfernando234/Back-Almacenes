using AutoMapper;
using DTO;
using Entity;

namespace Common
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<OrdenCompraAgregarDTO, OrdenCompraBE>();
            CreateMap<OrdenCompraModificarDTO, OrdenCompraBE>();
            CreateMap<OrdenCompraBE, OrdenCompraListarDTO>();
        }
    }
}