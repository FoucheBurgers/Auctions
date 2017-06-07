using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Auctions.Models
{
    [MetadataType(typeof(ltRollDescriptionMetaData))]
    public partial class ltRollDescription
    {
    }

    public class ltRollDescriptionMetaData
    {

        public int ID { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        [DisplayName("Roll Background Color")]
        public string BackgroundColor { get; set; }
        [DisplayName("Font Color")]
        public string FontColor { get; set; }
        [DisplayName("Refresh Time")]
        public Nullable<int> RefreshTime { get; set; }
        [DisplayName("Start Date")]
        public Nullable<System.DateTime> StartDate { get; set; }
        [DisplayName("End Date")]
        public Nullable<System.DateTime> EndDate { get; set; }
        [DisplayName("Delay Time at Close")]
        public Nullable<int> AuctionDelayTime { get; set; }
        [DisplayName("Sort Position")]
        public Nullable<int> SortPosition { get; set; }
        [DisplayName("Logo Path")]
        public string LogoPath { get; set; }
        [DisplayName("Logo Name")]
        public string LogoName { get; set; }
        [DisplayName("Small Logo Name")]
        
        public string SmallLogoName { get; set; }
        [DisplayName("Roll Images Path")]
        [Required]
        public string RollImagesPath { get; set; }
        [DisplayName("Sms New Bidder")]
        public Nullable<bool> SmsNotification { get; set; }
        [DisplayName("Logo Background Color")]
        public string LogoBackgroundColor { get; set; }
        [DisplayName("Sms Outgoing Bidder")]
        public Nullable<bool> smsOutBidder { get; set; }
        [DisplayName("Roll Image Background Color")]
        public string RollImagesBackColor { get; set; }

        [DisplayName("(Home) Auction/Period Description")]
        public string HomePeriodDescription { get; set; }
        [DisplayName("Auction/Period Description Text Color")]
        public string HomePeriodDescriptionTextColor { get; set; }
        [DisplayName("Auction/Period Description Text Background Color")]
        public string HomePeriodDescriptionBackColor { get; set; }
        [DisplayName("Home Action Text Color (not implemented)")]
        public string ActionTextColor { get; set; }
        [DisplayName("Home Action Background Color")]
        public string ActionBackColor { get; set; }
        [DisplayName("Roll Bid Background Color")]
        public string RollActionBidColor { get; set; }
        [DisplayName("Roll Back to Index Background Color")]
        public string RollActionBackIndexColor { get; set; }



        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblRoll> tblRolls { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblRoll> tblRolls1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BuyerNo> BuyerNoes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DefaultSetup> DefaultSetups { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BidHistory> BidHistories { get; set; }
    }
}

