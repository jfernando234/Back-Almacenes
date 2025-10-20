using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    internal class Almacen:Auditoria
    { 
        public int Id { get; set; }
        public string Name { get; set; }
        public int productId  { get; set; }
        public int Cantidad { get; set; }
        public DateTime Fecha_Registro { get; set; }

        public Almacen()
        {
            Id = 0;
            Name = string.Empty;
            productId = 0;
            Cantidad = 0;
            Fecha_Registro = DateTime.Now;
        }

    }
}
