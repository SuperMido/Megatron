using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Megatron.Data;
using Megatron.ViewModels;
using Megatron.Models;
using Megatron.Utility;
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

        public IEnumerable<UserFaculty> GetUserInFaculty()
        {
            var userInFaculty = _dbContext.UserFaculties.Include(u => u.ApplicationUser)
                .Include(u => u.Faculty).ToList();

            return userInFaculty;
        }

        public UserFacultyViewModel GetMcInFaculty()
        {
            var mcUsers = GetListUser(SystemRoles.MarketingCoordinator);

            var userFacultyViewModel = new UserFacultyViewModel
            {
                User = mcUsers,
                Faculty = _dbContext.Faculties.ToList(),
                UserFaculty = new Models.UserFaculty()
            };

            return userFacultyViewModel;
        }

        public UserFacultyViewModel GetGuestInFaculty()
        {
            var guestUsers = GetListUser(SystemRoles.Guest);

            var userFacultyViewModel = new UserFacultyViewModel
            {
                User = guestUsers,
                Faculty = _dbContext.Faculties.ToList(),
                UserFaculty = new Models.UserFaculty(),
            };

            return userFacultyViewModel;
        }

        public bool AssignMcToFaculty(UserFacultyViewModel model)
        {
            var mcUsers = GetListUser(SystemRoles.MarketingCoordinator);

            if (CheckUserExistInFaculty(model)) return false;

            _dbContext.UserFaculties.Add(model.UserFaculty);
            _dbContext.SaveChanges();
            
            return true;

        }

        public bool AssignGuestToFaculty(UserFacultyViewModel model)
        {
            var guestUsers = GetListUser(SystemRoles.Guest);

            if (CheckUserExistInFaculty(model)) return false;

            _dbContext.UserFaculties.Add(model.UserFaculty);
            _dbContext.SaveChanges();
            
            return true;
        }

        private IEnumerable<ApplicationUser> GetListUser(string id)
        {
            var userRoleId = _dbContext.Roles.Where(t => t.Name == id).Select(t => t.Id)
                .FirstOrDefault();

            var listUserId = _dbContext.UserRoles
                .Where(t => t.RoleId == userRoleId)
                .Select(t => t.UserId)
                .ToArray();

            var listUsers = _dbContext.ApplicationUsers
                .Where(t => listUserId
                    .Any(name => name.Equals(t.Id)))
                .ToList();

            return listUsers;
        }

        private bool CheckUserExistInFaculty(UserFacultyViewModel model)
        {
            var doesUserExistsInFaculty = _dbContext.UserFaculties.Include(t => t.Faculty).Include(t => t.ApplicationUser)
                .Where(t => t.Faculty.Id == model.UserFaculty.FacultyId && t.ApplicationUser.Id == model.UserFaculty.UserId);

            return doesUserExistsInFaculty.Any();
        }

        public bool DeleteAccount(string id)
		    {
            var accountInDb = GetUserById(id);
            if (accountInDb == null) return false;
            _dbContext.ApplicationUsers.Remove(accountInDb);
            _dbContext.SaveChanges();
            return true;
		    }
        public ApplicationUser GetUserById(string id)
		    {
            return _dbContext.ApplicationUsers.SingleOrDefault(u => u.Id == id);
		    }

        public bool EditAccount(ApplicationUser applicationUser)
		    {
			      var accountInDb = GetUserById(applicationUser.Id);
			      if (accountInDb == null) return false;

            accountInDb.FullName = applicationUser.FullName;
            accountInDb.PhoneNumber = applicationUser.PhoneNumber;
			      _dbContext.ApplicationUsers.Update(accountInDb);
            _dbContext.SaveChanges();
            return true;
		    }
    }
}