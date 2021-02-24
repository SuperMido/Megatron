using System.Collections.Generic;
using System.Threading.Tasks;
using Megatron.ViewModels;

namespace Megatron.Services
{
    public interface IUserRepository
    {
        public Task<string> GetUserFullName(string userId);

        public IEnumerable<UserRoleViewModel> GetAllUsers();
        
    }
}