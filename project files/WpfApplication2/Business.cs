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
    
    public partial class Business
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Business()
        {
            this.AddressLogs = new HashSet<AddressLog>();
            this.BusinessLogs = new HashSet<BusinessLog>();
            this.ISICLogs = new HashSet<ISICLog>();
            this.NumEmpLogs = new HashSet<NumEmpLog>();
            this.NumSalariedEmpLogs = new HashSet<NumSalariedEmpLog>();
            this.PercentForeignLogs = new HashSet<PercentForeignLog>();
            this.RevenueLogs = new HashSet<RevenueLog>();
            this.SalesLogs = new HashSet<SalesLog>();
            this.WagesLogs = new HashSet<WagesLog>();
        }
    
        public string StatisticalNumber { get; set; }
        public string UniqueID { get; set; }
        public string LegalName { get; set; }
        public string OperationalName { get; set; }
        public string Address { get; set; }
        public string LegalAddress { get; set; }
        public string District { get; set; }
        public string PostalCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Fax { get; set; }
        public string GPS { get; set; }
        public string Email { get; set; }
        public string URL { get; set; }
        public byte BusinessTypeCode { get; set; }
        public byte BusinessStatusCode { get; set; }
        public Nullable<System.DateTime> YearBegunOperation { get; set; }
        public Nullable<System.DateTime> FiscalPeriodStart { get; set; }
        public Nullable<System.DateTime> FiscalPeriodEnd { get; set; }
        public Nullable<double> PercentForeignOwn { get; set; }
        public Nullable<double> Revenue { get; set; }
        public string RevenueSource { get; set; }
        public Nullable<System.DateTime> RevenueDate { get; set; }
        public Nullable<long> NumberOfEmployees { get; set; }
        public string NumberOfEmployeesSource { get; set; }
        public Nullable<System.DateTime> NumberOfEmployeesDate { get; set; }
        public Nullable<long> NumberOfSalariedEmployees { get; set; }
        public string NumberOfSalariedEmployeesSource { get; set; }
        public Nullable<System.DateTime> NumberOfSalariedEmployeesDate { get; set; }
        public Nullable<double> Sales { get; set; }
        public string SalesSource { get; set; }
        public Nullable<System.DateTime> SalesDate { get; set; }
        public Nullable<double> Wages { get; set; }
        public string WagesSource { get; set; }
        public Nullable<System.DateTime> WagesDate { get; set; }
        public string ISIC { get; set; }
        public string ISICSource { get; set; }
        public Nullable<System.DateTime> ISICDate { get; set; }
        public string SectorCode { get; set; }
        public string LegalCode { get; set; }
        public int LastUserIdUpdated { get; set; }
        public Nullable<System.DateTime> LastUpdateDate { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AddressLog> AddressLogs { get; set; }
        public virtual BusinessStatu BusinessStatu { get; set; }
        public virtual BusinessType BusinessType { get; set; }
        public virtual Sector Sector { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BusinessLog> BusinessLogs { get; set; }
        public virtual Contact Contact { get; set; }
        public virtual ExternalKey ExternalKey { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ISICLog> ISICLogs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NumEmpLog> NumEmpLogs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NumSalariedEmpLog> NumSalariedEmpLogs { get; set; }
        public virtual OptionalBusinessAttribute OptionalBusinessAttribute { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PercentForeignLog> PercentForeignLogs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RevenueLog> RevenueLogs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SalesLog> SalesLogs { get; set; }
        public virtual StatisticalStructure StatisticalStructure { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WagesLog> WagesLogs { get; set; }
    }
}