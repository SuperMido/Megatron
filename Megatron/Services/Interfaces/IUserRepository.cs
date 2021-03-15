using System.Collections.Generic;
using System.Threading.Tasks;
using Megatron.ViewModels;
using Megatron.Models;

namespace Megatron.Services
{
    public interface IUserRepository
    {
        public Task<string> GetUserFullName(string userId);

        public IEnumerable<UserRoleViewModel> GetAllUsers();


        public IEnumerable<UserFaculty> GetUserInFaculty();

        public UserFacultyViewModel GetMcInFaculty();
        public UserFacultyViewModel GetGuestInFaculty();

        public bool AssignMcToFaculty(UserFacultyViewModel model);
        public bool AssignGuestToFaculty(UserFacultyViewModel model);
        bool DeleteAccount(string id);
        ApplicationUser GetUserById(string id);
        bool EditAccount(ApplicationUser applicationUser);

        bool LockUser(string id);
        bool UnLockUser(string id);
    }
}