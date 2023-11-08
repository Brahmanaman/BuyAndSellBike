using Microsoft.AspNetCore.Mvc;
using BuyAndSellBike.Data;
using BuyAndSellBike.Models.ViewModel;
using System.Linq;
using BuyAndSellBike.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace BuyAndSellBike.Controllers
{
    [Authorize(Roles = "Admin,Executive")]
    public class BikeController : Controller
    {
        private readonly BuyAndSellBikeDbContext dbContext = null;

        [BindProperty]
        public BikeViewModel BikeVM { get; set; }

        public BikeController(BuyAndSellBikeDbContext dbContext)
        {
            this.dbContext = dbContext;
            BikeVM = new BikeViewModel()
            {
                Makes = dbContext.Makes.ToList(),
                Models = dbContext.Models.ToList(),
                Currencies = dbContext.Currencies.ToList(),
                Bike = new Models.Bike(),
            };
        }
        public IActionResult Index()
        {
            var bike = dbContext.Bikes.Include(x => x.Make).Include(x=>x.Model);
            return View(bike.ToList());
            
        }

        public IActionResult Create()
        {
            return View(BikeVM);
        }

        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost()
        {
            if (!ModelState.IsValid) return View(BikeVM);
            await dbContext.Bikes.AddAsync(BikeVM.Bike);
            await dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        //public IActionResult Edit(int id)
        //{
        //    //ModelVM.Model = dbContext.Models.Include(m => m.Make).SingleOrDefault(m => m.Id == id);
        //    ModelVM.Model = dbContext.Models.Find(id);
        //    if (ModelVM.Model == null) return NotFound();
        //    return View(ModelVM);
        //}

        //[HttpPost, ActionName("Edit")]
        //public IActionResult EditPost(ModelViewModel data)
        //{
        //    if (!ModelState.IsValid) return View(data);
        //    dbContext.Models.Update(data.Model);
        //    dbContext.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        //[HttpPost]
        //public IActionResult Delete(int id)
        //{
        //    Model model = dbContext.Models.Find(id);
        //    if (model == null) return NotFound();
        //    dbContext.Models.Remove(model);
        //    dbContext.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        
        
    }
}
