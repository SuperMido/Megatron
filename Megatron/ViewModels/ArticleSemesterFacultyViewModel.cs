using Megatron.Models;
using System.Collections.Generic;

namespace Megatron.ViewModels
{
    public class ArticleSemesterFacultyViewModel
    {
        public Article Article { get; set; }
        public Faculty Faculties { get; set; }
        public Semester Semester { get; set; }
    }
}