using GdPicture;
using Nuance.OmniPage.CSDK.ArgTypes;
using Nuance.OmniPage.CSDK.Objects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication18.OcrImpletentacao
{
    public class OmniPageOcr : IOcr
    {

        public StringBuilder executarOcr(byte[] arquivo)
        {
            StringBuilder retorno = new StringBuilder();
            try
            {



                Engine.Init(null, null);    // use your company and product name here
                if (Engine.RECERR == RECERR.API_INIT_WARN)
                {

                }
                using (SettingCollection settings = new SettingCollection())
                {
                    Bitmap bmp;
                    using (var ms = new MemoryStream(arquivo))
                    {
                        bmp = new Bitmap(ms);
                    }

                    using (Page page = new Page(bmp, new IMG_INFO(), settings))
                    {
                        settings.PageDescription = PAGEDESCRIPTION.LZ_TABLE_AUTO;

                        page.LocateZones();

                        page.OCRZones.CopyToUserZones();

                        //foreach (UserZone userzone in page.UserZones)
                        //{
                        //    ZONE zone = userzone.GetZoneInfo();
                        //    //if (zone.type == ZONETYPE.WT_TABLE)
                        //    //{
                        //    ((InputZone)userzone).LocateTable(false);
                        //    //   }
                        //}

                        page.Recognize();

                        page.OCRZones.CopyToUserZones();

                        foreach (UserZone userzone in page.UserZones)
                        {
                            ZONE zone = userzone.GetZoneInfo();
                            //   InputZone inputzone = userzone as InputZone;
                            //if (zone.type == ZONETYPE.WT_AUTO)
                            //{



                            // CELL_INFO[] cells = inputzone.GetTableCells();
                            StringBuilder sb = new StringBuilder();
                            bool inicio = true;
                            for (int i = 0; i < 1; i++)
                            {
                                StringBuilder sBuffer = new StringBuilder();


                                List<positionDs> lisds = new List<positionDs>();

                                LETTER[] letters = page[IMAGEINDEX.II_CURRENT].GetLetters();
                                for (int j = 0; j < letters.Length; j++)
                                {


                                    if (letters[j].code == ' ')
                                    {
                                        try
                                        {
                                            var we = letters[j];


                                            inicio = true;
                                            string x = "";
                                            string y = "";
                                            lisds.Add(new positionDs() { Conteudo = letters[j].code.ToString(), Height = letters[j].height, Width = letters[j].width, Y = letters[j].top, X = letters[j].left });
                                            var wid = lisds[lisds.Count - 1].X - lisds[0].X;
                                            var height = lisds.Max(_s => _s.Height);
                                            if (lisds[0] != null)
                                            {
                                                //var ds = CropImage(Bitmap.FromFile(strInput), lisds[0].X, lisds[0].Y, wid, height);
                                                //ds.Save(@"C:\Users\Administrador\Desktop\crop\ddddddddddddddddddddd" + Guid.NewGuid() + ".png");
                                                //ds.Dispose();
                                                retorno.Append(sb.ToString() + " ");

                                            }
                                            sb = new StringBuilder();
                                            lisds = new List<positionDs>();
                                        }
                                        catch (Exception ex)
                                        {

                                        }
                                    }
                                    else
                                    {
                                        sBuffer.Append(letters[j].code);
                                        lisds.Add(new positionDs() { Conteudo = letters[j].code.ToString(), Height = letters[j].height, Width = letters[j].width, Y = letters[j].top, X = letters[j].left });
                                        //var d = CropImage(Bitmap.FromFile(strInput), letters[j].left, letters[j].top, letters[j].width, letters[j].height);
                                        //d.Save(@"C:\Users\Administrador\Desktop\crop\"+Guid.NewGuid() + ".png");
                                        sb.Append(letters[j].code);
                                        inicio = false;
                                    }
                                }

                            }
                        }
                        // }
                    }
                    bmp.Dispose();
                }
            }
            catch (Exception e)
            {

            }
            finally
            {

                Engine.ForceQuit();

            }
            return retorno;
        }
        private readonly Color colorBlack =
         Color.FromArgb(255, 0, 0, 0);
        private readonly Color colorWhite =
          Color.FromArgb(255, 255, 255, 255);
        public StringBuilder executarIcry(string caminho)
        {

            //var imgSrc = (System.Drawing.Bitmap)Bitmap.FromFile(caminho);
            //int width = imgSrc.Width;
            //int height = imgSrc.Height;
            //Color pixel;
            //Bitmap imgOut = new Bitmap(imgSrc);
            //for (int row = 0; row < height - 1; row++)
            //{
            //    for (int col = 0; col < width - 1; col++)
            //    {
            //        pixel = imgSrc.GetPixel(col, row);

            //        imgOut.SetPixel(0, 31, this.colorWhite);
            //        imgOut.SetPixel(31, 0, this.colorWhite);

            //        if (row == 0 || row == 31)
            //        {
            //            imgOut.SetPixel(col, row, this.colorWhite);
            //        }
            //        if (col == 0 || col == 31)
            //        {
            //            imgOut.SetPixel(col, row, this.colorWhite);
            //        }

            //        //if (pixel.GetBrightness() == 0)
            //        //{

            //        //    imgOut.SetPixel(col, row, this.colorBlack);
            //        //}
            //        //else
            //        //{
            //        //    imgOut.SetPixel(col, row, this.colorWhite);
            //        //}
            //    }
            //}



            //String ssa = @"C:\ConsoleApplication18\ConsoleApplication18\bin\Debug\CropScript\a";

            //imgOut.Save(ssa + @"\" + Path.GetFileName(caminho));
            //imgSrc.Dispose();
            //imgOut.Dispose();

            //GdPictureImaging oGdPictureImaging = new GdPictureImaging();
            //oGdPictureImaging.SetLicenseNumber("4118106456693265856441854");
            //var id = oGdPictureImaging.CreateGdPictureImageFromFile(ssa + @"\" + Path.GetFileName(caminho));
            ////oGdPictureImaging.FxDespeckle(id);
            //oGdPictureImaging.ConvertTo1Bpp(id);
            //oGdPictureImaging.SaveAsPNG(id, ssa + @"\" + Path.GetFileName(caminho));
            //oGdPictureImaging.ReleaseGdPictureImage(id);


            //try
            //{
            //    caminho = ssa + @"\" + Path.GetFileName(caminho);
            //    Engine.Init(null, null);    // use your company and product name here
            //    if (Engine.RECERR == RECERR.API_INIT_WARN)
            //    {

            //    }
            //    using (SettingCollection settings = new SettingCollection())
            //    {
            //        using (FormTemplateLibrary library = new FormTemplateLibrary("FormTemplate.ftl", settings, true))
            //        {
            //            string[] files = { caminho };
            //            process images:
            //            foreach (string file in files)
            //            {
            //                using (Page page = new Page(file, 0, settings))
            //                {
            //                    page.Preprocess();

            //                    int Confidence;
            //                    int nMatching;
            //                    FormTemplateCollection collection = null;
            //                    MatchingFormTemplate BestMatchingID = page.FindFormTemplate(library, out collection, out Confidence, out nMatching);
            //                    using (collection)
            //                    {
            //                        if (nMatching > 0)
            //                        {
            //                            page.ApplyFormTemplate(BestMatchingID);
            //                            page.Recognize();

            //                            write text:
            //                            foreach (OCRZone zone in page.OCRZones)
            //                            {
            //                                string Zonename = zone.Name;
            //                                string Text = zone.Text;

            //                            }
            //                        }
            //                        else
            //                        {
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
            //catch (Exception e)
            //{
            //}
            //finally
            //{
            //    Engine.ForceQuit();
            //}


            var str = new StringBuilder();

            try
            {
                Engine.Init(null, null);    // use your company and product name here
                if (Engine.ModulesInfo[(int)KRECMODULES.INFO_HNR].Version <= 0)
                {
                }
                using (SettingCollection settings = new SettingCollection())
                {
                    using (Page page = new Page(caminho, RecAPIConstants.IMGF_FIRSTPAGE, settings))
                    {
                        settings.CodePages.Current = "Windows ANSI";

                        IMG_INFO ii = page[IMAGEINDEX.II_CURRENT].ImageInfo;

                        ZONE zone = new ZONE();
                        zone.rectBBox.left = 0;
                        zone.rectBBox.top = 0;
                        zone.rectBBox.right = ii.Size.cx;
                        zone.rectBBox.bottom = ii.Size.cy;
                        zone.fm = FILLINGMETHOD.FM_HANDPRINT;
                        zone.rm = RECOGNITIONMODULE.RM_HNR;
                        zone.filter = CHR_FILTER.FILTER_DIGIT;
                        zone.type = ZONETYPE.WT_FLOW;
                        zone.chk_sect = "";
                        zone.chk_fn = null;
                        zone.chk_control = 0;
                        zone.userdata = 0;

                        page.UserZones.InsertInputZone(zone, 0);


                        page.Recognize();


                        settings.DTXTOutputformat = DTXTOUTPUTFORMATS.DTXT_TXTS;
                        //page.Convert2DTXT(caminho + @"\" + Path.GetFileName(caminho) + ".txt");
                        page.Recognize();
                         LETTER[] letters = page[IMAGEINDEX.II_CURRENT].GetLetters();


                        foreach (var item in letters)
                        {
                            str.Append(item.code.ToString());
                        }

                    }
                }
            }
            catch (Exception e)
            {

            }
            finally
            {

                Engine.ForceQuit();

            }
            //GdPictureImaging oGdPictureImaging = new GdPictureImaging();
            //oGdPictureImaging.SetLicenseNumber("4118106456693265856441854");
            //var id = oGdPictureImaging.CreateGdPictureImageFromFile(caminho);
            //oGdPictureImaging.SaveAsTIFF(id, caminho+".tif", TiffCompression.TiffCompressionLZW);
            //string templateLib = @"C:\ConsoleApplication18\ConsoleApplication18\Content\Template.tlz";
            //string inputFile = caminho + ".tif";
            //string outputXML = Path.ChangeExtension(inputFile, ".out.xml");
            //int conf = 0;
            //int matching = 0;

            //if (File.Exists(outputXML))
            //    File.Delete(outputXML);

            ////Initialize Engine
            //Engine.Init(null, null, true, @"C:\Program Files (x86)\Nuance\OPCaptureSDK20\Bin");
            ////Create a settings collection
            ////Load the template library.
            ////Load the input file
            //using (SettingCollection sett = new SettingCollection())
            //using (FormTemplateLibrary formLibrary = new FormTemplateLibrary(templateLib, sett))
            //using (ImageFile inputImageFile = new ImageFile(inputFile, FILEOPENMODE.IMGF_READ, IMF_FORMAT.FF_SIZE, sett))
            //{
            //    //Modify any recognition settings                
            //    sett.DTXTOutputformat = DTXTOUTPUTFORMATS.DTXT_XMLCOORD;

            //    sett.DefaultRecognitionModule = RECOGNITIONMODULE.RM_OMNIFONT_PLUS3W;

            //    //Iterate through the pages
            //    for (int i = 0; i < inputImageFile.PageCount; i++)
            //    {
            //        //Load a page from the input file
            //        using (Page page = new Page(inputImageFile, i, sett))
            //        {
            //            FormTemplateCollection templateCollection = null;
            //            //Pre-process the document (rotate, deskew, etc.)

            //            page.Preprocess();

            //            //Find any templates that match thie input page
            //            MatchingFormTemplate matchingTemplate = page.FindFormTemplate(formLibrary, out templateCollection, out conf, out matching);
            //            //Get detailed information about the match, useful for debugging.
            //            string findInfo = templateCollection.FindInfo;
            //            if (matching != 0)
            //            {
            //                //Apply the matching template to the page.
            //                page.ApplyFormTemplate(matchingTemplate);

            //                foreach (UserZone z in page.UserZones)
            //                {
            //                    ZONE zoneInfo = z.GetZoneInfo(IMAGEINDEX.II_CURRENT);
            //                    if (zoneInfo.fm == FILLINGMETHOD.FM_HANDPRINT)
            //                    {
            //                        //Change the recognition algo to only recognize handprinted digits
            //                        zoneInfo.rm = RECOGNITIONMODULE.RM_RER;
            //                        zoneInfo.filter = CHR_FILTER.FILTER_DIGIT;

            //                        //Update the zone in the zone list.
            //                        InputZone iz = z as InputZone;
            //                        iz.Update(zoneInfo);
            //                    }
            //                }

            //                //Recognize (OCR) the page to XML.
            //                page.Recognize(outputXML);

            //                //Access results
            //                foreach (OCRZone zone in page.OCRZones)
            //                {
            //                    Console.WriteLine("{0}, {1}", zone.Name, zone.Text);
            //                }
            //            }
            //            //Free the template collection
            //            if (templateCollection != null)
            //                templateCollection.Dispose();
            //        }
            //    }
            //}
            return str;
        }


        public StringBuilder executarOcr(string caminho)
        {

            StringBuilder retorno = new StringBuilder();
            try
            {


                Engine.Init(null, null);    // use your company and product name here
                if (Engine.RECERR == RECERR.API_INIT_WARN)
                {

                }
                using (SettingCollection settings = new SettingCollection())
                {

                    using (Page page = new Page(caminho, 0, settings))
                    {
                        settings.PageDescription = PAGEDESCRIPTION.LZ_TABLE_AUTO;

                        page.LocateZones();

                        page.OCRZones.CopyToUserZones();

                        //foreach (UserZone userzone in page.UserZones)
                        //{
                        //    ZONE zone = userzone.GetZoneInfo();
                        //    //if (zone.type == ZONETYPE.WT_TABLE)
                        //    //{
                        //    ((InputZone)userzone).LocateTable(false);
                        //    //   }
                        //}

                        page.Recognize();

                        page.OCRZones.CopyToUserZones();


                        //   InputZone inputzone = userzone as InputZone;
                        //if (zone.type == ZONETYPE.WT_AUTO)
                        //{



                        // CELL_INFO[] cells = inputzone.GetTableCells();
                        StringBuilder sb = new StringBuilder();
                        bool inicio = true;
                        for (int i = 0; i < 1; i++)
                        {
                            StringBuilder sBuffer = new StringBuilder();


                            List<positionDs> lisds = new List<positionDs>();

                            LETTER[] letters = page[IMAGEINDEX.II_CURRENT].GetLetters();
                            for (int j = 0; j < letters.Length; j++)
                            {


                                if (letters[j].code == ' ')
                                {
                                    try
                                    {
                                        var we = letters[j];


                                        inicio = true;
                                        string x = "";
                                        string y = "";
                                        lisds.Add(new positionDs() { Conteudo = letters[j].code.ToString(), Height = letters[j].height, Width = letters[j].width, Y = letters[j].top, X = letters[j].left });
                                        var wid = lisds[lisds.Count - 1].X - lisds[0].X;
                                        var height = lisds.Max(_s => _s.Height);
                                        if (lisds[0] != null)
                                        {
                                            //var ds = CropImage(Bitmap.FromFile(strInput), lisds[0].X, lisds[0].Y, wid, height);
                                            //ds.Save(@"C:\Users\Administrador\Desktop\crop\ddddddddddddddddddddd" + Guid.NewGuid() + ".png");
                                            //ds.Dispose();
                                            retorno.Append(sb.ToString() + " ");

                                        }
                                        sb = new StringBuilder();
                                        lisds = new List<positionDs>();
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                }
                                else
                                {
                                    sBuffer.Append(letters[j].code);
                                    lisds.Add(new positionDs() { Conteudo = letters[j].code.ToString(), Height = letters[j].height, Width = letters[j].width, Y = letters[j].top, X = letters[j].left });
                                    //var d = CropImage(Bitmap.FromFile(strInput), letters[j].left, letters[j].top, letters[j].width, letters[j].height);
                                    //d.Save(@"C:\Users\Administrador\Desktop\crop\"+Guid.NewGuid() + ".png");
                                    sb.Append(letters[j].code);
                                    inicio = false;
                                }
                            }

                        }

                        // }
                    }
                }
            }
            catch (Exception e)
            {

            }
            finally
            {

                Engine.ForceQuit();

            }
            return retorno;
        }
        class positionDs
        {
            private string _conteudo;
            private int _x;
            private int _y;
            private int _width;
            private int _height;

            public string Conteudo { get => _conteudo; set => _conteudo = value; }
            public int X { get => _x; set => _x = value; }
            public int Y { get => _y; set => _y = value; }
            public int Width { get => _width; set => _width = value; }
            public int Height { get => _height; set => _height = value; }
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
            List<PalavrasCoordenadasDocumento> lis = new List<PalavrasCoordenadasDocumento>();

            try
            {


                Engine.Init(null, null);    // use your company and product name here
                if (Engine.RECERR == RECERR.API_INIT_WARN)
                {

                }
                using (SettingCollection settings = new SettingCollection())
                {

                    using (Page page = new Page(caminho, 0, settings))
                    {
                        settings.PageDescription = PAGEDESCRIPTION.LZ_TABLE_AUTO;

                        page.LocateZones();

                        page.OCRZones.CopyToUserZones();

                        //foreach (UserZone userzone in page.UserZones)
                        //{
                        //    ZONE zone = userzone.GetZoneInfo();
                        //    //if (zone.type == ZONETYPE.WT_TABLE)
                        //    //{
                        //    ((InputZone)userzone).LocateTable(false);
                        //    //   }
                        //}

                        page.Recognize();

                        page.OCRZones.CopyToUserZones();


                        //   InputZone inputzone = userzone as InputZone;
                        //if (zone.type == ZONETYPE.WT_AUTO)
                        //{



                        // CELL_INFO[] cells = inputzone.GetTableCells();
                        StringBuilder sb = new StringBuilder();
                        bool inicio = true;
                        for (int i = 0; i < 1; i++)
                        {
                            StringBuilder sBuffer = new StringBuilder();


                            List<positionDs> lisds = new List<positionDs>();

                            LETTER[] letters = page[IMAGEINDEX.II_CURRENT].GetLetters();
                            for (int j = 0; j < letters.Length; j++)
                            {


                                if (letters[j].code == ' ')
                                {
                                    try
                                    {
                                        var we = letters[j];


                                        inicio = true;
                                        string x = "";
                                        string y = "";
                                        lisds.Add(new positionDs() { Conteudo = letters[j].code.ToString(), Height = letters[j].height, Width = letters[j].width, Y = letters[j].top, X = letters[j].left });
                                        var wid = lisds[lisds.Count - 1].X - lisds[0].X;
                                        var height = lisds.Max(_s => _s.Height);
                                        if (lisds[0] != null)
                                        {
                                            //var ds = CropImage(Bitmap.FromFile(strInput), lisds[0].X, lisds[0].Y, wid, height);
                                            //ds.Save(@"C:\Users\Administrador\Desktop\crop\ddddddddddddddddddddd" + Guid.NewGuid() + ".png");
                                            //ds.Dispose();
                                            Console.WriteLine(sb.ToString());
                                            lis.Add(new PalavrasCoordenadasDocumento() { texto = sb.ToString(), x = lisds[0].X, y = lisds[0].Y, height = height, width = wid });
                                        }
                                        sb = new StringBuilder();
                                        lisds = new List<positionDs>();
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                }
                                else
                                {
                                    sBuffer.Append(letters[j].code);
                                    lisds.Add(new positionDs() { Conteudo = letters[j].code.ToString(), Height = letters[j].height, Width = letters[j].width, Y = letters[j].top, X = letters[j].left });
                                    //var d = CropImage(Bitmap.FromFile(strInput), letters[j].left, letters[j].top, letters[j].width, letters[j].height);
                                    //d.Save(@"C:\Users\Administrador\Desktop\crop\"+Guid.NewGuid() + ".png");
                                    sb.Append(letters[j].code);
                                    inicio = false;
                                }
                            }


                        }
                        // }
                    }
                }
            }
            catch (Exception e)
            {

            }
            finally
            {

                Engine.ForceQuit();

            }
            return lis;
        }

        public string tipo()
        {
            return "OmniPageOcr";
        }
    }
}
