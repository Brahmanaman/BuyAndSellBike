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

using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace BuyAndSellBike.Controllers
{
    [Authorize(Roles = "Admin,Executive")]
    public class BikeController : Controller
    {
        private readonly BuyAndSellBikeDbContext dbContext = null;
        private readonly IWebHostEnvironment Environment;

        [BindProperty]
        public BikeViewModel BikeVM { get; set; }

        public BikeController(BuyAndSellBikeDbContext dbContext, IWebHostEnvironment Environment)
        {
            this.dbContext = dbContext;
            this.Environment = Environment;
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

            //save bike logic
            //Get bike id we have saved in database
            var BikeId = BikeVM.Bike.Id;

            //Get wwwrootpath to save the file on server
            string wwwrootpath = Environment.WebRootPath;

            //get the upload file
            var files = HttpContext.Request.Form.Files;

            var savedBike = dbContext.Bikes.Find(BikeId);

            if(files.Count != 0)
            {
                var ImagePath = @"images\bike\";
                var Extension = Path.GetExtension(files[0].FileName);
                var RelativeImagePath = ImagePath + BikeId + Extension;
                var AbsImagePath = Path.Combine(wwwrootpath, RelativeImagePath);

                //upload the file on server
                using (var fileStream = new FileStream(AbsImagePath, FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                };

                //set image path to database
                savedBike.ImagePath = RelativeImagePath;
                dbContext.SaveChanges();
            }

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
