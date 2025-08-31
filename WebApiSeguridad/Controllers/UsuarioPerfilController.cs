using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace WebApiSeguridad.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioPerfilController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string cnBD = "";
        private Service.UsuarioPerfilSER objBss;
        private readonly IMapper mapper;

        public UsuarioPerfilController(IConfiguration configuracion, IMapper mapper)
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
            List<DTO.Result.UsuarioPerfilDTO> value = new List<DTO.Result.UsuarioPerfilDTO>();

            objBss = new Service.UsuarioPerfilSER(this.cnBD, this.mapper);
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
        [Route("listarAllxUsuario")]
        public IActionResult getAll(int idUsuario)
        {
            List<DTO.Result.UsuarioPerfilDTO> value = new List<DTO.Result.UsuarioPerfilDTO>();

            objBss = new Service.UsuarioPerfilSER(this.cnBD, this.mapper);
            value = objBss.listarxUsuario(idUsuario);

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
            DTO.Result.UsuarioPerfilDTO value = new DTO.Result.UsuarioPerfilDTO();

            objBss = new Service.UsuarioPerfilSER(this.cnBD, this.mapper);
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
        public IActionResult agregar(DTO.UsuarioPerfilAgregarDTO dto)
        {
            objBss = new Service.UsuarioPerfilSER(this.cnBD, this.mapper);
            DTO.Result.Result<int> result = new DTO.Result.Result<int>();

            result = objBss.agregar(dto);

            return Ok(result);
        }

        [HttpPut]
        [Route("modificar")]
        public IActionResult modificar(DTO.UsuarioPerfilModificarDTO dto)
        {
            objBss = new Service.UsuarioPerfilSER(this.cnBD, this.mapper);
            DTO.Result.Result<int> result = new DTO.Result.Result<int>();

            result = objBss.modificar(dto);

            return Ok(result);
        }

        [HttpPut]
        [Route("eliminar")]
        public IActionResult eliminar(DTO.EliminarDTO dto)
        {
            objBss = new Service.UsuarioPerfilSER(this.cnBD, this.mapper);
            DTO.Result.Result<int> result = new DTO.Result.Result<int>();

            result = objBss.eliminar(dto);

            return Ok(result);
        }



    }
}
