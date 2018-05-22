
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract;

namespace ConsoleApplication18.OcrImpletentacao
{
    public class TesseractOcr : IOcr
    {
        public StringBuilder executarOcr(byte[] arquivo)
        {
            string path = new DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.FullName;
            StringBuilder sb = new StringBuilder();

            var dataPath = path + @"\Content\tessdata";

            try
            {
                using (var tEngine = new Tesseract.TesseractEngine(dataPath, "eng", EngineMode.Default)) //creating the tesseract OCR engine with English as the language
                {
                    Bitmap bmp;
                    using (var ms = new MemoryStream(arquivo))
                    {
                        bmp = new Bitmap(ms);
                        using (var page = tEngine.Process(bmp)) //process the specified image
                        {
                            var text = page.GetText(); //Gets the image's content as plain text.
                            sb.Append(text);
                        }
                        bmp.Dispose();
                    }
                    

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected Error: " + e.Message);
            }
            return sb;
        }

        public StringBuilder executarOcr(string caminho)
        {
            string path = new DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.FullName;
            StringBuilder sb = new StringBuilder();

            var dataPath = path + @"\Content\tessdata";

            try
            {
                using (var tEngine = new Tesseract.TesseractEngine(dataPath, "eng", EngineMode.Default)) //creating the tesseract OCR engine with English as the language
                {
                    using (var img = Pix.LoadFromFile(caminho)) // Load of the image file from the Pix object which is a wrapper for Leptonica PIX structure
                    {
                        using (var page = tEngine.Process(img)) //process the specified image
                        {
                            var text = page.GetText(); //Gets the image's content as plain text.
                            sb.Append(text);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected Error: " + e.Message);
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

        public static List<PalavrasCoordenadasDocumento> DoOecr(Tesseract.TesseractEngine ensgine, int frameIndex, Bitmap bitmap, string resultStream)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sbs = new StringBuilder();
            List<PalavrasCoordenadasDocumento> palavrasOrdenadas = new List<PalavrasCoordenadasDocumento>();
            using (var engine = ensgine)
            {
                //   engine.SetVariable("save_blob_choices", true);
                engine.SetVariable("tessedit_char_whitelist", "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ");
                using (var img = Pix.LoadFromFile(resultStream))
                {
                    using (var page = engine.Process(img))
                    {
                        int symbolCount = 0;
                        using (var iter = page.GetIterator())
                        {
                            iter.Begin();
                            do
                            {
                                if (iter.IsAtBeginningOf(PageIteratorLevel.Block))
                                {

                                    Console.WriteLine("START BLOCK");
                                }

                                do
                                {
                                    do
                                    {
                                        do
                                        {
                                            if (iter.IsAtBeginningOf(PageIteratorLevel.Word))
                                            {
                                                Rect symbolBounds;
                                                sbs.Append(iter.GetText(PageIteratorLevel.Word) + " ");
                                                var d = iter.TryGetBoundingBox(PageIteratorLevel.Word, out symbolBounds);



                                                PalavrasCoordenadasDocumento pl = new PalavrasCoordenadasDocumento();
                                                StringBuilder buluder = new StringBuilder();



                                                buluder.Append(iter.GetText(PageIteratorLevel.Word));




                                                pl.y = Convert.ToInt32(symbolBounds.Y1);


                                                pl.x = Convert.ToInt32(symbolBounds.X1);

                                                int width = Convert.ToInt32(symbolBounds.X2) - Convert.ToInt32(symbolBounds.X1);



                                                int height = Convert.ToInt32(symbolBounds.X2) - Convert.ToInt32(symbolBounds.X1);

                                                pl.width = width;
                                                pl.height = height;

                                                //if (pl.width > 0)
                                                //{
                                                //    var bp = CropImage(System.Drawing.Image.FromFile(resultStream), pl.x, pl.y, pl.width, pl.height);

                                                //    bp.Dispose();
                                                //}

                                                pl.texto = buluder.ToString();






                                                palavrasOrdenadas.Add(pl);


                                            }

                                            do
                                            {

                                                sb.Append(iter.GetText(PageIteratorLevel.Symbol) + " " +
                                                    iter.GetText(PageIteratorLevel.Symbol));


                                                Console.Write(iter.GetText(PageIteratorLevel.Word));
                                                if (iter.IsAtFinalOf(PageIteratorLevel.Word, PageIteratorLevel.Symbol))
                                                {
                                                    Console.Write("END WORD");

                                                }
                                                symbolCount++;
                                            } while (iter.Next(PageIteratorLevel.Word, PageIteratorLevel.Symbol));
                                        } while (iter.Next(PageIteratorLevel.TextLine, PageIteratorLevel.Word));
                                    } while (iter.Next(PageIteratorLevel.Para, PageIteratorLevel.TextLine));


                                    if (iter.IsAtFinalOf(PageIteratorLevel.Block, PageIteratorLevel.Para))
                                    {
                                        Console.WriteLine("\nEND BLOCK");
                                    }
                                } while (iter.Next(PageIteratorLevel.Block, PageIteratorLevel.Para));
                            } while (iter.Next(PageIteratorLevel.Block));
                        }

                        Console.WriteLine("Confidence: {0}", page.GetMeanConfidence());
                        Console.WriteLine("Symbol Count: {0}", symbolCount);
                    }
                }
            }
            return palavrasOrdenadas;
        }

        public List<PalavrasCoordenadasDocumento> executarOcrCoordenadas(string caminho)
        {
            string path = new DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.FullName;
            StringBuilder bulsuder = new StringBuilder();
            var dataPath = path + @"\Content\tessdata";
            List<PalavrasCoordenadasDocumento> palavrasOrdenadas = new List<PalavrasCoordenadasDocumento>();
            palavrasOrdenadas = DoOecr(new TesseractEngine(dataPath, "eng", EngineMode.Default), 0, null, caminho);
            try
            {

                List<PalavrasOcr> palavras = new List<PalavrasOcr>();
                using (var ocr = new TesseractEngine(dataPath, "eng", EngineMode.TesseractOnly))
                {
                    ocr.SetVariable("tessedit_char_whitelist", "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ");

                    // image could be a Bitmap however it's probably better if its a pix
                    using (var page = ocr.Process(Pix.LoadFromFile(caminho)))
                    {
                        using (var iter = page.GetIterator())
                        {
                            iter.Begin();
                            do
                            {
                                var h = iter.GetText(PageIteratorLevel.Para);
                                // for each line in the paragraph
                                do
                                {
                                    // for each word in the line


                                    // Do whatever you want with the word, and\or iterate through it's characters
                                    do
                                    {
                                        // for each symbol(character) in the word
                                        var word = iter.GetText(PageIteratorLevel.Word);
                                        // Do whatever you want with the symbol

                                    } while (iter.Next(PageIteratorLevel.TextLine, PageIteratorLevel.Word));
                                } while (iter.Next(PageIteratorLevel.Para, PageIteratorLevel.TextLine));
                            } while (iter.Next(PageIteratorLevel.Block, PageIteratorLevel.Para));
                        }
                    }
                }
                using (var tEngine = new Tesseract.TesseractEngine(dataPath, "eng", EngineMode.Default)) //creating the tesseract OCR engine with English as the language
                {
                    using (var img = Pix.LoadFromFile(caminho)) // Load of the image file from the Pix object which is a wrapper for Leptonica PIX structure
                    {
                        using (var page = tEngine.Process(img)) //process the specified image
                        {

                            var d = page.GetText();
                            using (var iter = page.GetIterator())
                            {
                                iter.Begin();
                                do
                                {
                                    do
                                    {
                                        do
                                        {
                                            if (iter.IsAtBeginningOf(PageIteratorLevel.Block))
                                            {
                                                bulsuder.Append(iter.GetText(PageIteratorLevel.Block));
                                            }
                                            if (iter.IsAtBeginningOf(PageIteratorLevel.Para))
                                            {
                                                bulsuder.Append(iter.GetText(PageIteratorLevel.Para));
                                            }
                                            if (iter.IsAtBeginningOf(PageIteratorLevel.TextLine))
                                            {
                                                bulsuder.Append(iter.GetText(PageIteratorLevel.TextLine));
                                            }
                                            if (iter.IsAtBeginningOf(PageIteratorLevel.Word))
                                            {
                                                bulsuder.Append(iter.GetText(PageIteratorLevel.Word));
                                            }

                                            // get bounding box for symbol
                                            Rect symbolBounds;
                                            if (iter.TryGetBoundingBox(PageIteratorLevel.Symbol, out symbolBounds))
                                            {
                                                bulsuder.Append(iter.GetText(PageIteratorLevel.Symbol));
                                            }
                                        } while (iter.Next(PageIteratorLevel.Word, PageIteratorLevel.Block));
                                    } while (iter.Next(PageIteratorLevel.TextLine, PageIteratorLevel.Word));
                                } while (iter.Next(PageIteratorLevel.Para, PageIteratorLevel.TextLine));
                            }

                            //using (var iter = page.GetIterator())
                            //{
                            //    do
                            //    {
                            //        StringBuilder bulsuder = new StringBuilder();
                            //        Rect s;
                            //        var text = page.GetText();
                            //        bulsuder.Append(iter.GetText(PageIteratorLevel.Para));

                            //        bulsuder.Append(iter.GetText(PageIteratorLevel.Block));

                            //        bulsuder.Append(iter.GetText(PageIteratorLevel.TextLine));
                            //        do
                            //        {
                            //            do
                            //            {


                            //                Rect symbolBounds;
                            //                if (iter.TryGetBoundingBox(PageIteratorLevel.Word, out symbolBounds))
                            //                {
                            //                    PalavrasCoordenadasDocumento pl = new PalavrasCoordenadasDocumento();
                            //                    StringBuilder buluder = new StringBuilder();



                            //                    buluder.Append(iter.GetText(PageIteratorLevel.Word));




                            //                    pl.y = Convert.ToInt32(symbolBounds.Y1);


                            //                    pl.x = Convert.ToInt32(symbolBounds.X1);

                            //                    int width = Convert.ToInt32(symbolBounds.X2) - Convert.ToInt32(symbolBounds.X1);



                            //                    int height = Convert.ToInt32(symbolBounds.X2) - Convert.ToInt32(symbolBounds.X1);

                            //                    pl.width = width;
                            //                    pl.height = height;

                            //                    if (pl.width > 0)
                            //                    {
                            //                        var bp = CropImage(System.Drawing.Image.FromFile(caminho), pl.x, pl.y, pl.width, pl.height);

                            //                        bp.Dispose();
                            //                    }

                            //                    pl.texto = buluder.ToString();






                            //                    palavrasOrdenadas.Add(pl);
                            //                }
                            //            } while (iter.Next(PageIteratorLevel.Word, PageIteratorLevel.Block));
                            //        } while (iter.Next(PageIteratorLevel.TextLine, PageIteratorLevel.Word));
                            //    } while (iter.Next(PageIteratorLevel.Para, PageIteratorLevel.TextLine));
                            //}
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected Error: " + e.Message);
            }

            return palavrasOrdenadas;
        }
        public string tipo()
        {
            return "TesseractOcr";
        }
    }
}
