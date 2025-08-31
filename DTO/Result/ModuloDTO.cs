using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Result
{
    public class ModuloDTO
    {
        public int idModulo { get; set; }
        public int idModuloPadre { get; set; }
        public string nombreModuloPadre { get; set; }
        public short nivel { get; set; }
        public bool esHijo { get; set; }
        public string nombreModulo { get; set; }="";
        public string nombreObjeto { get; set; }
        public string codigoAccesoDirecto { get; set; }
        public string icono { get; set; }
        public string url { get; set; }
        public short numeroOrden { get; set; }

        public bool esVisible { get; set; }
        public string dependencia { get; set; } = "";
    }
}
