using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class EnvioWSP
    {
        public class Peticion
        {
            public string messaging_product { get; set; }
            public string to { get; set; }
            public string type { get; set; }
            public Template template { get; set; }
        }

        public class Template
        {
            public string name { get; set; }
            public dynamic language { get; set; }
            //public List<Componente> components { get; set; }
        }

        public class Componente
        {
            public string type { get; set; }
            public List<dynamic> parameters { get; set; }
        }
    }
}
