 
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract;

namespace TesteTesseract
{
    class Program
    {
        static void Main(string[] args)
        {
            var testImagePath = @"C:\Users\Administrador\Desktop\Coopercarga\Canhotos\8.png";
            var dataPath = @"C:\Users\Administrador\Documents\Visual Studio 2015\Projects\ConsoleApplication18\OcrTesseract\Content\tessdata";
            
            try
            {
                using (var tEngine = new Tesseract.TesseractEngine(dataPath, "eng", EngineMode.Default)) //creating the tesseract OCR engine with English as the language
                {
                    using (var img = Pix.LoadFromFile(testImagePath)) // Load of the image file from the Pix object which is a wrapper for Leptonica PIX structure
                    {
                        using (var page = tEngine.Process(img)) //process the specified image
                        {
                            var text = page.GetText(); //Gets the image's content as plain text.
                            Console.WriteLine(text); //display the text
                            Console.WriteLine(page.GetMeanConfidence()); //Get's the mean confidence that as a percentage of the recognized text.
                            Console.ReadKey();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected Error: " + e.Message);
            }
        }
    }
}
