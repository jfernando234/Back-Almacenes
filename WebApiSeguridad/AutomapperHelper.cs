using AutoMapper;
using Entity;
using DTO;
using DTO.Result;

namespace WebApiSeguridad
{
    public class AutomapperHelper : Profile
    {
        public AutomapperHelper()
        {
            //var config = new MapperConfiguration(cfg =>cfg.CreateMap< Aplicativo, WebApiVersion  Aplicativo>());
            CreateMap<UsuarioBE, UsuarioDTO>();
            CreateMap<UsuarioBE, UsuarioLoginDTO>();
            CreateMap<UsuarioAgregarDTO, UsuarioBE>();
            CreateMap<UsuarioModificarDTO, UsuarioBE>();
            CreateMap<UsuarioCambiarClaveDTO, UsuarioBE>();
            CreateMap<EliminarDTO, UsuarioBE>();

            CreateMap<PerfilBE, PerfilDTO>();
            CreateMap<PerfilAgregarDTO, PerfilBE>();
            CreateMap<PerfilModificarDTO, PerfilBE>();
            CreateMap<EliminarDTO, PerfilBE>();

            CreateMap<ModuloBE, ModuloDTO>();
            CreateMap<ModuloAgregarDTO, ModuloBE>();
            CreateMap<ModuloModificarDTO, ModuloBE>();
            CreateMap<EliminarDTO, ModuloBE>();

            CreateMap<PerfilModuloBE, PerfilModuloDTO>();
            CreateMap<PerfilModuloAgregarDTO, PerfilModuloBE>();
            CreateMap<PerfilModuloModificarDTO, PerfilModuloBE>();            
            CreateMap<PerfilModuloDTO, PerfilModuloModificarDTO>();
            CreateMap<EliminarDTO, PerfilModuloBE>();

            CreateMap<UsuarioPerfilBE, UsuarioPerfilDTO>();
            CreateMap<UsuarioPerfilAgregarDTO, UsuarioPerfilBE>();
            CreateMap<UsuarioPerfilModificarDTO, UsuarioPerfilBE>();
            CreateMap<EliminarDTO, UsuarioPerfilBE>();

            CreateMap < PerfilModuloBE, UsuarioModuloDTO.ModuloAccesoDTO> ();
            CreateMap<ModuloDTO, UsuarioModuloDTO>();

            CreateMap<RolBE, RolDTO>();
            CreateMap<RolAgregarDTO, RolBE>();
            CreateMap<RolModificarDTO, RolBE>();
            CreateMap<EliminarDTO, RolBE>();

            CreateMap<UsuarioRolBE, UsuarioRolDTO>();
            CreateMap<UsuarioRolBE, UsuarioRolNuevoDTO>().ReverseMap();

            // Mapeos para Cliente
            CreateMap<ClienteBE, ClienteListarDTO>()
                .ForMember(dest => dest.fechaCreacion, opt => opt.MapFrom(src => src.fechaCreacion))
                .ForMember(dest => dest.estadoTexto, opt => opt.MapFrom(src => src.estadoTexto));
            CreateMap<ClienteAgregarDTO, ClienteBE>();
            CreateMap<ClienteModificarDTO, ClienteBE>();
            CreateMap<UsuarioRolDetBE, UsuarioRolDetDTO>().ReverseMap();
            CreateMap<UsuarioRolAgregarDTO, UsuarioRolBE>();
            CreateMap<UsuarioRolModificarDTO, UsuarioRolBE>();
            CreateMap<EliminarDTO, UsuarioRolBE>();

            CreateMap<UsuarioDTO, UsuarioyPerfilDTO>();
            CreateMap<PersonaAgregarDTO, PersonaBE>().ReverseMap();
            CreateMap<PersonaModificarDTO, PersonaBE>().ReverseMap();
            CreateMap<ClientePerfilDTO, ClientePerfilBE>().ReverseMap();

            CreateMap<PerfilcuentaBancariaDTO, PerfilcuentaBancariaBE>().ReverseMap();

            CreateMap<tipoCuentaBE, tipoCuentaDTO>().ReverseMap();
            CreateMap<monedaBE, monedaDTO>().ReverseMap();
            CreateMap<listacuentaBancariaDTO, PerfilcuentaBancariaBE>().ReverseMap();
            CreateMap<listacuentaBancariaNegDTO, PerfilcuentaBancariaBE>().ReverseMap();


            

            CreateMap<dashboardClienteBE, dashboardClienteDTO>().ReverseMap();

            // Cliente mappings
            CreateMap<ClienteAgregarDTO, ClienteBE>();
            CreateMap<ClienteModificarDTO, ClienteBE>();
            CreateMap<ClienteBE, ClienteAgregarDTO>();
            CreateMap<ClienteBE, ClienteModificarDTO>();

        }

    }
}
