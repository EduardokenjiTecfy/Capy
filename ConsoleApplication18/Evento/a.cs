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
    public class a : IEvento
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
            RetornoEventoDTO retornoEventoDTO = new RetornoEventoDTO();
            switch (entrada)
            {

                case EnumEntrada.IMAGEM:

                    break;
                case EnumEntrada.DATA:

                    break;

                case EnumEntrada.NUMERO:
                    break;
                case EnumEntrada.TEXTO:
                    string retono = "";
                    var entradaData = ((String)parametros).ToUpper().Trim().Split(' ');
                    try
                    {


                        foreach (var item in entradaData)
                        {
                            Regex regex = new Regex(@"\S*([a-zA-Z_ ]{4}\-[_ ]\d{4}|[a-zA-Z]{3}\-\d{4})+\S*");
                            Match match = regex.Match(item);
                            if (match.Success)
                                retono = match.Value;
                            break;

                        }
                        retono.Replace(" ", "").Trim();
                        var split = retono.Split('-');
                        if (split.Count() == 2)
                        {
                            if (split[0].Contains("0"))
                            {
                                split[0].Replace('0', 'O');
                            }
                            if (split[0].Contains("1"))
                            {
                                split[0].Replace('1', 'I');
                            }
                            if (split[0].Contains("5"))
                            {
                                split[0].Replace('5', 'S');
                            }


                            ////remove letras
                            if (split[1].Contains("O"))
                            {
                                split[1].Replace('O', '0');
                            }
                            if (split[1].Contains("S"))
                            {
                                split[1].Replace('S', '5');
                            }
                            if (split[1].Contains("I"))
                            {
                                split[1].Replace('I', '1');
                            }
                            if (split[0].Count() == 4)
                            {
                                if (split[0].Contains("UJ"))
                                {
                                    var d = split[0].Replace("UJ", "W");
                                    split[0] = d;
                                }
                            }
                            retornoEventoDTO.Retono = split[0] + "-" + split[1];
                        }



                        return retornoEventoDTO;
                    }
                    catch (Exception ex)
                    {
                        retornoEventoDTO.Retono = entradaData;
                        return retornoEventoDTO;
                    }

                default:
                    break;
            }


            return retornoEventoDTO;
        }
    }
}
