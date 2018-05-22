using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication18.Model
{
   public  class PosicaoLabel
    {
        private Coordenadas _coordenadas;
        private String _label;

        public string Label { get => _label; set => _label = value; }
        internal Coordenadas Coordenadas { get => _coordenadas; set => _coordenadas = value; }
    }
}
