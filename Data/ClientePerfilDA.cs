using Dapper;
using Dapper.FluentMap;
using Entity.Map;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class ClientePerfilDA
    {
        private readonly string cnBD = "";
        private string sqlQuery = "";
        private readonly string bdEsquema = "sch_seguridad.";
        private readonly string bdTabla = "perfil_mae";

        public ClientePerfilDA(string cnBD)
        {
            this.cnBD = cnBD;
            //FluentMapper.EntityMaps.Clear();
            //FluentMapper.Initialize(config => { config.AddMap(new PerfilMAP()); });
        }

        public List<Entity.ClientePerfilBE> listar(int idUsuario)
        {
            sqlQuery = "usp_cliente_perfil_mae";

            var p = new DynamicParameters();
            p.Add("idUsuario", idUsuario);

            using (SqlConnection sqlCn = new SqlConnection(this.cnBD))
            {
                var lista = sqlCn.Query<Entity.ClientePerfilBE>(sqlQuery, p, commandType: CommandType.StoredProcedure).ToList();
                return lista;
            }
        }

    }
}
