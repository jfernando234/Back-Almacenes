using AutoMapper;
using DTO;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class AlmacenSER
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly string cnBD = "";

        public AlmacenSER(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;
            cnBD = _configuration.GetConnectionString("");
        }
        

    }
}
