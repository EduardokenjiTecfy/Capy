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
    
    public partial class TipoExtracao
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TipoExtracao()
        {
            this.Tipo_Regex_Extracao = new HashSet<Tipo_Regex_Extracao>();
        }
    
        public int ID { get; set; }
        public string Regex { get; set; }
        public string Tipo { get; set; }
        public string Descricao { get; set; }
        public Nullable<int> Usados { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tipo_Regex_Extracao> Tipo_Regex_Extracao { get; set; }
    }
}
