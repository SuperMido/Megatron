using Megatron.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Megatron.ViewModels
{
    public class ArticleFacultyViewModel
    {
        public Article Article { get; set; }
        public IEnumerable<Faculty> Faculties { get; set; }
        public string StatusMessage { get; set; }
    }
}