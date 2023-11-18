using Microsoft.AspNetCore.Mvc;
using BuyAndSellBike.Data;
using BuyAndSellBike.Models.ViewModel;
using System.Linq;
using BuyAndSellBike.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using cloudscribe.Pagination.Models;

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
       
        [AllowAnonymous]
        public IActionResult Index(string searchString, string sortOrder, int pageNumber=1, int pageSize = 1)
        {
            ViewBag.CurrentSortOrder = sortOrder;
            ViewBag.CurrentFilter = searchString;
            ViewBag.PriceSortParam = string.IsNullOrEmpty(sortOrder) ? "price_desc" : "";

            //pagination logic
            int ExcludeRecords = (pageSize * pageNumber) - pageSize;

            var bike = from b in dbContext.Bikes.Include(x => x.Make).Include(x => x.Model)
                       select b;

            var BikeCount = bike.Count();

            if (!string.IsNullOrEmpty(searchString))
            {
                bike = bike.Where(b => b.Make.Name.Contains(searchString));
            }

            //sorting logic
            switch(sortOrder)
            {
                case "price_desc":
                    bike = bike.OrderByDescending(b => b.Price);
                    break;
                default:
                    bike = bike.OrderBy(b => b.Price);
                    break;
            }

            bike = bike.Skip(ExcludeRecords).Take(pageSize);

            var result = new PagedResult<Bike>
            {
                Data = bike.AsNoTracking().ToList(),
                TotalItems =BikeCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
            };
            return View(result);
        }

        public IActionResult Create()
        {
            return View(BikeVM);
        }

        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost()
        {
            if (!ModelState.IsValid)
            {
                BikeVM.Makes = dbContext.Makes.ToList();
                BikeVM.Models = dbContext.Models.ToList();
                BikeVM.Currencies = dbContext.Currencies.ToList(); 
                return View(BikeVM);
            }
            await dbContext.Bikes.AddAsync(BikeVM.Bike);
            UploadImageIfAvailable();
            await dbContext.SaveChangesAsync();

            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        private void UploadImageIfAvailable()
        {
            //save bike logic
            //Get bike id we have saved in database
            var BikeId = BikeVM.Bike.Id;

            //Get wwwrootpath to save the file on server
            string wwwrootpath = Environment.WebRootPath;

            //get the upload file
            var files = HttpContext.Request.Form.Files;

            var savedBike = dbContext.Bikes.Find(BikeId);

            if (files.Count != 0)
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

            }
        }


        public IActionResult Edit(int id)
        {
            BikeVM.Bike = dbContext.Bikes.SingleOrDefault(b => b.Id == id);

            //filter the models associated to the make of bike
            BikeVM.Models = dbContext.Models.Where(m => m.MakeId == BikeVM.Bike.MakeId);
            if(BikeVM.Bike == null)
            {
                return NotFound();
            }

            return View(BikeVM);
        }

        [AllowAnonymous ]
        public IActionResult View(int id)
        {
            BikeVM.Bike = dbContext.Bikes.SingleOrDefault(b => b.Id == id);
            if (BikeVM.Bike == null)
            {
                return NotFound();
            }

            return View(BikeVM);
        }





        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost()
        {
            if (!ModelState.IsValid)
            {
                BikeVM.Makes = dbContext.Makes.ToList();
                BikeVM.Models = dbContext.Models.ToList();
                BikeVM.Currencies = dbContext.Currencies.ToList();
                return View(BikeVM);
            }
            dbContext.Bikes.Update(BikeVM.Bike);
            UploadImageIfAvailable();
            await dbContext.SaveChangesAsync();

            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }



        [HttpPost]
        public IActionResult Delete(int id)
        {
            Bike bike = dbContext.Bikes.Find(id);
            if (bike == null) return NotFound();
            dbContext.Bikes.Remove(bike);
            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }



    }
}
