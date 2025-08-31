using AutoMapper;
using DTO.Result;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using ServiceWeb.Models;
using System;
using System.Collections.Generic;

namespace WebApiSeguridad.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        
        private readonly IConfiguration _configuration;
        private readonly IMapper mapper;
        private readonly string cnBD="";
        private Service.UsuarioSER objBss;

        public UsuarioController(IConfiguration configuracion, IMapper mapper)
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
        [Route("obtenerAmbiente")]
        public IActionResult getAmbiente()
        {
            string cnBD = "";
            cnBD = this._configuration.GetConnectionString("cn_bd_sige").Substring(12, 11).ToString();
            DTO.Result.AmbienteDTO Ambiente = new DTO.Result.AmbienteDTO();
            if (cnBD == "10.10.10.40") { Ambiente.nomAmbiente = "DESARROLLO";  return Ok(Ambiente); }
            if (cnBD == "10.10.10.41") { Ambiente.nomAmbiente = "QAS"; return Ok(Ambiente); }
            if (cnBD == "10.10.10.43") { Ambiente.nomAmbiente = "PRODUCCION"; return Ok(Ambiente); }
            return Ok(Ambiente);
        }
        

        [HttpGet]
        [Route("listarAll")]
        public IActionResult getAll()
        {
            List<DTO.Result.UsuarioDTO> value = new List<DTO.Result.UsuarioDTO>();
            objBss = new Service.UsuarioSER(this.cnBD, this.mapper);
            value=objBss.getAll();

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
            DTO.Result.UsuarioDTO value = new DTO.Result.UsuarioDTO();
            objBss = new Service.UsuarioSER(this.cnBD, this.mapper);
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


        [HttpGet]
        [Route("listarUsuarioPerfil")]
        public IActionResult getUsuarioPerfil(int idUsuario)
        {
            DTO.Result.UsuarioyPerfilDTO value = new DTO.Result.UsuarioyPerfilDTO();
            objBss = new Service.UsuarioSER(this.cnBD, this.mapper);
            value = objBss.getUsuarioPerfil(idUsuario);

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
        [Route("listarUsuarioModulo")]
        public IActionResult getUsuarioModuloPermiso(int id)
        {
            List<DTO.Result.UsuarioModuloDTO> value = new List<DTO.Result.UsuarioModuloDTO>();
            objBss = new Service.UsuarioSER(this.cnBD, this.mapper);
            value = objBss.listarUsuarioModulos(id);

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
        public IActionResult add(DTO.UsuarioAgregarDTO dto)
        {
            objBss = new Service.UsuarioSER(this.cnBD, this.mapper);

            DTO.Result.Result<int> result = new DTO.Result.Result<int>();
            result = objBss.agregar(dto);         

            return Ok(result);
        }

        [HttpPost]
        [Route("agregarUsuarioPerfil")]
        public IActionResult addUsuarioPerfil(DTO.UsuarioAgregarPerfilDTO dto)
        {
            objBss = new Service.UsuarioSER(this.cnBD, this.mapper);

            DTO.Result.Result<int> result = new DTO.Result.Result<int>();
            result = objBss.agregarUsuarioPerfil(dto);

            return Ok(result);
        }

        [HttpPost]
        [Route("validarUsuario")]
        public IActionResult validarLogin(DTO.UsuarioDatosLoginDTO dto)
        {
            try
            {

                objBss = new Service.UsuarioSER(this.cnBD, this.mapper);
                DTO.Result.Result<DTO.Result.UsuarioLoginDTO> result = new DTO.Result.Result<DTO.Result.UsuarioLoginDTO>();

                result = objBss.validarLogin(dto);

                return Ok(result);
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        [HttpPut]
        [Route("modificar")]
        public IActionResult modificar(DTO.UsuarioModificarDTO dto)
        {
            objBss = new Service.UsuarioSER(this.cnBD, this.mapper);
            DTO.Result.Result<int> result = new DTO.Result.Result<int>();
           
            result = objBss.modificar(dto);            

            return Ok(result);
        }

        [HttpPut]
        [Route("restaurarClave")]
        public IActionResult restaurarClave(DTO.UsuarioRestaurarClaveDTO dto)
        {
            objBss = new Service.UsuarioSER(this.cnBD, this.mapper);
            DTO.Result.Result<int> result = new DTO.Result.Result<int>();
            
            result = objBss.restaurarClave(dto);            

            return Ok(result);
        }

        [HttpPut]
        [Route("cambiarClave")]
        public IActionResult cambiarClave(DTO.UsuarioCambiarClaveDTO dto)
        {
            objBss = new Service.UsuarioSER(this.cnBD, this.mapper);
            DTO.Result.Result<int> result = new DTO.Result.Result<int>();
           
            result = objBss.cambiarClave(dto);           

            return Ok(result);
        }

        [HttpPut]
        [Route("eliminar")]
        public IActionResult eliminar(DTO.EliminarDTO dto)
        {
            objBss = new Service.UsuarioSER(this.cnBD, this.mapper);
            DTO.Result.Result<int> result = new DTO.Result.Result<int>();            

            result = objBss.eliminar(dto);            

            return Ok(result);
        }

        [HttpPost]
        [Route("registroCliente")]
        public IActionResult add(DTO.PersonaAgregarDTO dto)
        {
            objBss = new Service.UsuarioSER(this.cnBD, this.mapper);

            DTO.Result.Result<int> result = new DTO.Result.Result<int>();
            result = objBss.registroCliente(dto);

            return Ok(result);
        }

        [HttpPost] 
        [Route("registroPerfilCli")]
        public IActionResult addPerfilCli(DTO.PersonaAgregarDTO dto,int idUsuario)
        {
            objBss = new Service.UsuarioSER(this.cnBD, this.mapper);

            DTO.Result.Result<int> result = new DTO.Result.Result<int>();
            result = objBss.registroPerfilCli(dto, idUsuario);

            return Ok(result);
        }

        [HttpPut]
        [Route("modificaPerfilCli")]
        public IActionResult modificar(DTO.PersonaModificarDTO dto)
        {
            objBss = new Service.UsuarioSER(this.cnBD, this.mapper);
            DTO.Result.Result<int> result = new DTO.Result.Result<int>();

            result = objBss.modificaPerfilCli(dto);

            return Ok(result);
        }

        [HttpGet]
        [Route("listarPerfil")]
        public IActionResult getPerfil(int id)
        {
            DTO.PersonaAgregarDTO value = new DTO.PersonaAgregarDTO();
            objBss = new Service.UsuarioSER(this.cnBD, this.mapper);
            value = objBss.getPerfil(id);

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
