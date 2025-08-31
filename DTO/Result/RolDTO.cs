using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Result
{
    public class RolDTO
    {
        public int idRol { get; set; }
        public int idModulo { get; set; }
        public string nombreRol { get; set; }
        public string descripcionRol { get; set; }
        public int valorNumerico { get; set; }
        public string valorCadena { get; set; }
        public bool valorLogico { get; set; }
    }
}
