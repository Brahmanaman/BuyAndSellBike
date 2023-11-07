using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyAndSellBike.Models.ViewModel
{
    public class BikeViewModel
    {
        public Bike Bike { get; set; }
        public IEnumerable<Make> Makes { get; set; }
        public IEnumerable<Model> Models { get; set; }
        public IEnumerable<Currency> Currencies { get; set; }
    }
}
