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
    public class _2076_1005_4041 : IEvento
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
                    var imagemvalor = (System.Drawing.Bitmap)parametros;
                    oGdPictureImaging.SetLicenseNumber("4118106456693265856441854");
                    var imagemId = oGdPictureImaging.CreateGdPictureImageFromBitmap((imagemvalor));
                    var imagem = oGdPictureImaging.IsBlank(imagemId, 90);
                    imagemvalor.Dispose();
                    if (imagem)
                    {
                        retornoEventoDTO.Retono = "Não possui assinatura";
                        retornoEventoDTO.RetornoTipo = EnumRetornoEvento.RETORNO_STRING;
                    }
                    else
                    {
                        retornoEventoDTO.Retono = "Possui assinatura";
                        retornoEventoDTO.RetornoTipo = EnumRetornoEvento.RETORNO_STRING;
                    }
                    oGdPictureImaging.ReleaseGdPictureImage(imagemId);
                    break;
                case EnumEntrada.DATA:
                    var entradaData = ((String)parametros).Trim();
                    try
                    {

                        Regex regex = new Regex("^([0]?[0-9]|[12][0-9]|[3][01])[./-]([0]?[1-9]|[1][0-2])[./-]([0-9]{4}|[0-9]{2})$");
                        Match m = regex.Match(entradaData);

                        int matchCount = 0;
                        if (m.Success)
                        {


                            ///"
                            //^(?:[012]?[0-9]|3[01])[./-](?:0?[1-9]|1[0-2])[./-](?:[0-9]{2}){1,2}$
                            var data = Convert.ToDateTime(m.Value);

                            if (data.Day > 30 || data > DateTime.Now.Date)
                            {

                                var image = ConfigurationManager.AppSettings["DocumentoImages"];
                                byte[] bytes = System.IO.File.ReadAllBytes(image + "icon_inprogress.gif");
                                string img = Convert.ToBase64String(bytes, 0, bytes.Length);

                                retornoEventoDTO.Retono = "data:image/png;base64," + img;
                                retornoEventoDTO.MensagemRetorno = "Este campo capturado possui uma data inválida";

                                retornoEventoDTO.RetornoTipo = EnumRetornoEvento.RETORNO_IMAGEM;
                            }
                            else
                            {
                                var image = ConfigurationManager.AppSettings["DocumentoImages"];
                                byte[] bytes = System.IO.File.ReadAllBytes(image + "ok.png");
                                string img = Convert.ToBase64String(bytes, 0, bytes.Length);

                                retornoEventoDTO.Retono = "data:image/png;base64," + img;
                                retornoEventoDTO.MensagemRetorno = "Este campo capturado possui uma data válida";

                                retornoEventoDTO.RetornoTipo = EnumRetornoEvento.RETORNO_IMAGEM;
                            }
                        }
                        else
                        {
                            throw new Exception("Não foi possivel identificar o campo data");
                        }

                    }
                    catch (Exception ex)
                    {
                        var image = ConfigurationManager.AppSettings["DocumentoImages"];
                        byte[] bytes = System.IO.File.ReadAllBytes(image + "icon_inprogress.gif");
                        string img = Convert.ToBase64String(bytes, 0, bytes.Length);
                        retornoEventoDTO.Retono = "data:image/png;base64," + img;
                        retornoEventoDTO.MensagemRetorno = "Este campo capturado possui uma data inválida";
                        retornoEventoDTO.RetornoTipo = EnumRetornoEvento.RETORNO_IMAGEM;
                    }
                    break;

                case EnumEntrada.NUMERO:
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
