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
    
    public partial class Palavras
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Palavras()
        {
            this.Documento = new HashSet<Documento>();
        }
    
        public int ID { get; set; }
        public Nullable<int> IdModelo { get; set; }
        public string Palavra { get; set; }
        public Nullable<bool> Contem { get; set; }
        public Nullable<bool> Ancora { get; set; }
        public Nullable<bool> Barcode { get; set; }
        public Nullable<bool> QRcode { get; set; }
        public Nullable<bool> GuardarValorIndexacao { get; set; }
        public Nullable<int> porcentagemMin { get; set; }
        public Nullable<int> porcentagemmax { get; set; }
        public Nullable<int> pagina { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Documento> Documento { get; set; }
        public virtual Modelo Modelo { get; set; }
    }
}
