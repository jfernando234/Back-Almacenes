using System.Collections.Generic;

namespace WebApiMensajeria.Entity
{
    public class CorreoBE
    {
        public string asunto { get; set; }
        public string mensaje { get; set; }
        public List<string> listaPara { get; set; }
        public List<string> listaCC { get; set; }
    }
}
