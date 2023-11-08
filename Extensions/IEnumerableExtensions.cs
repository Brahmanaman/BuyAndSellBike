using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyAndSellBike.Extensions
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<SelectListItem> toSelectList<T>(this IEnumerable<T> Items)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            SelectListItem sli = new SelectListItem()
            {
                Text = "---Select---",
                Value = "0",
                Selected = true
            };
            list.Add(sli);
            foreach (var item in Items)
            {
                sli = new SelectListItem()
                {
                    Text = item.GetType().GetProperty("Name").GetValue(item).ToString(),
                    Value = item.GetType().GetProperty("Id").GetValue(item).ToString(),
                };
                list.Add(sli);
            }
            return list;
        }
    }
}
