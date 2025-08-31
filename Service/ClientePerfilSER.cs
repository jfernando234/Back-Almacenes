using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using DTO;
using DTO.Result;
using static System.Collections.Specialized.BitVector32;


namespace Service
{
    public class ClientePerfilSER
    {
        private Data.ClientePerfilDA objData;
        private readonly string cnBD = "";
        private readonly IMapper mapper;

        public ClientePerfilSER(string cnBD, IMapper mapper)
        {
            this.cnBD = cnBD;
            this.mapper = mapper;
        }

        public List<DTO.ClientePerfilDTO> getAll(int idUsuario)
        {
            objData = new Data.ClientePerfilDA(this.cnBD);
            List<Entity.ClientePerfilBE> lista = new List<Entity.ClientePerfilBE>();

            lista = objData.listar(idUsuario);

            return mapper.Map<List<DTO.ClientePerfilDTO>>(lista);

        }

    }
}
