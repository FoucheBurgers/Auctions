//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Auctions.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class BuyerNo
    {
        public int ID { get; set; }
        public Nullable<int> CustomerID { get; set; }
        public Nullable<int> RollID { get; set; }
        public string BuyerNumber { get; set; }
    
        public virtual tblCustomer tblCustomer { get; set; }
        public virtual ltRollDescription ltRollDescription { get; set; }
    }
}
