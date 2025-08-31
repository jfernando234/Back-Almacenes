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
    public class UsuarioRolController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string cnBD = "";
        private Service.UsuarioRolSER objBss;
        private readonly IMapper mapper;

        public UsuarioRolController(IConfiguration configuracion, IMapper mapper)
        {
            this._configuration = configuracion;
            this.mapper = mapper;
            this.cnBD = this._configuration.GetConnectionString("cn_bd_sige");
        }



        [HttpGet]
        [Route("listarAll")]
        public IActionResult getAll(int idUsuario)
        {
            List<DTO.Result.UsuarioRolDTO> value = new List<DTO.Result.UsuarioRolDTO>();

            objBss = new Service.UsuarioRolSER(this.cnBD, this.mapper);
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
        [HttpGet]
        [Route("listarDetalleAll")]
        public IActionResult getDetalleAll(int idRol, int idRolUsuario)
        {
            List<DTO.Result.UsuarioRolDetDTO> value = new List<DTO.Result.UsuarioRolDetDTO>();

            objBss = new Service.UsuarioRolSER(this.cnBD, this.mapper);
            value = objBss.getDetalleAll(idRol, idRolUsuario);

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
        public IActionResult agregar(DTO.Result.UsuarioRolNuevoDTO dto)
        {
            objBss = new Service.UsuarioRolSER(this.cnBD, this.mapper);

            DTO.Result.Result<int> result = new DTO.Result.Result<int>();
            result = objBss.agregar("ins_mant", dto);

            return Ok(result);
        }

    }
}
