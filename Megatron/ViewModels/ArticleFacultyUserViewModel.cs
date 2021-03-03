using Megatron.Models;
using System.Collections.Generic;

namespace Megatron.ViewModels
{
    public class ArticleFacultyViewModel
    {
        public Article Article { get; set; }
        public IEnumerable<Faculty> Faculties { get; set; }
        public string StatusMessage { get; set; }
        public string UserFullName { get; set; }
    }
}