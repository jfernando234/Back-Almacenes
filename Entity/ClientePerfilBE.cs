using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class ClientePerfilBE : Auditoria
    {
        public int idClientePerfil { get; set; }
        public int idUsuario { get; set; }
        public int idPersona { get; set; }
        public string perfil { get; set; }
    }

    public class bancoBE
    {
        public int idBanco { get; set; }
        public string nomBanco { get; set; }
    }
    public class tipoCuentaBE
    {
        public int idtipoCuenta { get; set; }
        public string tipocuenta { get; set; }
    }
    public class monedaBE
    {
        public int idmoneda { get; set; }
        public string moneda { get; set; }
    }

    public class PerfilcuentaBancariaBE
    {
        public int idCuentabanc { get; set; }
        public int idClientePerfil { get; set; }
        public int idBanco { get; set; }
        public int idtipoCuenta { get; set; }
        public int idmoneda { get; set; }
        public string nomBanco { get; set; }
        public string tipocuenta { get; set; }
        public string moneda { get; set; }
        public string nrocta { get; set; }
        public string cci { get; set; }
        public string flgtercero { get; set; }
        public int fk_tip_doc_id { get; set; }
        public string num_doc { get; set; }
        public string nombres { get; set; }
        public string apellidos { get; set; }
        public string flgmancomunada { get; set; }
        public int fk_tip_doc_id_man { get; set; }
        public string num_doc_man { get; set; }
        public string nombres_man { get; set; }
        public string apellidos_man { get; set; }
    }

    public class dashboardClienteBE
    {
        public int ayo { get; set; }
        public int mes { get; set; }
        public string codigoMerchant { get; set; }
        public double saldo_inicial { get; set; }
        public double payin { get; set; }
        public double payincomision { get; set; }
        public double payouting { get; set; }
        public double payoutingext { get; set; }
        public double payout { get; set; }
        public double payoutcomision { get; set; }
        public double solicitud { get; set; }
        public double saldo_final { get; set; }
        public double transfing { get; set; }
        public double transfext { get; set; }
    }

}
