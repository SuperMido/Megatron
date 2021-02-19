using System.Collections.Generic;
using Megatron.Models;

namespace Megatron.ViewModels
{
    public class CommentArticleViewModel
    {
        public Article Article { get; set; }
        public IEnumerable<CommentArticle> Comments { get; set; }
        
        public IEnumerable<ApplicationUser> User { get; set; }
    }
}