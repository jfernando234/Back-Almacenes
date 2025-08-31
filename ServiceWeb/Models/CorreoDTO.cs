using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceWeb.Models
{
    public class CorreoDTO
    {
        public string asunto { get; set; }
        public string mensaje { get; set; }
        public List<string> listaPara { get; set; }
        public List<string> listaCC { get; set; }
    }
}
