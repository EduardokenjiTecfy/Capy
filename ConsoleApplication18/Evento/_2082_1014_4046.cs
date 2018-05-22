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
    public class _2082_1014_4046 : IEvento
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
                case EnumEntrada.LISTA:
                    var ls = (String[])parametros;
                    string retono = "";
                    foreach (var item in ls)
                    {
                        var ent = "";
                        try
                        {

                            ent = item.Replace(" ", "");
                            ent.Trim();
                            var splitValida = ent.Split('-');
                            if (splitValida.Count() == 2)
                            {
                                string valida = splitValida[1];
                                if (valida.Length > 4)
                                {
                                    splitValida[1] = splitValida[1].Substring(0, 4);
                                }
                                ent = splitValida[0] + "-" + splitValida[1];
                                if (splitValida[0].Length > 6)
                                {
                                    continue;
                                }
                            }
                            List<string> regexVal = new List<string>();
                            regexVal.Add(@"(^[a-zA-Z]{3}\-\d{4}$)");
                            regexVal.Add("(^[A-Z]{3}[0-9]{4})");

                            foreach (var rgx in regexVal)
                            {

                                Regex regex = new Regex(rgx);
                                Match match = regex.Match(ent);
                                if (match.Success && (ent.Length == 7 || ent.Length == 8))
                                {
                                    retono = match.Value;


                                    if (retono.Contains('-'))
                                    {
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
                                            string arq = split[0] + "-" + split[1];
                                            if (arq.Count() > 8)
                                            {
                                                Regex regesx = new Regex(@"\S*([a-zA-Z_ ]{4}\-[_ ]\d{4}|[a-zA-Z]{3}\-\d{4})+\S*");
                                                Match matcsh = regesx.Match(arq);
                                                if (matcsh.Success)
                                                {
                                                    retornoEventoDTO.Retono = matcsh.Groups[0].Value;
                                                }
                                            }
                                            else
                                            {
                                                retornoEventoDTO.Retono = split[0] + "-" + split[1];
                                            }
                                        }
                                        else
                                        {
                                            if (retono.Length == 7)
                                            {
                                                var spl = new string[2];
                                                spl[0] = retono.Substring(0, 3);
                                                spl[1] = retono.Substring(3, retono.Length - 3);
                                                if (spl.Count() == 2)
                                                {
                                                    if (spl[0].Contains("0"))
                                                    {
                                                        spl[0].Replace('0', 'O');
                                                    }
                                                    if (spl[0].Contains("1"))
                                                    {
                                                        spl[0].Replace('1', 'I');
                                                    }
                                                    if (spl[0].Contains("5"))
                                                    {
                                                        spl[0].Replace('5', 'S');
                                                    }


                                                    ////remove letras
                                                    if (spl[1].Contains("O"))
                                                    {
                                                        spl[1].Replace('O', '0');
                                                    }
                                                    if (spl[1].Contains("S"))
                                                    {
                                                        spl[1].Replace('S', '5');
                                                    }
                                                    if (spl[1].Contains("I"))
                                                    {
                                                        spl[1].Replace('I', '1');
                                                    }
                                                    if (spl[0].Count() == 4)
                                                    {
                                                        if (spl[0].Contains("UJ"))
                                                        {
                                                            var d = spl[0].Replace("UJ", "W");
                                                            spl[0] = d;
                                                        }
                                                    }
                                                    string arq = spl[0] + "-" + spl[1];

                                                    retornoEventoDTO.Retono = split[0] + "-" + split[1];
                                                }
                                            }
                                            else
                                            {
                                                Regex regesx = new Regex(@"\S*([a-zA-Z_ ]{4}\-[_ ]\d{4}|[a-zA-Z]{3}\-\d{4})+\S*");
                                                Match matcsh = regesx.Match(ent);
                                                if (matcsh.Success)
                                                {
                                                    retornoEventoDTO.Retono = matcsh.Groups[0].Value;
                                                }
                                            }
                                        }
                                    }
                                }

                            }
                            
                        }
                        catch (Exception ex)
                        {

                        }
                        
                    }
                    return retornoEventoDTO;
                    break;
                case EnumEntrada.TEXTO:
                    retono = "";
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
                            string arq = split[0] + "-" + split[1];
                            if (arq.Count() > 8)
                            {
                                Regex regex = new Regex(@"\S*([a-zA-Z_ ]{4}\-[_ ]\d{4}|[a-zA-Z]{3}\-\d{4})+\S*");
                                Match match = regex.Match(arq);
                                if (match.Success)
                                {
                                    retornoEventoDTO.Retono = match.Groups[0].Value;
                                }
                            }
                            else
                            {
                                retornoEventoDTO.Retono = split[0] + "-" + split[1];
                            }
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
