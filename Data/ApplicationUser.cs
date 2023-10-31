using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BuyAndSellBike.Data
{
    public class ApplicationUser : IdentityUser
    {
        [DisplayName("Office Phone")]
        public string PhoneNumber2 { get; set; }

        [NotMapped]
        public bool IsAdmim { get; set; }
    }
}
