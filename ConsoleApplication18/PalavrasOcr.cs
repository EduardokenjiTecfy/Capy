using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication18
{


    public class DetectedLanguage
    {
        public string languageCode { get; set; }
    }

    public class DetectedBreak
    {
        public string type { get; set; }
    }

    public class Property
    {
        public List<DetectedLanguage> detectedLanguages { get; set; }
        public DetectedBreak detectedBreak { get; set; }
    }

    public class Vertex
    {
        public int x { get; set; }
        public int y { get; set; }
    }

    public class BoundingBox
    {
        public List<Vertex> vertices { get; set; }
    }

    public class PalavrasOcr
    {
        public Property property { get; set; }
        public BoundingBox boundingBox { get; set; }
        public string text { get; set; }
    }

}
