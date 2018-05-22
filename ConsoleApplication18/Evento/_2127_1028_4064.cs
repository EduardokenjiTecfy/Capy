using ConsoleApplication18.Model;
using ConsoleApplication18.OcrImpletentacao;
using GdPicture;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleApplication18.Evento
{
    public class _2127_1028_4064 : IEvento
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

        public string[] cortesScript(string imagemCaminho, string script)
        {
            string python = @"C:\Python27\Python.exe";

            // python app to call 
            string myPythonApp = script;

            // Create new process start info 
            ProcessStartInfo myProcessStartInfo = new ProcessStartInfo(python);

            // make sure we can read the output from stdout 
            myProcessStartInfo.UseShellExecute = false;
            myProcessStartInfo.RedirectStandardOutput = true;

            // start python app with 3 arguments  
            // 1st arguments is pointer to itself,  
            // 2nd and 3rd are actual arguments we want to send 
            myProcessStartInfo.Arguments = myPythonApp + " " + imagemCaminho + " " + Guid.NewGuid().ToString();

            Process myProcess = new Process();
            // assign start information to the process 
            myProcess.StartInfo = myProcessStartInfo;
            myProcess.StartInfo.CreateNoWindow = true;
            // start the process 
            myProcess.Start();

            // Read the standard output of the app we called.  
            // in order to avoid deadlock we will read output first 
            // and then wait for process terminate: 
            StreamReader myStreamReader = myProcess.StandardOutput;
            string myString = myStreamReader.ReadLine();

            /*if you need to read multiple lines, you might use: 
                string myString = myStreamReader.ReadToEnd() */

            // wait exit signal from the app we called and then close it. 
            myProcess.WaitForExit();
            myProcess.Close();

            string[] cortes = myString.Split('$');
            return cortes;
        }
        public class arq
        {
            private string local;
            private int ordenacao;

            public string Local { get => local; set => local = value; }
            public int Ordenacao { get => ordenacao; set => ordenacao = value; }
        }
        private readonly Color colorBlack =
        Color.FromArgb(255, 0, 0, 0);
        private readonly Color colorWhite =
          Color.FromArgb(255, 255, 255, 255);
        public string corte(String imagemCaminho, string script, string tipo, Image img)
        {
            DateTime dt = new DateTime();
            var imgId = Guid.NewGuid() + ".png";
            var arquivoCorteDataImage = CanhotosLeitura.CropImage((Bitmap)img, 627, 2424, 815, 499);
            img.Dispose();
            arquivoCorteDataImage.Save(imgId);
            arquivoCorteDataImage.Dispose();
            string[] corssstes = cortesScript(imgId, @"C:\PythonScript\data.py");


            for (int i = 0; i < corssstes.Length; i++)
            {

                var Corte = corssstes[i];
                var pos = Corte.Split(' ');
                if (pos.Length > 2)
                {
                    var coordenadas = new Coordenadas()
                    {
                        X = Convert.ToInt16(pos[0]),
                        Y = Convert.ToInt16(pos[1]),
                        W = Convert.ToInt16(pos[2]),
                        H = Convert.ToInt16(pos[3])

                    };
                    var bpData = Bitmap.FromFile(imgId);

                    var arquivoCortedata = CanhotosLeitura.CropImage(bpData, coordenadas.X, coordenadas.Y, coordenadas.W, coordenadas.H);
                    if (arquivoCortedata.Width > 300 && arquivoCortedata.Height > 90)
                    {
                        if (!Directory.Exists("dataICR"))
                        {
                            Directory.CreateDirectory("dataICR");
                        }
                        var iddataCrop = Guid.NewGuid() + ".png";
                        arquivoCortedata.Save(@"dataICR\" + iddataCrop + ".png");
                        GoogleOcr ocr = new GoogleOcr();
                        var saida = ocr.executarOcr(@"dataICR\" + iddataCrop + ".png");


                        var splitSaida = saida.ToString().Split(' ');
                        foreach (var item in splitSaida)
                        {
                            var rx = "(^((((0[1-9])|([1-2][0-9])|(3[0-1]))|([1-9]))\x2F(((0[1-9])|(1[0-2]))|([1-9]))\x2F(([0-9]{2})|(((19)|([2]([0]{1})))([0-9]{2}))))$)";
                            var d = Regex.Match(item, rx);
                            if (d.Success)
                            {
                                dt = Convert.ToDateTime(d.Value).Date;
                                break;
                            }
                        }






                    }
                    bpData.Dispose();
                    arquivoCortedata.Dispose();
                }
            }



            var arquivotif = Guid.NewGuid() + ".tif";
            var id = oGdPictureImaging.CreateGdPictureImageFromFile(imagemCaminho);
            oGdPictureImaging.SaveAsTIFF(id, arquivotif, TiffCompression.TiffCompressionLZW);
            //new OmniPageOcr().executarIcry(arquivotif);
            foreach (var item in Directory.GetFiles(@"Crop\"))
            {
                File.Delete(item);
            }
            string[] cortes = cortesScript(imagemCaminho, script);
            int verifica = 0;
            string dia = "";
            int count = 0;
            for (int i = 0; i < cortes.Length; i++)
            {
                var Corte = cortes[i];
                var posicoes = Corte.Split(' ');
                if (posicoes.Length > 2)
                {
                    var coor = new Coordenadas()
                    {
                        X = Convert.ToInt16(posicoes[0]),
                        Y = Convert.ToInt16(posicoes[1]),
                        W = Convert.ToInt16(posicoes[2]),
                        H = Convert.ToInt16(posicoes[3])

                    };
                    var bp = Bitmap.FromFile(imagemCaminho);

                    var arquivoCorte = CanhotosLeitura.CropImage(bp, coor.X, coor.Y, coor.W, coor.H);
                    arquivoCorte.Save(@"Crop\" + coor.X + ".png");
                    bp.Dispose();



                    // corteCaracteres
                    List<String> datas = new List<string>();
                    var array = cortesScript(@"Crop\" + coor.X + ".png", @"C:\PythonScript\corteCaracteres.py");
                    foreach (var arrayScript in array)
                    {
                        var posicoesScript = arrayScript.Split(' ');
                        if (posicoesScript.Length > 2)
                        {

                            var coord = new Coordenadas()
                            {
                                X = Convert.ToInt16(posicoesScript[0]),
                                Y = Convert.ToInt16(posicoesScript[1]),
                                W = Convert.ToInt16(posicoesScript[2]),
                                H = Convert.ToInt16(posicoesScript[3])

                            };
                            var bpp = Bitmap.FromFile(@"Crop\" + coor.X + ".png");

                            var arquivoCortes = CanhotosLeitura.CropImage(bpp, coord.X, coord.Y, coord.W, coord.H);
                            if (arquivoCortes.Width > 80)
                            {
                                count++;
                                arquivoCortes.Save(Guid.NewGuid() + "CRPPPPPPPPPPPPP.png");
                                Bitmap resized = new Bitmap(arquivoCortes, new Size(32, 32));

                                resized.Save(@"CropScript\" + coord.X + ".png");
                                datas.Add(@"CropScript\" + coord.X + ".png");


                            }
                            bpp.Dispose();


                        }
                    }
                    var aqruivos = datas.OrderByDescending(_s => _s);

                    int cc = 0;
                    List<arq> arqa = new List<arq>();

                    foreach (var item in aqruivos)
                    {
                        arqa.Add(new arq() { Local = item, Ordenacao = Convert.ToInt32(Path.GetFileNameWithoutExtension(item)) });
                    }

                    foreach (var item in arqa.OrderBy(_s => _s.Ordenacao))
                    {
                        string arquivo = Guid.NewGuid().ToString();
                        if (File.Exists(item.Local))
                        {

                            var imgSrc = (System.Drawing.Bitmap)Bitmap.FromFile(item.Local);
                            int width = imgSrc.Width;
                            int height = imgSrc.Height;
                            Color pixel;
                            Bitmap imgOut = new Bitmap(imgSrc);
                            for (int row = 0; row < height - 1; row++)
                            {
                                for (int col = 0; col < width - 1; col++)
                                {
                                    pixel = imgSrc.GetPixel(col, row);

                                    imgOut.SetPixel(0, 31, this.colorWhite);
                                    imgOut.SetPixel(31, 0, this.colorWhite);

                                    if (row == 0 || row == 31)
                                    {
                                        imgOut.SetPixel(col, row, this.colorWhite);
                                    }
                                    if (col == 0 || col == 31)
                                    {
                                        imgOut.SetPixel(col, row, this.colorWhite);
                                    }

                                    //if (pixel.GetBrightness() == 0)
                                    //{

                                    //    imgOut.SetPixel(col, row, this.colorBlack);
                                    //}
                                    //else
                                    //{
                                    //    imgOut.SetPixel(col, row, this.colorWhite);
                                    //}
                                }
                            }
                            imgSrc.Dispose();
                            File.Delete(item.Local);

                            imgOut.Save(item.Local);

                            imgOut.Dispose();






                            var omni = new OmniPageOcr();
                            dia += omni.executarIcry(item.Local).ToString();
                            dia = Regex.Replace(dia, "�", "");










                            File.Move(item.Local, @"C:\ConsoleApplication18\ConsoleApplication18\bin\Debug\CropScript\Entrada\" + arquivo + ".png");
                            var sair = true;

                            //while (sair)
                            //{
                            //    var processamento = Directory.GetFiles(@"C:\ConsoleApplication18\ConsoleApplication18\bin\Debug\CropScript\saida\");
                            //    foreach (var ar in processamento)
                            //    {
                            //        if (ar.Contains(arquivo))
                            //        {
                            //            sair = false;
                            //            var numero = Path.GetFileNameWithoutExtension(ar).Split('#');
                            //            if (numero.Count() > 1)
                            //            {
                            //                dia += numero[1];
                            //            }
                            //            break;
                            //        }
                            //    }
                            //    //{ }
                            //}

                            //
                            //   cc++;
                        }
                    }
                    foreach (var item in aqruivos)
                    {
                        File.Delete(item);
                    }



                    ///  arquivoCorte.Save(@"C:\Users\Administrador\Documents\Visual Studio 2015\Projects\ConsoleApplication18\ConsoleApplication18\bin\Debug\Crop\" + Corte + ".png");
                    MemoryStream ms = new MemoryStream();
                    arquivoCorte.Save(ms, ImageFormat.Png);
                    byte[] bmpBytes = ms.ToArray();
                    PosicaoLabel pol = new PosicaoLabel();
                    pol.Coordenadas = coor;
                    // pol.Label = ocr.executarOcr(bmpBytes).ToString();


                    arquivoCorte.Dispose();
                    ms.Close();
                    verifica++;
                }



            }
            foreach (var Corte in cortes)
            {

                //var posicoes = Corte.Split(' ');
                //if (posicoes.Length > 2)
                //{
                //    var coor = new Coordenadas()
                //    {
                //        X = Convert.ToInt16(posicoes[0]),
                //        Y = Convert.ToInt16(posicoes[1]),
                //        W = Convert.ToInt16(posicoes[2]),
                //        H = Convert.ToInt16(posicoes[3])

                //    };
                //    var bp = Bitmap.FromFile(imagemCaminho);

                //    var arquivoCorte = CanhotosLeitura.CropImage(bp, coor.X, coor.Y, coor.W, coor.H);
                //    arquivoCorte.Save(@"Crop\" + coor.X + ".png");
                //    bp.Dispose();







                //    // corteCaracteres
                //    List<String> datas = new List<string>();
                //    var array = cortesScript(@"Crop\" + coor.X + ".png", @"C:\PythonScript\corteCaracteres.py");
                //    foreach (var arrayScript in array)
                //    {
                //        var posicoesScript = arrayScript.Split(' ');
                //        if (posicoesScript.Length > 2)
                //        {

                //            var coord = new Coordenadas()
                //            {
                //                X = Convert.ToInt16(posicoesScript[0]),
                //                Y = Convert.ToInt16(posicoesScript[1]),
                //                W = Convert.ToInt16(posicoesScript[2]),
                //                H = Convert.ToInt16(posicoesScript[3])

                //            };
                //            var bpp = Bitmap.FromFile(@"Crop\" + coor.X + ".png");

                //            var arquivoCortes = CanhotosLeitura.CropImage(bpp, coord.X, coord.Y, coord.W, coord.H);
                //            Bitmap resized = new Bitmap(arquivoCortes, new Size(32, 32));

                //            resized.Save(@"CropScript\" + coord.X + ".png");
                //            datas.Add(@"CropScript\" + coord.X + ".png");
                //            bpp.Dispose();


                //        }
                //    }
                //    var aqruivos = datas.OrderByDescending(_s => _s);

                //    int cc = 0;

                //    foreach (var item in aqruivos)
                //    {
                //        string arquivo = Guid.NewGuid().ToString();
                //        if (verifica < 1) /// significa que é dia 
                //        {
                //            if (cc <= 1)
                //            {
                //                File.Move(item, @"C:\ConsoleApplication18\ConsoleApplication18\bin\Debug\CropScript\Entrada\" + arquivo + ".png");
                //                var sair = true;
                //                while (sair)
                //                {
                //                    var processamento = Directory.GetFiles(@"C:\ConsoleApplication18\ConsoleApplication18\bin\Debug\CropScript\saida\");
                //                    foreach (var ar in processamento)
                //                    {
                //                        if (ar.Contains(arquivo))
                //                        {
                //                            sair = false;
                //                            var numero = Path.GetFileNameWithoutExtension( ar).Split('#');
                //                            if (numero.Count() > 1)
                //                            {
                //                                dia += numero[1];
                //                            }
                //                            break;
                //                        }
                //                    }
                //                    //{ }
                //                }

                //            }
                //        }
                //        else if (verifica == 1) /// significa que é dimesa 
                //        {
                //            if (cc <= 1)
                //            {
                //                File.Move(item, @"C:\ConsoleApplication18\ConsoleApplication18\bin\Debug\CropScript\Entrada\" + arquivo + ".png");
                //                var sair = true;
                //                while (sair)
                //                {
                //                    var processamento = Directory.GetFiles(@"C:\ConsoleApplication18\ConsoleApplication18\bin\Debug\CropScript\saida\");
                //                    foreach (var ar in processamento)
                //                    {
                //                        if (ar.Contains(arquivo))
                //                        {
                //                            sair = false;
                //                            var numero = Path.GetFileNameWithoutExtension(ar).Split('#');
                //                            if (numero.Count() > 1)
                //                            {
                //                                dia += numero[1];
                //                            }
                //                            break;
                //                        }
                //                    }
                //                    //{ }
                //                }
                //            }

                //        }
                //        else if (verifica == 2) /// significa que é ano 
                //        {
                //            if (cc <= 3)
                //            {
                //                File.Move(item, @"C:\ConsoleApplication18\ConsoleApplication18\bin\Debug\CropScript\Entrada\" + arquivo + ".png");
                //                var sair = true;
                //                while (sair)
                //                {
                //                    var processamento = Directory.GetFiles(@"C:\ConsoleApplication18\ConsoleApplication18\bin\Debug\CropScript\saida\");
                //                    foreach (var ar in processamento)
                //                    {
                //                        if (ar.Contains(arquivo))
                //                        {
                //                            sair = false;
                //                            var numero = Path.GetFileNameWithoutExtension(ar).Split('#');
                //                            if (numero.Count() > 1)
                //                            {
                //                                dia += numero[1];
                //                            }
                //                            break;
                //                        }
                //                    }
                //                    //{ }
                //                }
                //            }
                //        }
                //        cc++;
                //    }




                //    ///  arquivoCorte.Save(@"C:\Users\Administrador\Documents\Visual Studio 2015\Projects\ConsoleApplication18\ConsoleApplication18\bin\Debug\Crop\" + Corte + ".png");
                //    MemoryStream ms = new MemoryStream();
                //    arquivoCorte.Save(ms, ImageFormat.Png);
                //    byte[] bmpBytes = ms.ToArray();
                //    PosicaoLabel pol = new PosicaoLabel();
                //    pol.Coordenadas = coor;
                //    // pol.Label = ocr.executarOcr(bmpBytes).ToString();


                //    arquivoCorte.Dispose();
                //    ms.Close();
                //    verifica++;
                //}
            }
            foreach (var item in Directory.GetFiles(@"Crop\"))
            {
                File.Delete(item);
            }
            dia = Regex.Replace(dia, " ", "");
            if (tipo.ToUpper() == "DIA")
            {
                int dd = 0;
                if (dia.Length == 1 || dia.Length > 2)
                {
                    dia = dt.Day.ToString();
                }
                else
                {
                    int sd = 0;
                    if (!Int32.TryParse(dia, out sd))
                    {

                        dia = dt.Day.ToString();
                    }
                    else
                    {
                        if (sd > 31) { dia = dt.Day.ToString(); }
                    }
                }
            }

            if (tipo.ToUpper() == "MÊS")
            {
                int dd = 0;
                if (dia.Length == 1 || dia.Length > 2)
                {
                    dia = "0" + dt.Month.ToString();
                }
                else
                {
                    int sd = 0;
                    if (!Int32.TryParse(dia, out sd))
                    {

                        dia = "0" + dt.Month.ToString();
                    }
                    else
                    {
                        if (sd > 12) { dia = "0" + dt.Month.ToString(); }
                    }
                }
            }

            if (tipo.ToUpper() == "ANO")
            {
                dia = dia.Replace("  ", "");
                dia = Regex.Replace(dia, " ", "");
                int d = 0;
                string retornoAno = "";
                if (int.TryParse(dia, out d))
                {
                    string ano = DateTime.Now.Year.ToString();

                    var dincompketo = dia.Substring(0, 3);

                    var anocompketo = ano.Substring(0, 3);
                    if (Convert.ToInt32(dincompketo) > Convert.ToInt32(anocompketo))
                    {
                        retornoAno += anocompketo;
                    }

                    retornoAno += dia[3];
                    dia = retornoAno;
                }
                else
                {

                    if (dt != null)
                    {
                        dia = dt.Year.ToString();
                    }
                }
            }

            return dia;

        }



        public object C004(object parametros, EnumEntrada entrada)
        {

            Object[] obj = (Object[])parametros;

            RetornoEventoDTO retornoEventoDTO = new RetornoEventoDTO();
            switch (entrada)
            {

                case EnumEntrada.IMAGEM:
                    var imagemvalor = (System.Drawing.Bitmap)obj[0];

                    oGdPictureImaging.SetLicenseNumber("4118106456693265856441854");
                    var imagemId = oGdPictureImaging.CreateGdPictureImageFromBitmap((imagemvalor));
                    var caminhoImagem = Guid.NewGuid() + ".png";
                    oGdPictureImaging.SaveAsPNG(imagemId, caminhoImagem);

                    var retorno = corte(caminhoImagem, @"C:\PythonScript\corteTegma.py", (string)obj[1], (Image)obj[2]);
                    var imagem = oGdPictureImaging.IsBlank(imagemId, 90);
                    imagemvalor.Dispose();
                    if (imagem)
                    {
                        retornoEventoDTO.Retono = "Não possui assinatura";
                        retornoEventoDTO.RetornoTipo = EnumRetornoEvento.RETORNO_STRING;
                    }
                    else
                    {
                        retornoEventoDTO.Retono = retorno;
                        retornoEventoDTO.RetornoTipo = EnumRetornoEvento.RETORNO_STRING;
                    }
                    retornoEventoDTO.Retono = retorno;
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
