using System.Threading.Tasks;

namespace Megatron.Services
{
    public interface IUserRepository
    {
        public Task<string> GetUserFullName(string userId);
    }
}