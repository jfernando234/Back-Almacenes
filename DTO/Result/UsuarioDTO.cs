using System;

namespace DTO.Result
{
    public class UsuarioDTO
    {
        public int idUsuario { get; set; }        
        public string login { get; set; }                
        public string primerApellido { get; set; }
        public string segundoApellido { get; set; }
        public string nombres { get; set; }
        public string ApellidosyNombres { get; set; }
        public string numeroDocumento { get; set; }
        public string area { get; set; }
        public string cargo { get; set; }
        public int idArea { get; set; }
        public int idCargo { get; set; }
        public string correoInstitucional { get; set; }
        public string estado { get; set; }
        public string codigo_merchant { get; set; }

    }

    public class AmbienteDTO
    {
        public string nomAmbiente { get; set; }
    }
   
}
