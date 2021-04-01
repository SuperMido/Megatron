using System.Collections;
using System.Collections.Generic;
using Megatron.Models;

namespace Megatron.ViewModels
{
    public class HomeViewModel
    {
        public Semester Semester { get; set; }
        public IEnumerable<Semester> Semesters { get; set; }
        public IEnumerable<Article> Articles { get; set; }
        public IEnumerable<Faculty> Faculties { get; set; }
    }
}