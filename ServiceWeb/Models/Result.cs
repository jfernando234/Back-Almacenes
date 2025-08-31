using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceWeb.Models
{
    public class Result<objTipo>
    {
        public objTipo value { get; set; }
        public string errorCodigo { get; set; } = "OK";
        public string errorMensaje { get; set; } = "";
    }
}
