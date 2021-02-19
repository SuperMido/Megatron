using Megatron.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Megatron.Models;

namespace Megatron.Data.DBInit
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public DbInitializer(ApplicationDbContext db, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async void Initialize()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Any())
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                throw new ApplicationException("Error when Migrate Database.", ex);
            }

            List<string> listRoles = new List<string>(new string[] { SystemRoles.Administrator,
                                                                     SystemRoles.MarketingCoordinator,
                                                                     SystemRoles.MarketingManager,
                                                                     SystemRoles.Student,
                                                                     SystemRoles.Guest});

            foreach (var role in listRoles)
            {
                if (_db.Roles.Any(r => r.Name == role)) continue;
                _roleManager.CreateAsync(new IdentityRole(role)).GetAwaiter().GetResult();
            }
            
            //Megatron Admin Account
            if (_userManager.FindByEmailAsync("megatronadmin@gmail.com").Result == null)
            {
                var result = _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "megatronadmin@gmail.com",
                    FullName = "Megatron Admin",
                    Email = "megatronadmin@gmail.com",
                    EmailConfirmed = true
                }, "Password@123").GetAwaiter().GetResult();
                if (result.Succeeded)
                {
                    var user = await _db.Users.FirstOrDefaultAsync(u => u.UserName == "megatronadmin@gmail.com");
                    await _userManager.AddToRoleAsync(user, SystemRoles.Administrator);
                }
            }
            //Megatron Marketing Manager Account
            if (_userManager.FindByEmailAsync("megatronmm@gmail.com").Result == null)
            {
                var result = _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "megatronmm@gmail.com",
                    FullName = "Megatron Marketing Manager",
                    Email = "megatronmm@gmail.com",
                    EmailConfirmed = true
                }, "Password@123").GetAwaiter().GetResult();
                if (result.Succeeded)
                {
                    var user = await _db.Users.FirstOrDefaultAsync(u => u.UserName == "megatronmm@gmail.com");
                    await _userManager.AddToRoleAsync(user, SystemRoles.MarketingManager);
                }
            }
            //Megatron Marketing Coordinator Account
            if (_userManager.FindByEmailAsync("megatronmc@gmail.com").Result == null)
            {
                var result = _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "megatronmc@gmail.com",
                    FullName = "Megatron Marketing Coordinator",
                    Email = "megatronmc@gmail.com",
                    EmailConfirmed = true
                }, "Password@123").GetAwaiter().GetResult();
                if (result.Succeeded)
                {
                    var user = await _db.Users.FirstOrDefaultAsync(u => u.UserName == "megatronmc@gmail.com");
                    await _userManager.AddToRoleAsync(user, SystemRoles.MarketingCoordinator);
                }
            }
            //Megatron student Account
            if (_userManager.FindByEmailAsync("megatronstudent@gmail.com").Result == null)
            {
                var result = _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "megatronstudent@gmail.com",
                    FullName = "Megatron Student",
                    Email = "megatronstudent@gmail.com",
                    EmailConfirmed = true
                }, "Password@123").GetAwaiter().GetResult();
                if (result.Succeeded)
                {
                    var user = await _db.Users.FirstOrDefaultAsync(u => u.UserName == "megatronstudent@gmail.com");
                    await _userManager.AddToRoleAsync(user, SystemRoles.Student);
                }
            }
            //Megatron Guest Account
            if (_userManager.FindByEmailAsync("megatronguest@gmail.com").Result == null)
            {
                var result = _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "megatronguest@gmail.com",
                    FullName = "Megatron Guest",
                    Email = "megatronguest@gmail.com",
                    EmailConfirmed = true
                }, "Password@123").GetAwaiter().GetResult();
                if (result.Succeeded)
                {
                    var user = await _db.Users.FirstOrDefaultAsync(u => u.UserName == "megatronguest@gmail.com");
                    await _userManager.AddToRoleAsync(user, SystemRoles.Guest);
                }
            }
            
            // If you want to create an new User, Duplicate the code below and change the account data
            // Data example from here (Copy from here)
            // Tran Quang Huy Account
            if (_userManager.FindByEmailAsync("hqtran@gmail.com").Result == null)
            {
                var result = _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "hqtran@gmail.com",
                    FullName = "Quang Huy",
                    Email = "hqtran@gmail.com",
                    EmailConfirmed = true
                }, "Password@123").GetAwaiter().GetResult();
                if (result.Succeeded)
                {
                    var user = await _db.Users.FirstOrDefaultAsync(u => u.UserName == "hqtran@gmail.com");
                    await _userManager.AddToRoleAsync(user, SystemRoles.Administrator);
                }
            }
            //To here
        }
    }
}