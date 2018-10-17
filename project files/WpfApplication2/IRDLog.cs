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
    
    public partial class IRDLog
    {
        public string IRDNumber { get; set; }
        public System.DateTime LogDate { get; set; }
        public string Name { get; set; }
        public string TradeName { get; set; }
        public string MailingAddress { get; set; }
        public string MailingCity { get; set; }
        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public string EmailAddress { get; set; }
        public Nullable<int> ValTaxSupply { get; set; }
        public string Primary_ISIC { get; set; }
        public string Secondary_ISIC { get; set; }
        public Nullable<System.DateTime> TaxSuppStartDate { get; set; }
        public Nullable<byte> LegalStatus { get; set; }
        public string Representative { get; set; }
    
        public virtual IRD IRD { get; set; }
    }
}