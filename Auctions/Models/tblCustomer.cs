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
    
    public partial class tblCustomer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblCustomer()
        {
            this.tblRolls = new HashSet<tblRoll>();
            this.tblRolls1 = new HashSet<tblRoll>();
            this.BuyerNoes = new HashSet<BuyerNo>();
            this.BidHistories = new HashSet<BidHistory>();
            this.BidHistories1 = new HashSet<BidHistory>();
            this.BidHistories2 = new HashSet<BidHistory>();
        }
    
        public int ID { get; set; }
        public string CustomerID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string CompanyName { get; set; }
        public string ContactPerson { get; set; }
        public string SA_ID { get; set; }
        public string CompanyID { get; set; }
        public string VATNr { get; set; }
        public string TaxNr { get; set; }
        public string PostalAddress1 { get; set; }
        public string PostalAddress2 { get; set; }
        public string PostalAddress3 { get; set; }
        public string PostalCity { get; set; }
        public string PostalCode { get; set; }
        public string ResAddress1 { get; set; }
        public string ResAddress2 { get; set; }
        public string ResAddress3 { get; set; }
        public string ResCity { get; set; }
        public string ResPostalCode { get; set; }
        public string eMail { get; set; }
        public string CellPhone { get; set; }
        public string Phone { get; set; }
        public string Bank { get; set; }
        public string BranchName { get; set; }
        public string BranchCode { get; set; }
        public string BankAccountNr { get; set; }
        public Nullable<double> Commotion { get; set; }
        public string Agent { get; set; }
        public string Language { get; set; }
        public bool Active { get; set; }
        public Nullable<int> LinkKey { get; set; }
        public Nullable<bool> VATRegistered { get; set; }
        public string CustomerNumber { get; set; }
        public string PIN { get; set; }
        public Nullable<bool> FICA { get; set; }
        public Nullable<System.DateTime> DateLoaded { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblRoll> tblRolls { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblRoll> tblRolls1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BuyerNo> BuyerNoes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BidHistory> BidHistories { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BidHistory> BidHistories1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BidHistory> BidHistories2 { get; set; }
    }
}
