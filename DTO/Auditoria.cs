using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace DTO
{
    public class Auditoria
    {
        [JsonIgnore]
        public int idUsuarioLogin { get; set; }        
        [JsonIgnore]
        public string pcIp { get; set; }
        [JsonIgnore]
        public string pcHost { get; set; }
    }
}
