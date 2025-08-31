using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class Auditoria
    {
        public int idUsuarioLogin { get; set; }        
        public string pcIp { get; set; }
        public string pcHost { get; set; }
    }
}
