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
    
    public partial class Projeto_modelo_evento
    {
        public int ID { get; set; }
        public Nullable<System.DateTime> Data { get; set; }
        public int IdEvento { get; set; }
        public int IdProjetoModelo { get; set; }
    
        public virtual Eventos Eventos { get; set; }
        public virtual PROJETO_MODELO PROJETO_MODELO { get; set; }
    }
}