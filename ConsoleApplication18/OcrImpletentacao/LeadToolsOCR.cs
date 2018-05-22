using Leadtools;
using Leadtools.Forms;
using Leadtools.Forms.Ocr;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication18.OcrImpletentacao
{
    public class LeadToolsOCR : IOcr
    {
        public static bool SetLicense(bool silent)
        {
            //try
            //{
            //   // TODO: Change this to use your license file and developer key */
            //   string licenseFilePath = "Replace this with the path to the LEADTOOLS license file";
            //   string developerKey = "Replace this with your developer key";
            //   RasterSupport.SetLicense(licenseFilePath, developerKey);
            //}
            //catch (Exception ex)
            //{
            //   System.Diagnostics.Debug.Write(ex.Message);
            //}

            if (RasterSupport.KernelExpired)
            {
                string dir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                /* Try the common LIC directory */
                string licenseFileRelativePath = dir + @"\eval-license-files.lic";
                string keyFileRelativePath = dir + @"\eval-license-files.lic.key";

                if (System.IO.File.Exists(licenseFileRelativePath) && System.IO.File.Exists(keyFileRelativePath))
                {
                    string developerKey = System.IO.File.ReadAllText(keyFileRelativePath);
                    try
                    {
                        RasterSupport.SetLicense(licenseFileRelativePath, developerKey);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.Write(ex.Message);
                    }
                }
            }

            if (RasterSupport.KernelExpired)
            {
                if (silent == false)
                {
                    string msg = "Your license file is missing, invalid or expired. LEADTOOLS will not function. Please contact LEAD Sales for information on obtaining a valid license.";
                    string logmsg = string.Format("*** NOTE: {0} ***{1}", msg, Environment.NewLine);
                    System.Diagnostics.Debugger.Log(0, null, "*******************************************************************************" + Environment.NewLine);
                    System.Diagnostics.Debugger.Log(0, null, logmsg);
                    System.Diagnostics.Debugger.Log(0, null, "*******************************************************************************" + Environment.NewLine);


                    System.Diagnostics.Process.Start("https://www.leadtools.com/downloads/evaluation-form.asp?evallicenseonly=true");
                }

                return false;
            }
            return true;
        }
        public StringBuilder executarOcr(string caminho)
        {
            StringBuilder retorno = new StringBuilder();
            SetLicense(false);
            IOcrEngine ocrEngine = OcrEngineManager.CreateEngine(OcrEngineType.Advantage, false);
            ocrEngine.Startup(null, null, null, @"C:\LEADTOOLS 19\Bin\Common\OcrAdvantageRuntime");
            ocrEngine.LanguageManager.EnableLanguages(new string[] { "en" });
 
            ocrEngine.SpellCheckManager.SpellCheckEngine = OcrSpellCheckEngine.Native;

            IOcrDocument ocrDocument = ocrEngine.DocumentManager.CreateDocument();

            // Add all the pages of a multi-page TIF image to the document 
            ocrDocument.Pages.AddPages(caminho, 1, -1, null);

            // *** Step 4: (Optional) Establish zones on the page(s), either manually or automatically 

            // Automatic zoning 
            ocrDocument.Pages.AutoZone(null);


            // Recognize this page 
            foreach (var item in ocrDocument.Pages)
            {
                var ocrPage = item;
                // Recognize this page 
                ocrPage.Recognize(null);
                var page = item;

                foreach (OcrZone oz in ocrPage.Zones)
                {
                    int index = ocrPage.Zones.IndexOf(oz);
                    retorno.Append(page.GetText(index) + " ");

                }

            }
            return retorno;
        }

        public StringBuilder executarOcr(byte[] arquivo)
        {
            StringBuilder retorno = new StringBuilder();
            SetLicense(false);
            IOcrEngine ocrEngine = OcrEngineManager.CreateEngine(OcrEngineType.Advantage, false);
            ocrEngine.Startup(null, null, null, @"C:\LEADTOOLS 19\Bin\Common\OcrAdvantageRuntime");
            ocrEngine.LanguageManager.EnableLanguages(new string[] { "en" });

            ocrEngine.SpellCheckManager.SpellCheckEngine = OcrSpellCheckEngine.Native;

            IOcrDocument ocrDocument = ocrEngine.DocumentManager.CreateDocument();

            Stream stream = new MemoryStream(arquivo);
            ocrDocument.Pages.AddPages(stream, 1, -1, null);

            // *** Step 4: (Optional) Establish zones on the page(s), either manually or automatically 

            // Automatic zoning 
            ocrDocument.Pages.AutoZone(null);

            foreach (var item in ocrDocument.Pages)
            {
                var ocrPage = item;
                // Recognize this page 
                ocrPage.Recognize(null);
                var page = item;

                foreach (OcrZone oz in ocrPage.Zones)
                {
                    int index = ocrPage.Zones.IndexOf(oz);
                    retorno.Append(page.GetText(index) + " ");

                }

            }

            return retorno;
        }

        public List<PalavrasCoordenadasDocumento> executarOcrCoordenadas(string caminho)
        {
            List<PalavrasCoordenadasDocumento> palavrasOrdenadas = new List<PalavrasCoordenadasDocumento>();
            SetLicense(false);
            IOcrEngine ocrEngine = OcrEngineManager.CreateEngine(OcrEngineType.Advantage, false);
            ocrEngine.Startup(null, null, null, @"C:\LEADTOOLS 19\Bin\Common\OcrAdvantageRuntime");
            ocrEngine.LanguageManager.EnableLanguages(new string[] { "en" });

            ocrEngine.SpellCheckManager.SpellCheckEngine = OcrSpellCheckEngine.Native;
            IOcrDocument ocrDocument = ocrEngine.DocumentManager.CreateDocument();

            // Add all the pages of a multi-page TIF image to the document 
            ocrDocument.Pages.AddPages(caminho, 1, -1, null);

            // *** Step 4: (Optional) Establish zones on the page(s), either manually or automatically 

            // Automatic zoning 
            ocrDocument.Pages.AutoZone(null);
            var ocrPage = ocrDocument.Pages[0];

            // Recognize this page 
            ocrPage.Recognize(null);


            IOcrPageCharacters ocrPageCharacters = ocrPage.GetRecognizedCharacters();

            foreach (IOcrZoneCharacters ocrZoneCharacters in ocrPageCharacters)
            {
                // Show the words found in this zone. Get the word boundaries in inches 
                ICollection<OcrWord> words = ocrZoneCharacters.GetWords(ocrPage.DpiX, ocrPage.DpiY, LogicalUnit.Inch);

                foreach (OcrWord word in words)
                {
                    PalavrasCoordenadasDocumento pl = new PalavrasCoordenadasDocumento();
                    pl.texto = word.Value;
                    pl.x = (int)word.Bounds.X;
                    pl.y = (int)word.Bounds.Y;
                    pl.width = (int)word.Bounds.Width;
                    pl.height = (int)word.Bounds.Height;
                    palavrasOrdenadas.Add(pl);

                }
            }
            return palavrasOrdenadas;
        }

        public List<PalavrasCoordenadasDocumento> executarOcrCoordenadas(byte[] arquivo)
        {
            throw new NotImplementedException();
        }

        public string tipo()
        {
            return "LeadToolsOCR";
        }
    }
}
