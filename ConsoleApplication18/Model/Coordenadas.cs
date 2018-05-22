using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication18.Model
{
    class Coordenadas
    {

        private int _x;
        private int _y;
        private int _w;
        private int _h;

        public int H { get => _h; set => _h = value; }
        public int W { get => _w; set => _w = value; }
        public int Y { get => _y; set => _y = value; }
        public int X { get => _x; set => _x = value; }
    }
}
