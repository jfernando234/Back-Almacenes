using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiSeguridad.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PerfilController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly string cnBD = "";
        private Service.PerfilSER objBss;
        private readonly IMapper mapper;

        public PerfilController(IConfiguration configuracion, IMapper mapper)
        {
            this._configuration = configuracion;
            this.mapper = mapper;
            this.cnBD = this._configuration.GetConnectionString("cn_bd_sige");
        }

        [HttpGet]
        [Route("obtenerFecha")]
        public string getFecha()
        {
            return DateTime.Now.ToString();
        }

        [HttpGet]
        [Route("listarAll")]
        public IActionResult getAll()
        {
            List<DTO.Result.PerfilDTO> value = new List<DTO.Result.PerfilDTO>();

            objBss = new Service.PerfilSER(this.cnBD,this.mapper);
            value = objBss.getAll();

            if (value != null)
            {
                return Ok(value);
            }
            else
            {
                return NotFound();
            }

        }

        [HttpGet]
        [Route("listar")]
        public IActionResult get(int id)
        {
            DTO.Result.PerfilDTO value = new DTO.Result.PerfilDTO();

            objBss = new Service.PerfilSER(this.cnBD, this.mapper);
            value = objBss.get(id);

            if (value != null)
            {
                return Ok(value);
            }
            else
            {
                return NotFound();
            }

        }

        [HttpPost]
        [Route("agregar")]
        public IActionResult agregar(DTO.PerfilAgregarDTO dto)
        {
            objBss = new Service.PerfilSER(this.cnBD, this.mapper);
            DTO.Result.Result<int> result = new DTO.Result.Result<int>();

            result = objBss.agregar(dto);           

            return Ok(result);
        }

        [HttpPost]
        [Route("agregarPerfilModulo")]
        public IActionResult agregarPerfilModulo(DTO.PerfilAgregarPerfilModuloDTO dto)
        {
            objBss = new Service.PerfilSER(this.cnBD, this.mapper);
            DTO.Result.Result<int> result = new DTO.Result.Result<int>();

            result = objBss.agregarPerfilModulo(dto);

            return Ok(result);
        }

        [HttpPut]
        [Route("modificar")]
        public IActionResult modificar(DTO.PerfilModificarDTO dto)
        {
            objBss = new Service.PerfilSER(this.cnBD, this.mapper);
            DTO.Result.Result<int> result = new DTO.Result.Result<int>();

            result = objBss.modificar(dto);
          
            return Ok(result);
        }

        [HttpPut]
        [Route("eliminar")]
        public IActionResult eliminar(DTO.EliminarDTO dto)
        {
            objBss = new Service.PerfilSER(this.cnBD, this.mapper);
            DTO.Result.Result<int> result = new DTO.Result.Result<int>();

            result = objBss.eliminar(dto);     
            
            return Ok(result);
        }
    }
}
