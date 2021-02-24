using System.Threading.Tasks;

namespace Megatron.Services
{
    public interface ICommentRepository
    {
        Task<bool> SaveComment(int articleId, string userId, string message);
    }
}