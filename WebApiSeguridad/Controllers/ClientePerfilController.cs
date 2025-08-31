using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace WebApiSeguridad.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientePerfilController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string cnBD = "";
        private Service.ClientePerfilSER objBss;
        private readonly IMapper mapper;

        public ClientePerfilController(IConfiguration configuracion, IMapper mapper)
        {
            this._configuration = configuracion;
            this.mapper = mapper;
            this.cnBD = this._configuration.GetConnectionString("cn_bd_sige");
        }

        [HttpGet]
        [Route("listarAll")]
        public IActionResult getAll(int idUsuario)
        {
            List<DTO.ClientePerfilDTO> value = new List<DTO.ClientePerfilDTO>();

            objBss = new Service.ClientePerfilSER(this.cnBD, this.mapper);
            value = objBss.getAll(idUsuario);

            if (value != null)
            {
                return Ok(value);
            }
            else
            {
                return NotFound();
            }

        }

    }
}
