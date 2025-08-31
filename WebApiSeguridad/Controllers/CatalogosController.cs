using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApiSeguridad.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogosController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string cnBD = "";
        private Service.CatalogosSER objBss;
        private readonly IMapper mapper;
        private readonly string tokenDNI = "";

        public CatalogosController(IConfiguration configuracion, IMapper mapper)
        {
            this._configuration = configuracion;
            this.mapper = mapper;
            this.cnBD = this._configuration.GetConnectionString("cn_bd_sige");
            this.tokenDNI = this._configuration.GetSection("FE:tokenDNI").Value;
        }
        
        [HttpGet]
        [Route("obtenerDNI")]
        public async Task<ActionResult<DTO.dniDTO>> getDNI(string dni)
        {
            DTO.dniDTO value = new DTO.dniDTO();

            objBss = new Service.CatalogosSER(this.cnBD);
            value = await objBss.getDNI(this.tokenDNI, dni);

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
        [Route("obtenerRUC")]
        public async Task<ActionResult<DTO.rucDTO>> getRUC(string ruc)
        {
            DTO.rucDTO value = new DTO.rucDTO();

            objBss = new Service.CatalogosSER(this.cnBD);
            value = await objBss.getRUC(this.tokenDNI, ruc);

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
