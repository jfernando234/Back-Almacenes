using System;

namespace Entity
{
    public class UsuarioBE:Auditoria
    {

        public int idUsuario { get; set; }
        public int idPersona { get; set; }
        public string login { get; set; }
        public string clave { get; set; }
        public bool nuevaClave { get; set; } 
        public string primerApellido { get; set; }
        public string segundoApellido { get; set; }
        public string nombres { get; set; }
        public string ApellidosyNombres
        {
            get
            {
                return primerApellido + " " + segundoApellido + ", " + nombres;
            }
        }
        public string numeroDocumento { get; set; }
        public int idArea { get; set; }
        public string area { get; set; }
        public int idCargo { get; set; }
        public string cargo { get; set; }
        public string correoInstitucional { get; set; }
        public short idEstado { get; set; }
        public string estado
        {
            get
            {
                string value = "";

                switch (this.idEstado)
                {
                    case (short)Type.Usuario.Estado.Activo:
                        value = "Activo";
                        break;
                    case (short)Type.Usuario.Estado.Inactivo:
                        value = "Inactivo";
                        break;
                    default:
                        value = "";
                        break;
                }

                return value;
            }
        }
        public int idClientePerfil { get; set; }
        public string clientePerfil { get; set; }
        public string codigo_merchant { get; set; }
        public int idPerfil { get; set; }
    }
}
