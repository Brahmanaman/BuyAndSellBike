using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BuyAndSellBike.Models;
using BuyAndSellBike.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace BuyAndSellBike.Controllers
{
    [Authorize(Roles = "Admin,Executive")]
    public class MakeController : Controller
    {
        private readonly BuyAndSellBikeDbContext dbContext = null;

        public MakeController(BuyAndSellBikeDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        //make/bike
        public async Task<IActionResult> Index()
        {
            var data = await dbContext.Makes.ToListAsync();
            return View(data);
        }

        public  IActionResult Create() 
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Make make)
        {
            if (ModelState.IsValid)
            {
                await dbContext.Makes.AddAsync(make);
                await dbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(make);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await dbContext.Makes.FindAsync(id);
            if (data == null) return NotFound();
            dbContext.Makes.Remove(data);
            await dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var data = await dbContext.Makes.FindAsync(id);
            if (data == null) return NotFound();
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Make make)
        {
            if (id != make.Id) return NotFound();
            if (ModelState.IsValid)
            {
                dbContext.Makes.Update(make);
                await dbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(make);
        }

    }
}
