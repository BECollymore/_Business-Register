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
    
    public partial class StatisticalStructure
    {
        public string ChildStatisticalNumber { get; set; }
        public string ParentStatisticalNumber { get; set; }
    
        public virtual Business Business { get; set; }
    }
}