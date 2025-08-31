using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Cadena
    {
        public static string FromBase64String(string valueBase64)
        {
            try
            {
                byte[] valorBase64 = Convert.FromBase64String(valueBase64);
                return Encoding.UTF8.GetString(valorBase64);
            }
            catch 
            {
                return "";
            }
            
        }
        
    }
}
