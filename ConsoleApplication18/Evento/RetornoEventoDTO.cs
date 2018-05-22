using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication18.Evento
{
    public class RetornoEventoDTO
    {
       


        private Object retono;
        private EnumRetornoEvento retornoTipo;
        private string mensagemRetorno;
        public object Retono { get => retono; set => retono = value; }
        public string MensagemRetorno { get => mensagemRetorno; set => mensagemRetorno = value; }
        internal EnumRetornoEvento RetornoTipo { get => retornoTipo; set => retornoTipo = value; }
    }
}
