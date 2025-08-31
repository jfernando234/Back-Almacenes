using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Result
{
    public  class UsuarioModuloDTO: ModuloDTO
    {      
        public ModuloAccesoDTO moduloAcceso { get; set; }

        public class ModuloAccesoDTO
        {
            public int idModulo { get; set; }
            public bool consultar { get; set; }
            public bool agregar { get; set; }
            public bool modificar { get; set; }
            public bool eliminar { get; set; }
            public bool imprimir { get; set; }
            public bool descargarPDF { get; set; }
            public bool descargarExcel { get; set; }
        }
    }  
}
