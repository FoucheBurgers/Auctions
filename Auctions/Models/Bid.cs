using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Auctions.Models
{
    public class Bid
    {
        public int ID { get; set; }
        public int? RollId { get; set; }
        public int? BuyerId { get; set; }
        public decimal? NewBidPrice { get; set; }
        public decimal? BidTotalPrice { get; set; }
        public int UserID { get; set; }
        public bool bidClosed { get; set; }
        public bool bidToLow { get; set; }
//        public string errMessage { get; set; }
    }
}