using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Megatron.Data;
using Megatron.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Megatron.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;


        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;

        }

        public async Task<string> GetUserFullName(string userId)
        {
            var user = await _dbContext.ApplicationUsers.FirstOrDefaultAsync(u => u.Id == userId);

            return user == null ? "Unknown" : user.FullName;
        }

        public IEnumerable<UserRoleViewModel> GetAllUsers()
        {
            var listUsers = (from user in _dbContext.ApplicationUsers
                    join userRoles in _dbContext.UserRoles on user.Id equals userRoles.UserId
                    join role in _dbContext.Roles on userRoles.RoleId equals role.Id
                    select new { UserId = user.Id, FullName = user.FullName, Email = user.Email, RoleName = role.Name, DateCreate = user.CreateAt })
                .ToList().Select( u => new UserRoleViewModel
                {
                    UserId = u.UserId,
                    FullName = u.FullName,
                    Email = u.Email,
                    Role = u.RoleName,
                    DateCreate = u.DateCreate.ToString("HH:mm - MM/dd/yyyy")
                });
            return listUsers;
        }
    }
}