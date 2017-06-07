using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Auctions.Models
{
    [MetadataType(typeof(DefaultSetupMetaData))]
    public partial class DefaultSetup
    {
    }
    public class DefaultSetupMetaData
    {
        // Copy van hier 
        public int ID { get; set; }
        public string Description { get; set; }
        public Nullable<bool> Active { get; set; }
        [DisplayName("Refresh Time")]
        public Nullable<int> RefreshTime { get; set; }
        [DisplayName("Delay Time After Last Bit")]
        public Nullable<int> AuctionDelayTime { get; set; }
        [Required]
        [DisplayName("Logo Path")]
        public string LogoPath { get; set; }
        [Required]
        [DisplayName("Logo Name")]
        public string LogoName { get; set; }
        [Required]
        [DisplayName("Small Logo Name")]
        public string SmallLogoName { get; set; }
        [Required]
        [DisplayName("Roll Images Path")]
        public string RollImagesPath { get; set; }
        [DisplayName("Background Color Home Page")]
        public string BackgroundColorHome { get; set; }
        [DisplayName("Background Color")]
        public string BackgroundColor { get; set; }
        [DisplayName("Font Color")]
        public string FontColor { get; set; }
        [DisplayName("Logo Background Color")]
        public string LogoBackgroundColor { get; set; }
        [DisplayName("Allow SMS'")]
        public Nullable<bool> SMSAllFunctionality { get; set; }
        [DisplayName("SMS Confirmation for Registration")]
        public Nullable<bool> SMSCustRegistration { get; set; }
        [DisplayName("SMS Confirmation Required/Log in")]
        public Nullable<bool> SMSConfirmationLoginRequired { get; set; }
        [DisplayName("SMS New Bidder")]
        public Nullable<bool> SmsNotification { get; set; }
        [DisplayName("eMail Confirmation for Registration")]
        public Nullable<bool> emailConfirmRegistration { get; set; }
        [DisplayName("eMail Confirmation Required/Log in")]
        public Nullable<bool> emailConfirmationLoginRequired { get; set; }
        [DisplayName("Default Auction")]
        public Nullable<int> DefaultAuction { get; set; }
        [DisplayName("SMS Outgoing Bidder")]
        public Nullable<bool> smsOutBidder { get; set; }
        [DisplayName("Lines on roll")]
        public Nullable<int> DispLines { get; set; }
        [DisplayName("Columns on roll")]
        public Nullable<int> DispColumns { get; set; }
        [DisplayName("Roll Refresh Rate")]
        public Nullable<int> RollDispRefreshRate { get; set; }

        [DisplayName("(Home) Auction/Period Description")]
        public string HomePeriodDescription { get; set; }
        [DisplayName("Auction/Period Description Text Color")]
        public string HomePeriodDescriptionTextColor { get; set; }
        [DisplayName("Auction/Period Description Text Background Color")]
        public string HomePeriodDescriptionBackColor { get; set; }
        [DisplayName("Home Action Text Color (not implemented)")]
        public string ActionTextColor { get; set; }
        [DisplayName("Home Action Text Background Color")]
        public string ActionBackColor { get; set; }
        [DisplayName("Roll Bid Background Color")]
        public string RollActionBidColor { get; set; }
        [DisplayName("Roll Back to Index Background Color")]
        public string RollActionBackIndexColor { get; set; }



        public virtual ltRollDescription ltRollDescription { get; set; }

    }
}