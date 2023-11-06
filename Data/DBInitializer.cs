using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyAndSellBike.Data
{
    //this class is used for database seeding and database migration
    public class DBInitializer : IDBInitializer
    {
        private readonly BuyAndSellBikeDbContext dbContext;
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public DBInitializer(BuyAndSellBikeDbContext dbContext, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async void Initialize()
        {
            const string roleName = "Admin";
            if (dbContext.Database.GetPendingMigrations().Count() > 0)
            {
                //this will show an error if database is already migrated and no any count of migration so pls check pending migration count above.
                dbContext.Database.Migrate();
            }

            //Create Role Admin already exit -- if yes return;
            if (dbContext.Roles.Any(x => x.Name == roleName)) return;
            //if No🙄 create it
            roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();

            //create admin user
            userManager.CreateAsync(new ApplicationUser
            {
                UserName = "Admin",
                Email = "Admin@gmail.com",
                EmailConfirmed = true,
            }, "Admin@123").GetAwaiter().GetResult();

            //Assign Admin Role to Admin User
            await userManager.AddToRoleAsync(await userManager.FindByNameAsync("Admin"), roleName);
        }

    }
}
