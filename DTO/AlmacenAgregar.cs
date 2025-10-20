using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class AlmacenAgregar
    {
        [Required]
        [StringLength(100)]
        public string NombreAlmacen { get; set; }

    }
}
