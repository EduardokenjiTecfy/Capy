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
    
    public partial class ControleQualidadeLog
    {
        public int ID { get; set; }
        public string caminho { get; set; }
        public Nullable<System.DateTime> data { get; set; }
        public Nullable<bool> processado { get; set; }
        public Nullable<int> exportId { get; set; }
        public Nullable<int> projetoId { get; set; }
        public Nullable<int> LogOcrId { get; set; }
        public string usuario { get; set; }
        public string indice { get; set; }
        public Nullable<bool> liberado { get; set; }
    
        public virtual LogOcr LogOcr { get; set; }
    }
}