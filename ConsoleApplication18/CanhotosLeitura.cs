using ConsoleApplication18.Evento;
using ConsoleApplication18.OcrImpletentacao;
using GdPicture;
using Google.Cloud.Vision.V1;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using ZXing;

namespace ConsoleApplication18
{
    public class CanhotosLeitura
    {
        private static readonly HttpClient client = new HttpClient();
        public CanhotosLeitura()
        {
        }
        //   private static IOcr ocr = Factory.FactoryOcr.FactoryOcrInstancia();
        public static Modelo verificaModelo(StringBuilder sb, int projetoId, int pagina, System.Drawing.Bitmap img = null)
        {

            //oGdPictureImaging.SetLicenseNumber("4118106456693265856441854");
            //int maior = 0;
            //Modelo retorno = null;
            //try
            //{

            //    Entidade entidade = new Entidade();
            //    var modelosPermitidos = entidade.PROJETO_MODELO.Where(_s => _s.PROJETO_ID == projetoId).Select(_s => _s.MODELO_ID).ToArray();


            //    var modelos = entidade.Modelo.Where(_modelo => modelosPermitidos.Contains(_modelo.ID)).ToList();


            //    foreach (var item in modelos)
            //    {


            //        int verifica = 0;
            //        int check = 0;

            //        //  var palavras = entidade.Palavras.Where(_pal => _pal.IdModelo == item.ID).ToList();
            //        List<Palavras> palavras = (
            //       from _pal in entidade.Palavras
            //       where _pal.IdModelo == (int?)item.ID
            //       select _pal).ToList<Palavras>();
            //        foreach (var palavrasmn in palavras)
            //        {
            //            if (palavrasmn.Contem.Value)
            //            {

            //                var quebra = sb.Replace('\n', ' ').ToString().ToUpper().Split(' ');
            //                foreach (var split in quebra)
            //                {
            //                    if (split.ToUpper().Equals(palavrasmn.Palavra.ToUpper()))
            //                    {
            //                        verifica++;

            //                    }

            //                }

            //            }
            //            else
            //            {

            //                if (sb.ToString().ToUpper().Contains(palavrasmn.Palavra.ToUpper()))
            //                {
            //                    verifica = 0;
            //                    continue;
            //                    //retorno = null;
            //                    // break;
            //                }
            //            }
            //            if (palavrasmn.Barcode != null && palavrasmn.Barcode.Value)
            //            {
            //                List<string> arqu = new List<string>();
            //                var imagemId = oGdPictureImaging.CreateGdPictureImageFromFile(arquivo);
            //                oGdPictureImaging.Barcode1DReaderDoScan(imagemId);
            //                var count = oGdPictureImaging.Barcode1DReaderGetBarcodeCount();
            //                for (int i = 0; i < count; i++)
            //                {
            //                    var resultado = oGdPictureImaging.Barcode1DReaderGetBarcodeValue(i+1);
            //                    arqu.Add(resultado);
            //                }
            //                oGdPictureImaging.ReleaseGdPictureImage(imagemId);
            //                if (arqu.Where(_s => _s == palavrasmn.Palavra).Count() > 0)
            //                {
            //                    verifica++;
            //                }
            //                else
            //                {
            //                   // verifica = 0;
            //                }
            //            }
            //        }
            //        ////achou qual modelo
            //        if (verifica > check && verifica > maior)
            //        {
            //            maior = verifica;
            //            check = verifica;
            //            retorno = null;
            //            retorno = item;
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    string path = @"C:\stack.txt";
            //    if (!File.Exists(path))
            //    {
            //        File.Create(path).Dispose();
            //        using (TextWriter tw = new StreamWriter(path))
            //        {

            //            tw.WriteLine(ex.StackTrace);


            //            tw.Close();
            //        }

            //    }

            //    else if (File.Exists(path))
            //    {
            //        using (TextWriter tw = new StreamWriter(path))
            //        {
            //            tw.WriteLine(ex.StackTrace);
            //            tw.Close();
            //        }
            //    }

            //}
            //return retorno;
            // public static Modelo verificaModelo(StringBuilder sb, int projetoId, int pagina, System.Drawing.Bitmap img = null)
            {
                Modelo retorno = null;
                Entidade entidade = new Entidade();
                var projeto = entidade.PROJETO.Where(_s => _s.ID == projetoId).FirstOrDefault();
                if (projeto.utilizarMineracao.Value)
                {



                    string atibrutos = "";
                    var modelosAtributos = entidade.PROJETO_MODELO.Where(_s => _s.PROJETO_ID == projetoId).ToList();
                    for (int i = 0; i < modelosAtributos.Count(); i++)
                    {
                        atibrutos += modelosAtributos[i].Modelo.nome;
                        if (i + 1 < modelosAtributos.Count())
                        {
                            atibrutos += ",";
                        }
                    }
                    atibrutos += ",NAO_CLASSIFICADO";
                    var idArquivo = projeto.arquivo.Split('_')[2];
                    var values = new Dictionary<string, string>
                {
                   { "projetoId", projetoId.ToString() },
                   { "elementos",atibrutos },
                   { "arquivo",sb.ToString() },
                        { "Id",  idArquivo }
                };

                    var content = new FormUrlEncodedContent(values);

                    var response = client.PostAsync("http://35.192.175.82:8080/webmineracaoHttp/IdentificarArquivo", content);
                    var retornov = response.Result.Content.ReadAsStringAsync();

                    if (retornov.Result != "")
                    {
                        var procura = retornov.Result.Replace('$', ' ');
                        retorno = entidade.Modelo.Where(_s => _s.nome == procura).FirstOrDefault();
                    }
                }
                else
                {




                    int maior = 0;

                    try
                    {

                        var modelosPermitidos = entidade.PROJETO_MODELO.Where(_s => _s.PROJETO_ID == projetoId).Select(_s => _s.MODELO_ID).ToArray();


                        var modelos = entidade.Modelo.Where(_modelo => modelosPermitidos.Contains(_modelo.ID)).ToList();


                        foreach (var item in modelos)
                        {


                            int verifica = 0;
                            int check = 0;

                            //  var palavras = entidade.Palavras.Where(_pal => _pal.IdModelo == item.ID).ToList();
                            List<Palavras> palavras = (
                           from _pal in entidade.Palavras
                           where _pal.IdModelo == (int?)item.ID
                           select _pal).ToList<Palavras>();
                            foreach (var palavrasmn in palavras)
                            {
                                if (palavrasmn.QRcode.Value)
                                {
                                    /////verificar o que tem no QR code
                                    ///se o resultado do qr bater comn a palvra pesquisda clasifica
                                    ///

                                    if (palavrasmn.pagina.Value == pagina)
                                    {
                                        oGdPictureImaging.SetLicenseNumber("4118106456693265856441854");

                                        int imageID = oGdPictureImaging.CreateGdPictureImageFromBitmap(img);
                                        if (imageID != 0)
                                        {
                                            //Step3: Barcode recognition from GdPicture Image

                                            oGdPictureImaging.BarcodeQRReaderDoScan(imageID, BarcodeQRReaderScanMode.BestQuality);
                                            oGdPictureImaging.ReleaseGdPictureImage(imageID);
                                            bool verificaQR = false;
                                            string valueQrcode = "";
                                            for (int i = 1; i <= oGdPictureImaging.BarcodeQRReaderGetBarcodeCount(); i++)
                                            {

                                                verificaQR = true;
                                                if (palavrasmn.GuardarValorIndexacao.Value)
                                                {
                                                    valueQrcode = oGdPictureImaging.BarcodeQRReaderGetBarcodeValue(i);
                                                }
                                                verifica++;
                                            }
                                            oGdPictureImaging.BarcodeQRReaderClear();

                                        }
                                    }

                                }

                                if (palavrasmn.Contem.Value)
                                {
                                    int quantidadeCaractere = 0;
                                    var quebra = sb.Replace('\n', ' ').ToString().ToUpper().Split(' ');
                                    foreach (var split in quebra)
                                    {

                                        int quantidade = split.Length;
                                        int ondeparouSplit = 0;

                                        var quebraSplit = split.ToUpper().ToCharArray();
                                        var palavrasSplit = palavrasmn.Palavra.ToUpper().ToCharArray();
                                        for (int i = 0; i < palavrasSplit.Length; i++)
                                        {

                                            if (palavrasSplit.Count() <= quebraSplit.Count())
                                            {
                                                if (quebraSplit[i] == palavrasSplit[i])
                                                {

                                                    quantidadeCaractere++;
                                                }
                                            }

                                        }


                                        if ((palavrasmn.porcentagemmax == 100 && palavrasmn.porcentagemMin == 100) || (palavrasmn.porcentagemmax == 0 && palavrasmn.porcentagemMin == 0))
                                        {

                                            if (split.ToUpper().Equals(palavrasmn.Palavra.ToUpper()))
                                            {
                                                verifica++;

                                            }
                                        }
                                        else
                                        {
                                            var porcentagemlrv = ((double)quantidadeCaractere / quantidade) * 100;


                                            if (palavrasmn.porcentagemMin >= porcentagemlrv && palavrasmn.porcentagemmax <= porcentagemlrv)
                                            {
                                                verifica++;
                                                quantidadeCaractere = 0;
                                            }



                                        }



                                    }

                                }
                                /////contem barcode ou qrcode
                                if (palavrasmn.Barcode.Value || palavrasmn.QRcode.Value)
                                {

                                }
                                else if (!palavrasmn.Contem.Value)
                                {

                                    if (sb.ToString().ToUpper() == (palavrasmn.Palavra.ToUpper()))
                                    {
                                        verifica = 0;
                                        continue;

                                    }
                                }

                            }
                            ////achou qual modelo
                            if (verifica > check && verifica > maior)
                            {
                                maior = verifica;
                                check = verifica;
                                retorno = null;
                                retorno = item;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string path = @"C:\stack.txt";
                        if (!File.Exists(path))
                        {
                            File.Create(path).Dispose();
                            using (TextWriter tw = new StreamWriter(path))
                            {

                                tw.WriteLine(ex.StackTrace);


                                tw.Close();
                            }

                        }

                        else if (File.Exists(path))
                        {
                            using (TextWriter tw = new StreamWriter(path))
                            {
                                tw.WriteLine(ex.StackTrace);
                                tw.Close();
                            }
                        }

                    }
                    if (img != null)
                        img.Dispose();
                }
                return retorno;

            }

        }



        public static StringBuilder DetectText(Google.Cloud.Vision.V1.Image image)
        {
            StringBuilder sb = new StringBuilder();
            var client = ImageAnnotatorClient.Create();
            //Thread.Sleep(20000);
            var response = client.DetectText(image);
            foreach (var annotation in response)
            {
                if (annotation.Description != null)
                {
                    sb.Append(annotation.Description + " ");
                    Console.WriteLine(annotation.Description);
                }
            }


            return sb;
        }


        public static byte[] ImageToByte(System.Drawing.Image img)
        {
            ImageConverter converter = new ImageConverter();
            var retorno = (byte[])converter.ConvertTo(img, typeof(byte[]));

            return retorno;
            /// img.Dispose();
        }

        private static String capturarIndices(byte[] imagen, int modeloId, int projetoid)
        {
            string nmimg = Guid.NewGuid() + ".png";
            using (var image = System.Drawing.Image.FromStream(new MemoryStream(imagen)))
            {
                image.Save(nmimg, ImageFormat.Png);  // Or Png
            }
            IOcr ocr = Factory.FactoryOcr.FactoryOcrInstancia(projetoid);
            var coordenadasModelo = new Entidade().OrientacaoDocumento.Where(_s => _s.IdModelo == modeloId).ToList();
            //  Thread.Sleep(20000);
            //  Google.Cloud.Vision.V1.Image image = Google.Cloud.Vision.V1.Image.FromBytes(imagen);
            List<string> retorno = new List<string>();
            List<String> sb = new List<String>();

            //var client = ImageAnnotatorClient.Create();

            var response = ocr.executarOcr(nmimg);//client.DetectText(image);
            File.Delete(nmimg);
            foreach (var item in response.ToString().Split(' '))
            {
                sb.Add(item);
            }

            //String textocgane = "";
            //foreach (var annotation in response)
            //{
            //    if (annotation.Description != null)
            //    {
            //        sb.Add(annotation.Description + " ");

            //        textocgane += annotation.Description.Replace(" ", "");
            //    }
            //}

            StringBuilder retonoIndex = new StringBuilder();
            foreach (var coordenadas in coordenadasModelo)
            {

                int inde = 0;
                if (coordenadas.QrCode != null && coordenadas.QrCode.Value)
                {
                    for (int z = 0; z <= 1; z++)
                    {

                        oGdPicturePDF.SetLicenseNumber("4118106456693265856441854");
                        oGdPictureImaging.SetLicenseNumber("4118106456693265856441854");

                        int imgteste = oGdPictureImaging.CreateGdPictureImageFromByteArray(imagen);
                        if (z == 1)
                        {
                            oGdPictureImaging.Rotate(imgteste, RotateFlipType.Rotate180FlipNone);
                        }
                        oGdPictureImaging.BarcodeDataMatrixReaderDoScan(imgteste, BarcodeDataMatrixReaderScanMode.BestQuality);
                        oGdPictureImaging.ReleaseGdPictureImage(imgteste);


                        //if (oGdPictureImaging.BarcodeDataMatrixReaderGetBarcodeCount() == 0)
                        //{
                        //    String rtn = "_";


                        //    string total = "-";
                        //    for (int v = 0; v < coordenadas.Tamanho; v++)
                        //    {
                        //        total += "-";
                        //    }
                        //    rtn = total;
                        //    retorno.Add(rtn + "_");
                        //}


                        for (int i = 1; i <= oGdPictureImaging.BarcodeDataMatrixReaderGetBarcodeCount(); i++)
                        {

                            var d = oGdPictureImaging.BarcodeDataMatrixReaderGetBarcodeValue(i);

                            Regex regex = new Regex(coordenadas.Regex.ToUpper());
                            Match matchReplace = regex.Match(d);
                            if (matchReplace.Success)
                            {
                                int start = d.IndexOf(coordenadas.Regex);
                                string rxxx = d.Substring(start, d.Length - d.IndexOf(coordenadas.Regex)).Replace("|", "");

                                retorno.Add(rxxx + "_");


                                oGdPictureImaging.BarcodeDataMatrixReaderClear();
                                StringBuilder sbdd = new StringBuilder();
                                foreach (var item in retorno.Distinct().ToList())
                                {
                                    sbdd.Append(item);
                                }


                                return sbdd.ToString();
                            }
                        }
                        if (oGdPictureImaging.BarcodeDataMatrixReaderGetBarcodeCount() > 0)
                        {
                            break;
                        }
                        else
                        {

                            String rtn = "_";


                            string total = "-";
                            for (int v = 0; v < coordenadas.Tamanho; v++)
                            {
                                total += "-";
                            }
                            rtn = total;
                            retorno.Add(rtn + "_");

                        }

                    }
                }
                else if (coordenadas.Barcode != null && coordenadas.Barcode.Value)
                {

                }
                else
                {
                    int idx = 0;
                    foreach (var texto in sb)
                    {


                        if (coordenadas.Regex != null && coordenadas.Regex != "")
                        {
                            Regex regex = new Regex(coordenadas.Regex);
                            Match match = regex.Match(texto.ToString().ToUpper());
                            if (match.Success)
                            {
                                if (coordenadas.SomenteNumeros.Value)
                                {
                                    if (coordenadas.Tamanho == texto.Trim().ToString().Length)
                                    {

                                        retorno.Add(texto.Trim() + "_");

                                    }
                                }
                                ////numeroes e letras
                                else
                                {
                                    Match matchReplace = regex.Match(response.ToString());
                                    if (matchReplace.Success)
                                    {
                                        //string rxxx = texto.Replace("\n", string.Empty).Substring(matchReplace.Index, texto.Replace("\n", string.Empty).Length - matchReplace.Index);


                                        //string output = Regex.Replace(rxxx, coordenadas.Regex, " ").TrimStart();

                                        //string resultfinal = output.Substring(0, output.IndexOf(""));
                                        //if (resultfinal == "")
                                        //{


                                        //    if (output != "")
                                        //    {
                                        //        if (output.Contains("\n"))
                                        //        {
                                        //            resultfinal = output.Substring(0, output.IndexOf("\n"));
                                        //        }
                                        //        //else
                                        //        //{
                                        //        //    resultfinal = output;
                                        //        //}
                                        //    }
                                        //}
                                        //string replaceWith = "";
                                        //string re = resultfinal.Replace("\r\n", replaceWith).Replace("\n", replaceWith).Replace("\r", replaceWith);
                                        retorno.Add(matchReplace.Value + "_");
                                        ///alterado
                                        break;
                                    }
                                    // string result = regex.Replace(coordenadas.Regex, substring);
                                    //Console.WriteLine(texto);
                                    //if (inde + 1 < sb.Count())
                                    //{


                                    //    if (coordenadas.Tamanho == sb[inde + 1].Trim().ToString().Length)
                                    //    {
                                    //        retorno.Add(coordenadas.Palavra + "_" + sb[inde + 1].Trim());
                                    //        break;
                                    //    }
                                    //}
                                }
                            }


                            inde++;


                        }
                        else
                        {
                            idx++;
                            retorno.Add(texto.Trim().ToString());

                        }
                    }
                }

            }

            StringBuilder sbd = new StringBuilder();
            foreach (var item in retorno.Distinct().ToList())
            {
                sbd.Append(item + " ");
            }

            return sbd.ToString();

        }

        private static String capturarIndices(byte[] imagen, int modeloId, int projetoid, OrientacaoDocumento coordenadas)
        {
            string nmimg = Guid.NewGuid() + ".png";
            using (var image = System.Drawing.Image.FromStream(new MemoryStream(imagen)))
            {
                image.Save(nmimg, ImageFormat.Png);  // Or Png
            }
            IOcr ocr = Factory.FactoryOcr.FactoryOcrInstancia(projetoid);
            var coordenadasModelo = new Entidade().OrientacaoDocumento.Where(_s => _s.IdModelo == modeloId).ToList();
            //  Thread.Sleep(20000);
            //  Google.Cloud.Vision.V1.Image image = Google.Cloud.Vision.V1.Image.FromBytes(imagen);
            List<string> retorno = new List<string>();
            List<String> sb = new List<String>();

            //var client = ImageAnnotatorClient.Create();

            var response = ocr.executarOcr(nmimg);//client.DetectText(image);
            File.Delete(nmimg);
            foreach (var item in response.ToString().Split(' '))
            {
                sb.Add(item);
            }

            //String textocgane = "";
            //foreach (var annotation in response)
            //{
            //    if (annotation.Description != null)
            //    {
            //        sb.Add(annotation.Description + " ");

            //        textocgane += annotation.Description.Replace(" ", "");
            //    }
            //}

            StringBuilder retonoIndex = new StringBuilder();


            int inde = 0;
            if (coordenadas.QrCode != null && coordenadas.QrCode.Value)
            {
                for (int z = 0; z <= 1; z++)
                {

                    oGdPicturePDF.SetLicenseNumber("4118106456693265856441854");
                    oGdPictureImaging.SetLicenseNumber("4118106456693265856441854");

                    int imgteste = oGdPictureImaging.CreateGdPictureImageFromByteArray(imagen);

                    var memeoryStre = new MemoryStream();
                    var barcodeReader = new BarcodeReader();

                    oGdPictureImaging.SaveAsStream(imgteste, memeoryStre, DocumentFormat.DocumentFormatPNG, 0);

                    // create an in memory bitmap
                    var barcodeBitmap = (Bitmap)Bitmap.FromStream(memeoryStre);
                    barcodeReader.Options.TryHarder = true;
                    // decode the barcode from the in memory bitmap
                    var cd = barcodeReader.Decode(barcodeBitmap);
                    string barcodeResult = "";
                    if (cd != null)
                    {
                        barcodeResult = cd.Text;
                    }
                   



                    if (barcodeResult != null && barcodeResult != "")
                    {
                        retorno.Add(barcodeResult.Trim() + "_"); break;
                    }
                    oGdPictureImaging.BarcodeDataMatrixReaderDoScan(imgteste, BarcodeDataMatrixReaderScanMode.BestQuality);
                    oGdPictureImaging.ReleaseGdPictureImage(imgteste);


                    //if (oGdPictureImaging.BarcodeDataMatrixReaderGetBarcodeCount() == 0)
                    //{
                    //    String rtn = "_";


                    //    string total = "-";
                    //    for (int v = 0; v < coordenadas.Tamanho; v++)
                    //    {
                    //        total += "-";
                    //    }
                    //    rtn = total;
                    //    retorno.Add(rtn + "_");
                    //}


                    for (int i = 1; i <= oGdPictureImaging.BarcodeDataMatrixReaderGetBarcodeCount(); i++)
                    {

                        var d = oGdPictureImaging.BarcodeDataMatrixReaderGetBarcodeValue(i);

                        Regex regex = new Regex(coordenadas.Regex.ToUpper());
                        Match matchReplace = regex.Match(d);
                        if (matchReplace.Success)
                        {
                            int start = d.IndexOf(coordenadas.Regex);
                            string rxxx = d.Substring(start, d.Length - d.IndexOf(coordenadas.Regex)).Replace("|", "");

                            retorno.Add(rxxx + "_");


                            oGdPictureImaging.BarcodeDataMatrixReaderClear();
                            StringBuilder sbdd = new StringBuilder();
                            foreach (var item in retorno.Distinct().ToList())
                            {
                                sbdd.Append(item);
                            }


                            return sbdd.ToString();
                        }
                    }
                    if (oGdPictureImaging.BarcodeDataMatrixReaderGetBarcodeCount() > 0)
                    {
                        break;
                    }
                    else
                    {

                        String rtn = "_";


                        string total = "-";
                        for (int v = 0; v < coordenadas.Tamanho; v++)
                        {
                            total += "-";
                        }
                        rtn = total;
                        retorno.Add(rtn + "_");

                    }

                }
            }
            else if (coordenadas.Barcode != null && coordenadas.Barcode.Value)
            {

            }
            else
            {
                int idx = 0;
                List<String> TXT = new List<string>();
                foreach (var texto in sb)
                {

                    if (texto.Contains("\n"))
                    {

                        var textSplitado = texto.Split(new string[] { "\n" }, StringSplitOptions.None);
                        foreach (var item in textSplitado)
                        {
                            TXT.Add(item);
                        }



                    }
                    else
                    {
                        TXT.Add(texto);
                    }
                }
                foreach (var texto in TXT)
                {


                    if (coordenadas.Regex != null && coordenadas.Regex != "")
                    {
                        Regex regex = new Regex(coordenadas.Regex);
                        Match match = regex.Match(texto.ToString().ToUpper());
                        if (match.Success)
                        {
                            if (coordenadas.SomenteNumeros.Value)
                            {
                                if (coordenadas.Tamanho == texto.Trim().ToString().Length)
                                {

                                    retorno.Add(texto.Trim() + "_");

                                }
                            }
                            ////numeroes e letras
                            else
                            {
                                Match matchReplace = regex.Match(response.ToString());
                                if (matchReplace.Success)
                                {
                                    //string rxxx = texto.Replace("\n", string.Empty).Substring(matchReplace.Index, texto.Replace("\n", string.Empty).Length - matchReplace.Index);


                                    //string output = Regex.Replace(rxxx, coordenadas.Regex, " ").TrimStart();

                                    //string resultfinal = output.Substring(0, output.IndexOf(""));
                                    //if (resultfinal == "")
                                    //{


                                    //    if (output != "")
                                    //    {
                                    //        if (output.Contains("\n"))
                                    //        {
                                    //            resultfinal = output.Substring(0, output.IndexOf("\n"));
                                    //        }
                                    //        //else
                                    //        //{
                                    //        //    resultfinal = output;
                                    //        //}
                                    //    }
                                    //}
                                    //string replaceWith = "";
                                    //string re = resultfinal.Replace("\r\n", replaceWith).Replace("\n", replaceWith).Replace("\r", replaceWith);
                                    retorno.Add(matchReplace.Value);
                                    ///alterado
                                    break;
                                }
                                // string result = regex.Replace(coordenadas.Regex, substring);
                                //Console.WriteLine(texto);
                                //if (inde + 1 < sb.Count())
                                //{


                                //    if (coordenadas.Tamanho == sb[inde + 1].Trim().ToString().Length)
                                //    {
                                //        retorno.Add(coordenadas.Palavra + "_" + sb[inde + 1].Trim());
                                //        break;
                                //    }
                                //}
                            }
                        }


                        inde++;


                    }
                    else
                    {
                        idx++;
                        retorno.Add(texto.Trim().ToString());

                    }
                }
            }


            StringBuilder sbd = new StringBuilder();
            foreach (var item in retorno.Distinct().ToList())
            {
                sbd.Append(item + " ");
            }

            return sbd.ToString();

        }


        private static GdPictureImaging oGdPictureImaging = new GdPictureImaging();
        private static GdPicturePDF oGdPicturePDF = new GdPicturePDF();




        public static Bitmap CropImage(System.Drawing.Image source, int x, int y, int width, int height)
        {



            Rectangle crop = new Rectangle(x, y, width, height);
            System.Drawing.Image x2 = (System.Drawing.Image)source.Clone();

            var bmp = new Bitmap(crop.Width, crop.Height);
            bmp.SetResolution(300, 300);
            using (var gr = Graphics.FromImage(bmp))
            {
                gr.DrawImage(x2, new Rectangle(0, 0, bmp.Width, bmp.Height), crop, GraphicsUnit.Pixel);

            }
            x2.Dispose();
            //  bmp.Save("teste.png");
            return bmp;
        }
        public static List<OrientacaoDocumento> GetCoordenadasdoModelo(int modelo, List<PalavrasCoordenadasDocumento> palavrasItem)
        {
            var db = new Entidade();
            string caminhoModeloAr = ConfigurationManager.AppSettings["caminhoModelo"];
            List<OrientacaoDocumento> retorno = new List<OrientacaoDocumento>();

            ///captura as posições do modelo, junto com as palavras
            var informacoesModelo = db.PalavrasCoordenadasDocumento.Where(_s => _s.IDMODELO == modelo).ToList();/// ocr.executarOcrCoordenadas(caminhoModelo);
            string k = "";
            foreach (var item in palavrasItem)
            {
                k += item.texto + "|";
            }
            using (FileStream fs = File.Create(@"c:\palavrasevento.txt"))
            {
                Byte[] info = new UTF8Encoding(true).GetBytes(k);
                // Add some information to the file.
                fs.Write(info, 0, info.Length);
            }

            ////verifica no banco de dados qual a é a palvra que vai servir como ancora. substituir depois por treinamento do modelo para evitar 
            ///chamadas duplicadas
            var AncoraPalavra = db.Palavras.Where(_s => _s.IdModelo == modelo && _s.Ancora == true).FirstOrDefault();

            using (FileStream fs = File.Create(@"c:\achouancora.txt"))
            {
                Byte[] info = new UTF8Encoding(true).GetBytes(AncoraPalavra.Palavra);
                // Add some information to the file.
                fs.Write(info, 0, info.Length);
            }
            var ancora = informacoesModelo.Where(_s => _s.texto.ToUpper().Contains(AncoraPalavra.Palavra.ToUpper())).FirstOrDefault();

            using (FileStream fs = File.Create(@"c:\ancora.txt"))
            {
                //Byte[] info = new UTF8Encoding(true).GetBytes(ancora.texto);
                //// Add some information to the file.
                //fs.Write(info, 0, info.Length);
            }
            if (ancora == null)
                foreach (var item in informacoesModelo)
                {

                    ///achei a ancora ou a apalvra que será a ancora.
                    using (FileStream fs = File.Create(@"c:\achouchave.txt"))
                    {
                        Byte[] info = new UTF8Encoding(true).GetBytes(AncoraPalavra.Palavra.ToUpper());
                        // Add some information to the file.
                        fs.Write(info, 0, info.Length);
                    }

                    ////quantidade da ancora
                    int qtdOriginal = AncoraPalavra.Palavra.ToUpper().Count();
                    int quantidadeiguais = 0;
                    ////siginifica que tem o mesmonumero de letras
                    if (item.texto.ToUpper().Trim().Count() == AncoraPalavra.Palavra.ToUpper().Trim().Count())
                    {

                        for (int i = 0; i < item.texto.ToUpper().Trim().Count(); i++)
                        {
                            if (item.texto.ToUpper()[i] == AncoraPalavra.Palavra.ToUpper()[i])
                            {
                                quantidadeiguais++;
                            }
                        }
                        double percentage = ((double)quantidadeiguais / (double)qtdOriginal) * 100;

                        if (percentage > 80)
                        {
                            ////é a plavra
                            ancora = item;
                        }
                        //foreach (var chAncora in AncoraPalavra.Palavra.ToUpper().Trim().ToCharArray())
                        //{
                        //    foreach (var  chbusca in item.texto.ToUpper().Trim().ToCharArray())
                        //    {
                        //        if(chAncora == chbusca)
                        //        {
                        //            quantidadeiguais++;
                        //        }
                        //    }
                        //}
                    }
                }

            ///captura as coordenadas do modelo
            var coordenadasModelo = new Entidade().OrientacaoDocumento.Where(_s => _s.IdModelo == modelo).ToList();
            if (ancora != null)
            {
                foreach (var coordenadas in coordenadasModelo)
                {
                    Documento doc = new Documento();
                    OrientacaoDocumento orienta = new OrientacaoDocumento();
                    orienta.x = coordenadas.x - ancora.x;
                    orienta.y = coordenadas.y - ancora.y;
                    orienta.width = coordenadas.width - ancora.width;
                    orienta.height = coordenadas.height - ancora.height;

                    ////tem aque achar a posicao  da ancora do item
                    var orientaacaoItem = palavrasItem.Where(_s => _s.texto.ToUpper().Equals(AncoraPalavra.Palavra.ToUpper())).FirstOrDefault();
                    if (orientaacaoItem == null)
                    {
                        continue;
                    }
                    OrientacaoDocumento or = new OrientacaoDocumento();
                    or.nome = coordenadas.Palavra;
                    or.Regex = coordenadas.Regex;
                    or.x = orientaacaoItem.x + orienta.x;
                    or.SomenteNumeros = coordenadas.SomenteNumeros;

                    or.y = orientaacaoItem.y + orienta.y;
                    or.width = orientaacaoItem.width + orienta.width;
                    or.height = orientaacaoItem.height + orienta.height;
                    or.Tamanho = coordenadas.Tamanho;
                    or.Evento = coordenadas.Evento;
                    or.Barcode = coordenadas.Barcode;
                    or.EspacoBranco = coordenadas.EspacoBranco;
                    or.QrCode = coordenadas.QrCode;
                    or.PARAMETROENTRADA = coordenadas.PARAMETROENTRADA;
                    retorno.Add(or);
                }
            }
            return retorno;

        }

        public static bool IsFileLocked(string filePath)
        {
            try
            {
                using (File.Open(filePath, FileMode.Open)) { }
            }
            catch (IOException e)
            {
                var errorCode = Marshal.GetHRForException(e) & ((1 << 16) - 1);

                return errorCode == 32 || errorCode == 33;
            }

            return false;
        }





        public static Dictionary<string, string> indicesAnalise(string modeloMove, Modelo modeloAchado, string conteudo, string conteudoposition)
        {
            Dictionary<string, string> conteudoArq = new Dictionary<string, string>();

            foreach (var w in modeloAchado.PROJETO_MODELO)
            {
                var pl = new Entidade().Tipo_Regex_Extracao.Where(_s => _s.IdProjetoModelo == w.ID).ToList();
                var extracao = pl.Select(_s => _s.TipoExtracaoId).Distinct().ToList();

                foreach (var TipoExtracaoId in extracao)
                {
                    var extracoes = pl.Where(_s => _s.TipoExtracaoId == TipoExtracaoId).OrderBy(_s => _s.ordem).ToList();

                    var PalavrasOrientacao = new Entidade().OrientacaoDocumento.Where(_s => _s.IdModelo == modeloAchado.ID && _s.x == null && _s.Tipo_Regex_Extracao.Count > 0).ToList();
                    var listaArqConteudo = conteudoposition.Split(' ');


                    foreach (var arquivo in PalavrasOrientacao)
                    {

                        if (arquivo.Tipo_Regex_Extracao.Where(_s => _s.TipoExtracaoId == TipoExtracaoId).Count() > 0)
                        {

                            int contador = 1;

                            foreach (var regx in extracoes)
                            {
                                int contadorPalavras = 0;

                                //foreach (var item in listaArqConteudo)
                                //{

                                //    ///arrumar a ordenação
                                //    if (regx.TipoExtracao.Regex != null)
                                //    {
                                //        if (contador == regx.ordem)
                                //        {
                                //            //   continue;
                                //        }
                                //        Regex regex = new Regex(regx.TipoExtracao.Regex);
                                //        Match matchReplace = regex.Match(item);

                                //        ////achou o campo
                                //        if (matchReplace.Success)
                                //        {
                                //            if (conteudoArq.Where(_s => _s.Key == regx.OrientacaoDocumento.Palavra).Count() == 0)
                                //            {
                                //                conteudoArq.Add(regx.OrientacaoDocumento.Palavra, matchReplace.Value);
                                //                contador++;
                                //                contadorPalavras++;
                                //                break;
                                //            }
                                //        }

                                //    }

                                //}
                                if (contadorPalavras == 0)
                                {
                                    List<string> lista = new List<string>();
                                    if (regx.TipoExtracao.Regex != null)
                                    {
                                        if (contador == regx.ordem)
                                        {
                                            //   continue;
                                        }
                                        Regex regex = new Regex(regx.TipoExtracao.Regex);
                                        Match matchReplace = regex.Match(conteudoposition);
                                        MatchCollection matchList = Regex.Matches(conteudoposition, regx.TipoExtracao.Regex);
                                        var listMatches = matchList.Cast<Match>().Select(match => match.Value).ToList();
                                        var list = new List<String>();
                                        foreach (var item in listMatches)
                                        {
                                            list.Add(item);
                                        }


                                        ////achou o campo
                                        string result = "";
                                        foreach (var item in list)
                                        {
                                            if (conteudoArq.Where(_s => _s.Key == regx.OrientacaoDocumento.Palavra).Count() == 0)
                                            {



                                                lista.Add(item);



                                            }
                                        }

                                        ///quando regex assumi lista
                                        var classe = "ConsoleApplication18.Evento._" + w.ID + "_" + w.PROJETO_ID + "_" + modeloAchado.ID;
                                        Type eventos = Type.GetType(classe);
                                        if (eventos != null)
                                        {
                                            var instancia = (IEvento)Activator.CreateInstance(eventos);

                                            if (instancia != null)
                                            {
                                                var dresult = (RetornoEventoDTO)instancia.C004(lista.ToArray(), EnumEntrada.LISTA);
                                                if (dresult.Retono != null)
                                                    result = dresult.Retono.ToString();
                                                else { result = ""; }
                                            }
                                        }
                                        else
                                        {
                                            if (lista.Count > 0)
                                                result = lista.FirstOrDefault();
                                        }
                                        if (conteudoArq.Where(_s => _s.Key == regx.OrientacaoDocumento.Palavra).Count() == 0)
                                        {

                                            conteudoArq.Add(regx.OrientacaoDocumento.Palavra, result);
                                        }
                                        if (matchReplace.Success)
                                        {

                                        }


                                    }
                                }

                            }
                        }
                    }
                }


            }



            var Palavras = new Entidade().OrientacaoDocumento.Where(_s => _s.IdModelo == modeloAchado.ID).ToList();


            var listaArq = conteudo.Split(' ');

            foreach (var arquivo in Palavras)
            {


                foreach (var item in listaArq)
                {


                    var extracao = arquivo.Tipo_Regex_Extracao.ToList().Select(_s => _s.TipoExtracaoId).Distinct().ToList();
                    foreach (var regx in arquivo.Tipo_Regex_Extracao.OrderBy(_s => _s.TipoExtracaoId).ThenBy(n => n.ordem))
                    {
                        ///arrumar a ordenação
                        if (regx.TipoExtracao.Regex != null)
                        {
                            Regex regex = new Regex(regx.TipoExtracao.Regex);
                            Match matchReplace = regex.Match(item);

                            ////achou o campo
                            if (matchReplace.Success)
                            {
                                if (conteudoArq.Where(_s => _s.Key == arquivo.Palavra).Count() == 0)
                                {
                                    if (arquivo.EspacoBranco != null && arquivo.EspacoBranco != "")
                                    {
                                        if (arquivo.EspacoBranco == "FIM")
                                        {
                                            conteudoArq.Add(arquivo.Palavra, item.TrimEnd());
                                            //  indiceArrumado = indice.Value.TrimEnd();
                                        }
                                        if (arquivo.EspacoBranco == "INICIO")
                                        {
                                            conteudoArq.Add(arquivo.Palavra, item.TrimStart());
                                        }
                                        if (arquivo.EspacoBranco == "TODOS")
                                        {
                                            conteudoArq.Add(arquivo.Palavra, item.Replace(" ", "").Trim());

                                        }
                                        if (arquivo.EspacoBranco == "NENHUM")
                                        {
                                            conteudoArq.Add(arquivo.Palavra, item);
                                        }
                                    }
                                    else
                                    {
                                        conteudoArq.Add(arquivo.Palavra, item);
                                    }
                                }
                            }
                        }
                    }


                }
            }



            //   List<String> indice = new List<string>();
            // var img = (Bitmap)System.Drawing.Image.FromFile(modeloMove); 
            //foreach (var cp in listCrop)
            //{

            //    var dcrops = CropImage(img, cp.x.Value, cp.y.Value, cp.width.Value, cp.height.Value);



            //    //dcrops.Save(@"C:\Users\Administrador\Desktop\Coopercarga\Canhotos\Crop\" + DateTime.Now.Millisecond + ".png");
            //    var imagens = ImageToByte((System.Drawing.Image)dcrops);
            //    var ls = capturarIndices(imagens, modeloAchado.ID);
            //    indiceLidos.Add(cp.nome, ls);
            //    //    dcrops.RotateFlip(RotateFlipType.Rotate180FlipNone);

            //    //if (ls.Count == 0)
            //    //{
            //    //    indice.Add("0_");
            //    //}
            //    //else
            //    //{
            //    //    int count = 0;
            //    //    foreach (var ind in ls)
            //    //    {
            //    //        count++;
            //    //        ind.Replace(':', ' ');
            //    //        if (ind == "_")
            //    //        {

            //    //                indice[indice.Count - 1] += ind;
            //    //        }
            //    //        else
            //    //        {

            //    //            indice.Add(ind);
            //    //        }
            //    //    }
            //    //}

            //    dcrops.Dispose();
            //}
            return conteudoArq;
            // StringBuilder nomeArquivo = new StringBuilder();
            // Documento doc = new Documento();
            // //foreach (var nomeArquivoIndice in indice.Distinct().OrderByDescending(_s => _s.Length))
            // foreach (var nomeArquivoIndice in indice.Distinct().ToList())
            // {
            //     nomeArquivo.Append(nomeArquivoIndice);
            // }
            // if (nomeArquivo.ToString() == "")
            // {
            //     nomeArquivo.Append(Guid.NewGuid());
            // }
            // nomeArquivo.Replace(":", "");
            // string move = Path.GetDirectoryName(modeloMove) + @"\" + nomeArquivo + modeloAchado.nome + "_" + Path.GetExtension(modeloMove);

            // img.Save(move);


            // doc.binario = File.ReadAllBytes(move);
            // doc.extensao = Path.GetExtension(modeloMove).Replace(".", "");
            // var nmDoc = Path.GetFileNameWithoutExtension(move);

            // var prop = nmDoc.Split('_');
            // doc.Nota = prop[1];
            // doc.localizacao = prop[0];
            // if (doc.localizacao == "")
            // {
            //     doc.localizacao = "_";
            // }
            // doc.Serie = prop[0];
            // doc.cnpj = "";// modeloAchado.CNPJ;
            // doc.cnpj = prop[2];
            // StringBuilder stbd = new StringBuilder();
            // foreach (var item in posicoes)
            // {
            //     stbd.Append(item.texto + " ");
            // }
            // doc.resultado = stbd.ToString();

            // UploadDocCloudFile(doc, modeloAchado);
            // img.Dispose();
            // while (true)
            // {
            //     if (!IsFileLocked(modeloMove))
            //     {
            //         break;
            //     }
            // }
            //// File.Delete(modeloMove);







        }


        public static Dictionary<string, string> indices(string modeloMove, Modelo modeloAchado, int projetoId, List<PalavrasCoordenadasDocumento> posicoes)
        {
            using (FileStream fs = File.Create(@"c:\logevento.txt"))
            {
                Byte[] info = new UTF8Encoding(true).GetBytes("indice disparado");
                // Add some information to the file.
                fs.Write(info, 0, info.Length);
            }
            Dictionary<string, string> indiceLidos = new Dictionary<string, string>();
            ///captura as posições do documento
            //var posicoes = ocr.executarOcrCoordenadas(modeloMove);
            var listCrop = GetCoordenadasdoModelo(modeloAchado.ID, posicoes);
            using (FileStream fs = File.Create(@"c:\logeventoCoordenadas.txt"))
            {
                Byte[] info = new UTF8Encoding(true).GetBytes(listCrop.Count.ToString());
                // Add some information to the file.
                fs.Write(info, 0, info.Length);
            }
            List<String> indice = new List<string>();


            System.Drawing.Image iamge = System.Drawing.Image.FromFile(modeloMove);
            Bitmap img = new Bitmap(new Bitmap(iamge));
            iamge.Dispose();



            foreach (var cp in listCrop)
            {
                if (cp.x == null && cp.y == null)
                {
                    continue;
                }
                ///Pen pen = new Pen(Color.Red, 2);

                //using (Graphics g = Graphics.FromImage(img))
                //{
                //    g.FillRectangle(Brushes.Red, (float)cp.x, (float)cp.y, (float)cp.width, (float)cp.height);
                //    g.Save();
                //}

                Graphics g = Graphics.FromImage(img);
                //  g.DrawRectangle(pen, (float)cp.x, (float)cp.y, (float)cp.width, (float)cp.height);
                var dcrops = CropImage(img, cp.x.Value, cp.y.Value, cp.width.Value, cp.height.Value);
                dcrops.Save("sasas" + Guid.NewGuid() + ".png");
                g.Dispose();









                var prjModelo = new Entidade().PROJETO_MODELO.Where(_s => _s.MODELO_ID == modeloAchado.ID && _s.PROJETO_ID == projetoId).FirstOrDefault();

                RetornoEventoDTO retorno = null;
                if (cp.Evento.HasValue && cp.Evento.Value)
                {

                    var DurtanteCapturarDocumento = new Entidade().Projeto_modelo_evento.Where(_s => _s.PROJETO_MODELO.ID == prjModelo.ID && _s.Eventos.codigo == "C004").ToList();
                    /////evento antes da indexacao
                    if (DurtanteCapturarDocumento.Count > 0)////significa que tem evento
                    {

                        using (FileStream fs = File.Create(@"c:\logeventodurante.txt"))
                        {
                            Byte[] info = new UTF8Encoding(true).GetBytes(DurtanteCapturarDocumento.ToString());
                            // Add some information to the file.
                            fs.Write(info, 0, info.Length);
                        }
                        if (cp.PARAMETROENTRADA != "")
                        {




                            Enum.TryParse(cp.PARAMETROENTRADA, out EnumEntrada entrada);

                            var classe = "ConsoleApplication18.Evento._" + prjModelo.ID + "_" + projetoId + "_" + modeloAchado.ID;
                            Type eventos = Type.GetType(classe);
                            var instancia = (IEvento)Activator.CreateInstance(eventos);
                            switch (entrada)
                            {
                                case EnumEntrada.TEXTO:
                                    break;
                                case EnumEntrada.NUMERO:
                                    break;
                                case EnumEntrada.DATA:
                                    var imagens = ImageToByte((System.Drawing.Image)dcrops);
                                    var ls = capturarIndices(imagens, modeloAchado.ID, projetoId);

                                    retorno = (RetornoEventoDTO)instancia.C004(ls, entrada);
                                    if (retorno != null && retorno.Retono != null)
                                    {
                                        if (retorno.RetornoTipo == EnumRetornoEvento.RETORNO_IMAGEM)
                                        {
                                            indiceLidos.Add(cp.nome, ls + "_parametro_" + ((string)retorno.Retono).ToString() + "_mensagem_" + retorno.MensagemRetorno);
                                        }
                                    }
                                    else
                                    {
                                        indiceLidos.Add(cp.nome, ls);
                                    }
                                    break;
                                case EnumEntrada.IMAGEM:
                                    if(projetoId == 1028)
                                    {
                                        var para = new Object[3];
                                        para[0] = (System.Drawing.Image)dcrops;
                                        para[1] = cp.nome;
                                        para[2]=  System.Drawing.Image.FromFile(modeloMove);
                                        retorno = (RetornoEventoDTO)instancia.C004(para, EnumEntrada.IMAGEM);
                                        if (retorno != null)
                                        {
                                            if (retorno.RetornoTipo == EnumRetornoEvento.RETORNO_STRING)
                                            {
                                                indiceLidos.Add(cp.nome, ((string)retorno.Retono).ToString());
                                            }
                                        }
                                    }
                                    else
                                    {
                                        retorno = (RetornoEventoDTO)instancia.C004((System.Drawing.Image)dcrops, EnumEntrada.IMAGEM);
                                        if (retorno != null)
                                        {
                                            if (retorno.RetornoTipo == EnumRetornoEvento.RETORNO_STRING)
                                            {
                                                indiceLidos.Add(cp.nome, ((string)retorno.Retono).ToString());
                                            }
                                        }
                                    }
                                  
                                    break;
                                default:
                                    break;
                            }



                        }
                    }
                }
                else
                {


                    var imagens = ImageToByte((System.Drawing.Image)dcrops);

                    var ls = capturarIndices(imagens, modeloAchado.ID, projetoId, cp);
                    indiceLidos.Add(cp.nome, ls);

                }
                dcrops.Dispose();

            }
            string DocumentoView = ConfigurationManager.AppSettings["DocumentoView"];

            var Imageview = (System.Drawing.Image)img.Clone();
            Imageview.Save(DocumentoView + Path.GetFileName(modeloMove));
            Imageview.Dispose();

            img.Dispose();
            //  File.Delete(modeloMove);

            // ((System.Drawing.Image)d).Save(modeloMove);

            return indiceLidos;

        }


        public static void criarDocumentoIndice(Dictionary<string, string> arquivos, string caminho, string nome)
        {
            string arquivo = caminho + nome + ".inx";
            if (!File.Exists(arquivo))
            {
                File.Create(arquivo);
            }

            using (var tw = new StreamWriter(arquivo, true))
            {
                foreach (var valores in arquivos)
                {
                    tw.WriteLine(valores.Key + " : " + valores.Value);


                }
                tw.Close();
            }
        }

        public class Documento
        {
            public string Serie { get; set; }
            public string Nota { get; set; }
            public byte[] binario { get; set; }
            public string extensao { get; set; }
            public string cnpj { get; set; }
            public string localizacao { get; set; }
            public string resultado { get; set; }
        }

        //public static void InserirDocumentoLog(ConsoleApplication18.Documento doc)
        //{
        //    var context = new Entidade();
        //    context.Documento.Add(doc);
        //    context.SaveChanges();

        //}

        //public static void UploadDocCloudFile(Documento doc, Modelo modelo)
        //{

        //    wsIntegracao.WSIntegracao ws = new wsIntegracao.WSIntegracao();
        //    string XML = ws.ConsultarFolders(0, "327", "3310", "2378", Guid.NewGuid().ToString(), doc.localizacao, doc.Nota, "", "", "", "", "");
        //    XmlDocument docXml = new XmlDocument();
        //    docXml.LoadXml(XML);

        //    String protocolo = "";

        //    XmlNodeList elemList = docXml.GetElementsByTagName("Protocolo");
        //    ////achou o protocolo
        //    if (elemList.Count > 0)
        //    {
        //        foreach (XmlNode childNode in elemList)
        //        {
        //            var sverificacao = childNode;//InnerText ;
        //            if (sverificacao.Name == "Protocolo")
        //            {
        //                protocolo = sverificacao.InnerText;
        //                ws.GravarFolderCloudDocs(Convert.ToDecimal(protocolo), 327, 2583, 3310, 2583, doc.Nota, doc.Serie, modelo.nome, doc.localizacao, "", "Canhoto Digitalizado", "", doc.binario, doc.extensao, 16349, "", 0);
        //                break;
        //            }
        //        }






        //    }
        //    else
        //    {

        //        ws.GravarFolderCloudDocs(0, 327, 2583, 3325, 2430, doc.Serie, doc.Nota, doc.cnpj, "", "", "", "", doc.binario, doc.extensao, 16349, "", 0);
        //    }

        //    ///inclusão 
        //    ///





        //}

        public static void UploadDocCloudFile(Documento doc, Modelo modelo)
        {

            wsIntegracao.WSIntegracao ws = new wsIntegracao.WSIntegracao();
            string XML = ws.ConsultarFolders(0, "0327", "003290", "02339", doc.Nota, doc.Serie, "", "", "", "", "", "");
            XmlDocument docXml = new XmlDocument();
            docXml.LoadXml(XML);

            String protocolo = "";

            XmlNodeList elemList = docXml.GetElementsByTagName("Protocolo");
            ////achou o protocolo
            if (elemList.Count > 0)
            {
                foreach (XmlNode childNode in elemList)
                {
                    var sverificacao = childNode;//InnerText ;
                    if (sverificacao.Name == "Protocolo")
                    {
                        protocolo = sverificacao.InnerText;
                        ws.GravarFolderCloudDocs(Convert.ToDecimal(protocolo), 327, 2583, 003290, 02339, doc.Nota, doc.Serie, modelo.nome, doc.localizacao, "", "Canhoto Digitalizado", "", doc.binario, doc.extensao, 16349, "", 0);
                        break;
                    }
                }






            }
            //else
            //{

            //    ws.GravarFolderCloudDocs(0, 327, 2583, 3325, 2430, doc.Serie, doc.Nota, doc.cnpj, "", "", "", "", doc.binario, doc.extensao, 16349, "", 0);
            //}

            ///inclusão 
            ///





        }
        public static void UploadFtpFile(string fileName)
        {
            string caminhoTiff = ConfigurationManager.AppSettings["caminhoTiff"];
            string saidaPdf = ConfigurationManager.AppSettings["saidaPdf"];
            string saidaErro = ConfigurationManager.AppSettings["saidaErro"];
            string saidaBack = ConfigurationManager.AppSettings["saidaBack"];
            string ftpUrl = ConfigurationManager.AppSettings["ftpUrl"];
            string usuarioFtp = ConfigurationManager.AppSettings["usuarioFtp"];
            string senhaFtp = ConfigurationManager.AppSettings["senhaFtp"];
            using (System.Net.WebClient client = new System.Net.WebClient())
            {
                client.Credentials = new System.Net.NetworkCredential(usuarioFtp, senhaFtp);
                client.UploadFile("ftp://54.233.110.24" + "/Tecnoset/Canhotos/Entrada/" + Path.GetFileName(fileName), "STOR", fileName);
            }
        }

    }
}
