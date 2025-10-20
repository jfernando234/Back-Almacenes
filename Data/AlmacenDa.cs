using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class AlmacenDa
    {
        private readonly string cnBD = "";
        private string sqlQuery = "";
        private readonly string bdEsquema = "dbo.";
        private readonly string bdTabla = "almacen_mae";
        public AlmacenDa(string cnBD)
        {
            this.cnBD = cnBD;
        }
        
    }
}
