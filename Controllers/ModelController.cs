using Microsoft.AspNetCore.Mvc;
using BuyAndSellBike.Data;
using BuyAndSellBike.Models.ViewModel;
using System.Linq; 
using BuyAndSellBike.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace BuyAndSellBike.Controllers
{
    [Authorize(Roles ="Admin,Executive")]
    public class ModelController : Controller
    {
        private readonly BuyAndSellBikeDbContext dbContext = null;

        [BindProperty]
        public ModelViewModel ModelVM { get; set; } 
        
        public ModelController(BuyAndSellBikeDbContext dbContext)
        {
            this.dbContext = dbContext;
            ModelVM = new ModelViewModel()
            {
                Makes = dbContext.Makes.ToList(),
                Model = new Model()
            };
        }
        public IActionResult Index()
        {
            //var model = dbContext.Models.Include(x=>x.Make);
            var data = dbContext.Models.ToList();
            return View(data);
        }

        public IActionResult Create()
        {
            return View(ModelVM);
        }

        [HttpPost,ActionName("Create")]
        public async Task<IActionResult> CreatePost()
        {
            if (!ModelState.IsValid) return View(ModelVM);
            await dbContext.Models.AddAsync(ModelVM.Model);
            await dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            //ModelVM.Model = dbContext.Models.Include(m => m.Make).SingleOrDefault(m => m.Id == id);
            ModelVM.Model = dbContext.Models.Find(id);
            if (ModelVM.Model == null) return NotFound();
            return View(ModelVM);
        }

        [HttpPost,ActionName("Edit")]
        public IActionResult EditPost(ModelViewModel data)
        {
            if (!ModelState.IsValid) return View(data);
            dbContext.Models.Update(data.Model);
            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            Model model = dbContext.Models.Find(id);
            if (model == null) return NotFound();
            dbContext.Models.Remove(model);
            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
