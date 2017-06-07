using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Auctions.Models
{

    public class DefaultSetupModel
    {
        public int? ID { get; set; }
        public string AuctionID { get; set; }
        public string Description { get; set; }
        public bool? Active { get; set; }
        public string BackgroundColor { get; set; }
        public string FontColor { get; set; }
        public string LogoPath { get; set; }
        public string LogoName { get; set; }
        public string SmallLogoName { get; set; }
        public string LogoBackgroundColor { get; set; }
        public string RollImagePath { get; set; }
        public string RefreshTime { get; set; } // html werk net met strings
        public string AuctionDelayTime { get; set; }
        public bool? SMSNewBidder { get; set; }
        public string message { get; set; }
        public bool? SMSAllFunctionality { get; set; }
        public bool? SMSCustRegistration { get; set; }
        public bool? SMSConfirmationLoginRequired { get; set; }
        public bool? emailConfirmRegistration { get; set; }
        public bool? emailConfirmationLoginRequired { get; set; }
        public string BackgroundColorHome { get; set; }
        public int? DefaultAuction { get; set; }
        public bool? SMSOutBidder { get; set; }
        public string RollImagesBackColor { get; set; }
        public int? pageSize { get; set; }
        public Nullable<int> DispColumns { get; set; }
        public Nullable<int> DispLines { get; set; }
        public Nullable<int> RollDispRefreshRate { get; set; }

        public string HomePeriodDescription { get; set; }
        public string HomePeriodDescriptionTextColor { get; set; }
        public string HomePeriodDescriptionBackColor { get; set; }
        public string ActionTextColor { get; set; }
        public string ActionBackColor { get; set; }
        public string RollActionBidColor { get; set; }
        public string RollActionBackIndexColor { get; set; }



    }

    public class SetectedAuction
    {
             public int?  AuctionID { get; set; }
    }

    public class CurrentBuyer
    {
        //[Required]
        //public string Lot { get; set; }
        public string NewBuyerCell { get; set; }
        public int? NewBuyerID { get; set; }
        public bool NewBuyerHaveCellNumber { get; set; }

        public string OutBuyerCell { get; set; }
        public int? OutBuyerID { get; set; }
        public bool OutBuyerHaveCellNumber { get; set; }

    }

}