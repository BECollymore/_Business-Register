//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace StatsBR
{
    using System;
    using System.Collections.Generic;
    
    public partial class VAT
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public VAT()
        {
            this.ExternalKeys = new HashSet<ExternalKey>();
            this.VATLogs = new HashSet<VATLog>();
        }
    
        public string VATNumber { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
        public string StandardRatedSupplies { get; set; }
        public string Hotel { get; set; }
        public string ZeroRatedSupplies { get; set; }
        public string ExemptSupplies { get; set; }
        public string TotalSupplies { get; set; }
        public string TotalOutputTax { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ExternalKey> ExternalKeys { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VATLog> VATLogs { get; set; }
    }
}