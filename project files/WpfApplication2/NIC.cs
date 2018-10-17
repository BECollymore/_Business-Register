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
    
    public partial class NIC
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NIC()
        {
            this.ExternalKeys = new HashSet<ExternalKey>();
            this.NICLogs = new HashSet<NICLog>();
        }
    
        public string NICNumber { get; set; }
        public string Year { get; set; }
        public string Name { get; set; }
        public string RegistrarOfCompNum { get; set; }
        public string TradeName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string District { get; set; }
        public string Location { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public Nullable<System.DateTime> DateOfRegistration { get; set; }
        public Nullable<int> NumberOfEmployees { get; set; }
        public Nullable<int> TotalContributions { get; set; }
        public Nullable<byte> Sector { get; set; }
        public string NatureOfBusiness { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ExternalKey> ExternalKeys { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NICLog> NICLogs { get; set; }
    }
}
