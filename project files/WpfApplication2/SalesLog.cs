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
    
    public partial class SalesLog
    {
        public int ID { get; set; }
        public string StatisticalNumber { get; set; }
        public Nullable<double> Sales { get; set; }
        public string SalesSource { get; set; }
        public Nullable<System.DateTime> SalesDate { get; set; }
        public Nullable<int> LasUserIDUpdated { get; set; }
    
        public virtual Business Business { get; set; }
    }
}
