using Google.Cloud.Vision.V1;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication18
{
    public class GoogleOcr : IOcr
    {
 
        public StringBuilder executarOcr(byte[] arquivo)
        {
            StringBuilder sb = new StringBuilder();
            var client = ImageAnnotatorClient.Create();
            //Thread.Sleep(20000);
            var response = client.DetectText(Google.Cloud.Vision.V1.Image.FromBytes(arquivo));
            foreach (var annotation in response)
            {
                if (annotation.Description != null)
                {
                    sb.Append(annotation.Description + " ");

                }
            }

            return sb;
        }

        public StringBuilder executarOcr(string caminho)
        {
           
          
           StringBuilder sb = new StringBuilder();
            var client = ImageAnnotatorClient.Create();
            //Thread.Sleep(20000);
            var response = client.DetectText(Google.Cloud.Vision.V1.Image.FromFile(caminho));
            foreach (var annotation in response)
            {
                if (annotation.Description != null)
                {
                    sb.Append(annotation.Description + " ");

                }
            }
         
             

            return sb;
        }

         
        
        public List<PalavrasCoordenadasDocumento> executarOcrCoordenadas(byte[] arquivo)
        {
            throw new NotImplementedException();
        }

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

            return bmp;
        }
        public List<PalavrasCoordenadasDocumento> executarOcrCoordenadas(string caminho)
        {
            Google.Cloud.Vision.V1.Image image = Google.Cloud.Vision.V1.Image.FromFile(caminho);
            //    Thread.Sleep(20000);
            
            List<PalavrasOcr> palavras = new List<PalavrasOcr>();
            var client = ImageAnnotatorClient.Create();
            var response = client.DetectDocumentText(image);
            List<PalavrasCoordenadasDocumento> palavrasOrdenadas = new List<PalavrasCoordenadasDocumento>();
            foreach (var page in response.Pages)
            {
                var d = page.ToString();
                foreach (var block in page.Blocks)
                {

                    foreach (var paragraph in block.Paragraphs)
                    {
                        Console.WriteLine(string.Join("\n", paragraph.Property));

                        foreach (var item in paragraph.Words)
                        {
                            PalavrasCoordenadasDocumento pl = new PalavrasCoordenadasDocumento();
                            string myJsonString = item.Symbols.ToString();
                            PalavrasOcr[] myProduct = JsonConvert.DeserializeObject<PalavrasOcr[]>(myJsonString);
                            StringBuilder buluder = new StringBuilder();
                            for (int i = 0; i < myProduct.Count(); i++)
                            {

                                buluder.Append(myProduct[i].text);
                                if (i == myProduct.Count() - 1)
                                {
                                    var primeira = myProduct[0];
                                    var segunda = myProduct[i];

                                    string y = primeira.boundingBox.vertices.Min(_s => _s.y).ToString();
                                    pl.y = Convert.ToInt32(y);

                                    string x = primeira.boundingBox.vertices.Min(_s => _s.x).ToString();
                                    pl.x = Convert.ToInt32(x);
                                    var join = segunda.boundingBox.vertices.Concat(primeira.boundingBox.vertices).ToArray();
                                    int width = Convert.ToInt32(join.Max(_s => _s.x)) - Convert.ToInt32(join.Min(_s => _s.x));



                                    int height = Convert.ToInt32(join.Max(_s => _s.y)) - Convert.ToInt32(join.Min(_s => _s.y));

                                    pl.width = width;
                                    pl.height = height;

                                    //if (pl.width > 0)
                                    //{
                                    //    var bp = CropImage(System.Drawing.Image.FromFile(caminho), pl.x, pl.y, pl.width, pl.height);
                                        
                                    //    bp.Dispose();
                                    //}

                                    pl.texto = buluder.ToString();
                                }



                            }

                            palavrasOrdenadas.Add(pl);
                        }

                    }

                }
            }

            return palavrasOrdenadas;
        }

        public string tipo()
        {
            return "GoogleOcr";
        }
    }
}
