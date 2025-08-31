using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Map
{
    public class PerfilModuloMAP : EntityMap<PerfilModuloBE>
    {
        public PerfilModuloMAP()
        {
            Map(m => m.idPerfilModulo).ToColumn("pk_per_mod_id");
            Map(m => m.idPerfil).ToColumn("fk_per_id");
            Map(m => m.idModulo).ToColumn("fk_mod_id");
            Map(m => m.nombreModulo).ToColumn("nombre_modulo"); 
            Map(m => m.consultar).ToColumn("consultar_b");
            Map(m => m.agregar).ToColumn("crear_b");
            Map(m => m.modificar).ToColumn("modificar_b");
            Map(m => m.eliminar).ToColumn("eliminar_b");
            Map(m => m.imprimir).ToColumn("imprimir_b");
            Map(m => m.descargarPDF).ToColumn("descargar_pdf_b");
            Map(m => m.descargarExcel).ToColumn("descagar_excel_b");
        }
    }
}
