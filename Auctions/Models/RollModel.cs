using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Auctions.Models
{
    using System;
    using System.Collections.Generic;

    [MetadataType(typeof(tblRollMetaData))]
    public partial class tblRoll  // Ietest is nie reg nie. 
    {

    }
    public class tblRollMetaData
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        //public tblRoll()
        //{
        //    this.BidHistories = new HashSet<BidHistory>();
        //}

        public int ID { get; set; }
        public Nullable<int> RollId { get; set; }
        public string Lot { get; set; }

        [DisplayName("Specy")]
        public Nullable<int> SpeciesCode { get; set; }
        [DisplayName("Tag Number")]
        public string TagNr { get; set; }
        public string Age { get; set; }
        [DisplayName("Date Measured")]
        public string DateMeasured { get; set; }
        [DisplayName("Horn Length")]
        public string HornLength { get; set; }
        [DisplayName("Tip To Tip")]
        public string TipToTip { get; set; }

        [DisplayName("Other Info")]
        [DataType(DataType.MultilineText)]
        public string OtherInfo { get; set; }
        [DisplayName("Date Available")]
        public string DateAvailable { get; set; }
        [DisplayName("Number of Males")]
        public Nullable<int> Male { get; set; }
        [DisplayName("Number of Females")]
        public Nullable<int> Female { get; set; }
        [DisplayName("Number of Young")]
        public Nullable<int> Young { get; set; }
        [DisplayName("Total Number to pay for")]
        [DisplayFormat(DataFormatString = "{0:0.###}")]
        public Nullable<decimal> Quantity { get; set; }
        [DisplayName("Type of Bid")]
        public string Quantity_Lot { get; set; }


        [DisplayName("Seller")]
        public Nullable<int> SellerId { get; set; }
        [DisplayName("Buyer")]
        public Nullable<int> BuyerId { get; set; }
        [DisplayName("Bidding Price")]
        [DisplayFormat(DataFormatString = "{0:0.###}")]
        public Nullable<decimal> BiddingPrice { get; set; }
        [DisplayName("Bid Date Time")]
        public Nullable<System.DateTime> BidDateTime { get; set; }
        [DisplayName("Bid Total Price")]
        [DisplayFormat(DataFormatString = "{0:0.###}")]
        public Nullable<decimal> BidTotalPrice { get; set; }
        [DisplayName("New Bid Price")]
        [DisplayFormat(DataFormatString = "{0:0.###}")]
        public Nullable<decimal> NewBidPrice { get; set; }
        [DisplayName("New Bidder")]
        public Nullable<int> NewBidder { get; set; }
        public byte[] Picture { get; set; }
        [DisplayName("On Auction")]
        public bool OnAuction { get; set; }
        [DisplayName("Sold")]
        public bool Sold { get; set; }
        [DisplayName("Date Loaded")]
        public Nullable<System.DateTime> DateLoaded { get; set; }
        [DisplayName("Date Sold")]
        public Nullable<System.DateTime> DateSold { get; set; }


        [DisplayName("Picture Path")]
        public string PicturePath { get; set; }
        [DisplayName("Picture Name")]
        public string PictureName { get; set; }


        [DisplayName("Minimum Increment")]
        public Nullable<decimal> Increments { get; set; }
        [DisplayName("Reserve Price")]
        [DisplayFormat(DataFormatString = "{0:0.###}")]
        public Nullable<decimal> ReservePrice { get; set; }
        public Nullable<decimal> LotQ { get; set; }
        [DisplayName("Date Time Bid")]
        public Nullable<System.DateTime> DateTimeBid { get; set; }
        [DisplayName("Date Time Close")]
        public Nullable<System.DateTime> DateTimeClose { get; set; }
        public string CustomerNumber { get; set; }
        [DisplayName("Bid Open?")]
        public Nullable<bool> BidOpen { get; set; }
        public string AuctionBuyerNo { get; set; }


        public virtual ltRollDescription ltRollDescription { get; set; }
        public virtual ltRollDescription ltRollDescription1 { get; set; }
        public virtual ltSpecy ltSpecy { get; set; }
        public virtual tblCustomer tblCustomer { get; set; }
        public virtual tblCustomer tblCustomer1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BidHistory> BidHistories { get; set; }
    }






    //[SystemDiagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    //public tblRoll()
    //{
    //    this.BidHistories = new HashSet<BidHistory>();
    //}


    //      public int ID { get; set; }
    //      public Nullable<int> RollId { get; set; }
    //      //[Required]
    //      public string Lot { get; set; }
    //      [DisplayName("Specy")]
    //      public Nullable<int> SpeciesCode { get; set; }
    //      [DisplayName("Tag Number")]
    //      public string TagNr { get; set; }
    //      public string Age { get; set; }
    //      [DisplayName("Date Measured")]
    //      public string DateMeasured { get; set; }
    //      [DisplayName("Horn Length")]
    //      public string HornLength { get; set; }
    //      [DisplayName("Tip To Tip")]
    //      public string TipToTip { get; set; }
    //      [DisplayName("Other Info")]
    //      [DataType(DataType.MultilineText)]
    //      public string OtherInfo { get; set; }

    //      [DisplayName("Date Available")]
    //      public string DateAvailable { get; set; }
    //      [DisplayName("Number of Males")]
    //      public Nullable<int> Male { get; set; }
    //      [DisplayName("Number of Females")]
    //      public Nullable<int> Female { get; set; }
    //      [DisplayName("Number of Young")]
    //      public Nullable<int> Young { get; set; }
    //      [DisplayName("Total Number to pay for")]
    //     // [DisplayFormat(DataFormatString = "{0:0.###}")]
    //      public Nullable<decimal> Quantity { get; set; }
    //      [DisplayName("Type of Bid")]
    //      public string Quantity_Lot { get; set; }


    //      [DisplayName("Seller")]
    //      public Nullable<int> SellerId { get; set; }
    //      [DisplayName("Buyer")]
    //      public Nullable<int> BuyerId { get; set; }
    //      [DisplayName("Bidding Price")]
    //     // [DisplayFormat(DataFormatString = "{0:0.###}")]
    //      public Nullable<decimal> BiddingPrice { get; set; }
    //      [DisplayName("Bid Date Time")]
    //      public Nullable<System.DateTime> BidDateTime { get; set; }
    //      [DisplayName("Bid Total Price")]
    //     // [DisplayFormat(DataFormatString = "{0:0.###}")]
    //      public Nullable<decimal> BidTotalPrice { get; set; }
    //      [DisplayName("New Bid Price")]
    //     // [DisplayFormat(DataFormatString = "{0:0.###}")]
    //      public Nullable<decimal> NewBidPrice { get; set; }
    //      [DisplayName("New Bidder")]
    //      public Nullable<int> NewBidder { get; set; }
    //      public byte[] Picture { get; set; }
    //      [DisplayName("On Auction")]
    //      public bool OnAuction { get; set; }
    //      [DisplayName("Sold")]
    //      public bool Sold { get; set; }
    //      [DisplayName("Date Loaded")]
    //      public Nullable<System.DateTime> DateLoaded { get; set; }
    //      [DisplayName("Date Sold")]
    //      public Nullable<System.DateTime> DateSold { get; set; }

    //      [DisplayName("Picture Path")]
    //      public string PicturePath { get; set; }
    //      [DisplayName("Picture Name")]
    //      [Required]
    //      public string PictureName { get; set; }


    //      [DisplayName("Minimum Increment")]
    //      [Required]
    //      public Nullable<decimal> Increments { get; set; }
    //      [DisplayName("Reserve Price")]
    //     // [DisplayFormat(DataFormatString = "{0:0.###}")]
    //      public Nullable<decimal> ReservePrice { get; set; }
    //      public Nullable<decimal> LotQ { get; set; }
    //      [DisplayName("Date Time Bid")]
    //      public Nullable<System.DateTime> DateTimeBid { get; set; }
    //      [DisplayName("Date Time Close")]
    //      public Nullable<System.DateTime> DateTimeClose { get; set; }
    //      public string CustomerNumber { get; set; }
    //      [DisplayName("Bid Open?")]
    //      public Nullable<bool> BidOpen { get; set; }

    //      public virtual ltRollDescription ltRollDescription { get; set; }
    //      public virtual ltRollDescription ltRollDescription1 { get; set; }
    //      public virtual ltSpecy ltSpecy { get; set; }
    //      public virtual tblCustomer tblCustomer { get; set; }
    //      public virtual tblCustomer tblCustomer1 { get; set; }
    ////      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
    //      public virtual ICollection<BidHistory> BidHistories { get; set; }
}

