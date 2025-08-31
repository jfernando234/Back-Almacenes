using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Result
{
    public  class UsuarioLoginDTO
    {
        public int idUsuario { get; set; }
        public string login { get; set; }
        public bool nuevaClave { get; set; }
        public string ApellidosyNombres { get; set; }        
        public string correoInstitucional { get; set; }
        public int idClientePerfil { get; set; }
        public string clientePerfil { get; set; }
        public string codigo_merchant { get; set; }
        public int idPerfil { get; set; }
    }
}
