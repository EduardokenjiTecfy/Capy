using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication18
{
    public interface IOcr
    {
        string tipo( );
        StringBuilder executarOcr(String caminho);

        StringBuilder executarOcr(byte[] arquivo);

        List<PalavrasCoordenadasDocumento> executarOcrCoordenadas(String caminho);

        List<PalavrasCoordenadasDocumento> executarOcrCoordenadas(byte[] arquivo);
    }
}
