using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Auctions.Models
{
    public class RollAdminModel
    {
        [Required]
        public int? AuctionID { get; set; }
        public string Lot { get; set; }

    }
}