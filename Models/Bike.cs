using System;
using System.ComponentModel.DataAnnotations;
using BuyAndSellBike.Extensions;

namespace BuyAndSellBike.Models
{
    public class Bike
    {
        public int Id { get; set; }

        public Make Make { get; set; }

        [RegularExpression("^[1-9]*$", ErrorMessage = "Select Model")]
        public int MakeId { get; set; }

        public Model Model { get; set; }

        [RegularExpression("^[1-9]*$", ErrorMessage ="Select Model")]
        public int ModelId { get; set; }

        [Required]
        [YearRangeTillNowDate(2000, ErrorMessage ="Invalid Year")]
        public int Year { get; set; }

        [Required]
        [Range(1,int.MaxValue, ErrorMessage = "Provide Mileage")]
        public int Mileage { get; set; }

        public string features { get; set; }

        [Required(ErrorMessage ="Provide Seller Name")]
        public string SellerName { get; set; }

        [Required(ErrorMessage ="Invalid Email ID")]
        [DataType(DataType.EmailAddress)]
        public string SellerEmail { get; set; }

        [Required(ErrorMessage ="Provide Seller Phone")]
        public string SellerPhone { get; set; }

        [Required(ErrorMessage ="Provide Selling Price")]
        public int Price { get; set; }
    
        [Required(ErrorMessage = "Provide Currency")]
        public string Currency { get; set; }
        public string ImagePath { get; set; }
    }
}
