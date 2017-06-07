using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Auctions.Models
{
    public class smsSendModel
    {
        [Required]
        [Display(Name = "Cell Number for SMS Confirmations")]
        public string CellNumber { get; set; }

        [Required]
        [Display(Name = "Message")]
        [DataType(DataType.MultilineText)]
        [StringLength(132, ErrorMessage = "Message cannot be longer than 132 characters.")]
        public string Message { get; set; }


    }
}