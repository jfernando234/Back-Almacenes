using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class UsuarioAgregarPerfilDTO : UsuarioAgregarDTO
    {        
        public List<UsuarioPerfilDTO> listaPerfil { get; set; }

        public class UsuarioPerfilDTO
        {
            public int idPerfil { get; set; }
        }
    }  
}
