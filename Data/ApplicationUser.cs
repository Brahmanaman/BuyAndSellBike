using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;


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
