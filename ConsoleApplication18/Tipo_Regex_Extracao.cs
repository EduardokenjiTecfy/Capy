//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ConsoleApplication18
{
    using System;
    using System.Collections.Generic;
    
    public partial class Tipo_Regex_Extracao
    {
        public int ID { get; set; }
        public int TipoExtracaoId { get; set; }
        public int IdProjetoModelo { get; set; }
        public int IdOrientacaoModelo { get; set; }
        public int ordem { get; set; }
    
        public virtual OrientacaoDocumento OrientacaoDocumento { get; set; }
        public virtual PROJETO_MODELO PROJETO_MODELO { get; set; }
        public virtual TipoExtracao TipoExtracao { get; set; }
    }
}
