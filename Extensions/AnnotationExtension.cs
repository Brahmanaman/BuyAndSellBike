using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BuyAndSellBike.Extensions
{
    public class YearRangeTillNowDate:RangeAttribute
    {
        public YearRangeTillNowDate(int startYear):base(startYear, DateTime.Today.Year)
        {

        }
    }
}
