using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyAndSellBike.Models.ViewModel
{
    public class ModelViewModel
    {
        public Model Model { get; set; }
        public IEnumerable<Make> Makes { get; set; }

        //Making a function to convert list Makes into SelectListitem
        public IEnumerable<SelectListItem> ConvertSelectListItem(IEnumerable<Make> Items)
        {
            List<SelectListItem> MakeList = new List<SelectListItem>();
            SelectListItem sli = new SelectListItem
            {
                Text="----Select----",
                Value="0"
            };
            MakeList.Add(sli);
            foreach(Make item in Items)
            {
                sli = new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString(),
                };
                MakeList.Add(sli);
            }
            return MakeList;
        }

    }
}
