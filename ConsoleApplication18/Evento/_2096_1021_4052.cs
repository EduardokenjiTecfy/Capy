using GdPicture;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleApplication18.Evento
{
    public class _2096_1021_4052 : IEvento
    {
        public object C001(object parametros)
        {
            RetornoEventoDTO retornoEventoDTO = new RetornoEventoDTO();
            MailMessage mail = new MailMessage("contatotecfy@gmail.com", "ekm_1417@msn.com");
            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Host = "smtp.google.com";
            mail.Subject = "novo envio.";
            mail.Body = "this is my test email body";
            client.Credentials = new System.Net.NetworkCredential("contatotecfy@gmail.com", "Teste1234@");

            client.Send(mail);
            return retornoEventoDTO;
        }

        public object C002(object parametros, EnumEntrada entrada)
        {
            throw new NotImplementedException();
        }

        public object C003(object parametros, EnumEntrada entrada)
        {
            throw new NotImplementedException();
        }
        GdPictureImaging oGdPictureImaging = new GdPictureImaging();

        public object C004(object parametros, EnumEntrada entrada)
        {
            var entradaData = ((String[])parametros);

            RetornoEventoDTO retornoEventoDTO = new RetornoEventoDTO();
            switch (entrada)
            {

                case EnumEntrada.IMAGEM:

                    break;
                case EnumEntrada.DATA:

                    break;

                case EnumEntrada.NUMERO:
                    break;
                case EnumEntrada.LISTA:
                    if (entradaData.Count() > 0)
                    {
                        retornoEventoDTO.Retono = entradaData.FirstOrDefault().Replace(" ", "").ToUpper().Replace("Ç", "C");
                    }
                    else
                    {
                        retornoEventoDTO.Retono = "";
                    }
                    break;
                case EnumEntrada.TEXTO:
                    break;

                default:
                    break;
            }


            return retornoEventoDTO;
        }
    }
}
