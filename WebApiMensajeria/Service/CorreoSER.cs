using System;
using System.Net.Mail;
using System.Net.Mime;
using WebApiMensajeria.Entity;

namespace WebApiMensajeria.Service
{
    public class CorreoSER
    {
        private readonly string serverMail = "";
        private readonly int serverPuerto = 0;
        private readonly string usuarioFrom = "";
        private readonly string usuarioClave = "";
        private readonly string listaCorreoCCO = "";

        public CorreoSER(string serverMail, int serverPuerto, string usuarioFrom, string usuarioClave, string listaCorreoCCO)
        {
            this.usuarioFrom = usuarioFrom;
            this.usuarioClave = usuarioClave;
            this.serverMail = serverMail;
            this.serverPuerto = serverPuerto;
            this.listaCorreoCCO = listaCorreoCCO;
        }

        private bool validar(CorreoBE dto, out string mensaje)
        {
            bool value = false;
            mensaje = "";

            //validar los destinarios para
            if (dto.listaPara.Count < 1)
            {
                mensaje = "No existen destinatarios";
                return value;
            }

            value = true;
            return value;
        }

        public Entity.Result<bool> enviar(CorreoBE dto)
        {
            bool value = false;
            string mensaje = "";
            Entity.Result<bool> resultValue = new Entity.Result<bool>();

            if (validar(dto, out mensaje) == false)
            {
                resultValue.errorMensaje = mensaje;
                resultValue.errorCodigo = "VAL";
                return resultValue;
            }

            string Comillas = "\"";
            AlternateView objHtmlView;
            string Html = "<h3> <p style=" + Comillas + "color:#0A3F78" + Comillas + "> " + dto.mensaje + " </p> </h3>";
            System.Text.UTF8Encoding objEncode = new System.Text.UTF8Encoding();
            objHtmlView = AlternateView.CreateAlternateViewFromString(Html, objEncode, MediaTypeNames.Text.Html);

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(usuarioFrom);

            try
            {

                //agregar los correos para
                foreach (string fila in dto.listaPara)
                {
                    mail.To.Add(fila);
                }

                //agregar los correo concopia
                if (dto.listaCC != null )
                {
                    foreach (string fila in dto.listaCC)
                    {
                        mail.CC.Add(fila);
                    }
                }               

                //agregar los correo concopia oculta
                foreach (string fila in listaCorreoCCO.Split(","))
                {
                    if (fila.Length>5)
                    {
                        mail.Bcc.Add(fila);
                    }                    
                }

                mail.Subject = dto.asunto;
                mail.AlternateViews.Add(objHtmlView);
                mail.IsBodyHtml = true;


                SmtpClient smtp = new SmtpClient(serverMail, serverPuerto);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.EnableSsl = true;

                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(usuarioFrom, usuarioClave);
                smtp.Send(mail);
                value = true;

                resultValue.value = value;

            }
            catch (Exception ex)
            {

                string codigo = System.Guid.NewGuid().ToString();
                string seccion = "Service/CorreoSER";

                Common.Log.EscribirLog(seccion, "Identificador:" + codigo + ", enviar correo", ex.Message.ToString(), false);

                resultValue.errorMensaje = "Se origino una exception en el código: " + seccion + ". Para más detalle verifique el archivo log con el identificador: " + codigo;
                resultValue.errorCodigo = "ERROR";

            }

            return resultValue;
        }
    }
}
