using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyAndSellBike.Models
{
    public class Model
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //navigation property
        public Make Make { get; set; }

        //foreign property
        public int MakeId { get; set; }
    }
}
