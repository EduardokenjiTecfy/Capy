using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication18.Factory
{
    public class FactoryOcr
    {
        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, ignoreCase: true);
        }
        public static IOcr FactoryOcrInstancia(int projetoId)
        {


            var context = new Entidade();
            var projeto = context.PROJETO.Where(_proj => _proj.ID == projetoId).FirstOrDefault();
            var tipo = context.CONFIGURACAO.Where(_s => _s.ID == projeto.ConfiguracaoID).FirstOrDefault();
            IOcr retonoInstancia = null;
            var enumocr = ParseEnum<EnumOcr>(tipo.TIPO);
            switch (enumocr)
            {
                case EnumOcr.GoogleOcr:
                    Type GoogleOcr = Type.GetType("ConsoleApplication18.GoogleOcr");
                    retonoInstancia = (IOcr)Activator.CreateInstance(GoogleOcr);
                    break;
                case EnumOcr.TesseractOcr:
                    Type TesseractOcr = Type.GetType("ConsoleApplication18.OcrImpletentacao.TesseractOcr");
                    retonoInstancia = (IOcr)Activator.CreateInstance(TesseractOcr);
                    break;
                case EnumOcr.NicomSoftOcr:
                    break;
                case EnumOcr.GdpictureOcr:
                    break;
                case EnumOcr.LeadToolsOCR:
                    Type LeadToolsOCR = Type.GetType("ConsoleApplication18.OcrImpletentacao.LeadToolsOCR");
                    retonoInstancia = (IOcr)Activator.CreateInstance(LeadToolsOCR);

                    break;
                case EnumOcr.OmniPageOcr:
                    Type OmniPageOcr = Type.GetType("ConsoleApplication18.OcrImpletentacao.OmniPageOcr");
                    retonoInstancia = (IOcr)Activator.CreateInstance(OmniPageOcr);

                    break;

                default:
                    break;
            }
            return retonoInstancia;
        }


    }
}
