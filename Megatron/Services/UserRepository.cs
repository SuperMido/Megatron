using System.Linq;
using System.Threading.Tasks;
using Megatron.Data;
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
    }
}