using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Auctions.Models;
using System.ComponentModel.DataAnnotations;

namespace Auctions.Models
{
    public class AdminBidModel
    {
        //[Required]
        //public string Lot { get; set; }
        [Required]
        public int LotID { get; set; }
        [Required]
        public int BuyerID { get; set; }
        [Required]
        
        public decimal NewBidPrice { get; set; }

    }

}