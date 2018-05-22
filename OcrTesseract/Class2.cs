using NuGet.Tessnet2.src;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrTesseract
{
    class Class2
    {
        public static void Main(string[] args)
        {
            var d = new Ocr(@"C:\Users\Administrador\Documents\Visual Studio 2015\Projects\ConsoleApplication18\OcrTesseract\Content\tessdata");
            var dd = d.DoOcrNormal((Bitmap)Image.FromFile(@"C:\Users\Administrador\Desktop\Coopercarga\Canhotos\8.png"), "eng");
        }
    }
}
