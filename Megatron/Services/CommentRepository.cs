using System.Threading.Tasks;
using Megatron.Data;
using Megatron.Models;

namespace Megatron.Services
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CommentRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> SaveComment(int articleId, string userId, string message)
        {
            var newComment = new CommentArticle
            {
                ArticleId = articleId,
                UserId = userId,
                Message = message
            };
            await _dbContext.CommentArticles.AddAsync(newComment);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}