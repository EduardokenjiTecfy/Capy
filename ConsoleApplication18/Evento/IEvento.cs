using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication18.Evento
{
    public interface IEvento
    {
        //Antes de capturar Documento
        object C001(object parametros );
        //Durante a capturar Documento
        object C002(object parametros, EnumEntrada entrada);
        //Após a capturar Documento
        object C003(object parametros, EnumEntrada entrada);
       // Durante a captura dos indices
        object C004(object parametros, EnumEntrada entrada);


    }
}
