using Megatron.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Megatron.Data.DBInit
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(ApplicationDbContext db, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _roleManager = roleManager;
        }

        public void Initialize()
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
        }
    }
}