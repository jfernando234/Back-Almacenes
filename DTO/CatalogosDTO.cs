using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class dniDTO
    {
        public string success { get; set; }
        public string message { get; set; }
        public string dni { get; set; }
        public string nombres { get; set; }
        public string apellidoPaterno { get; set; }
        public string apellidoMaterno { get; set; }
    }
    public class rucDTO
    {
        public string success { get; set; }
        public string message { get; set; }
        public string ruc { get; set; }
        public string razonSocial { get; set; }


   
        public string estado { get; set; }
        public string condicion { get; set; }

    }
}
