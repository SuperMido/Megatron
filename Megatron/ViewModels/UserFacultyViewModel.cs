using System.Collections.Generic;
using Megatron.Models;

namespace Megatron.ViewModels
{
    public class UserFacultyViewModel
    {
        public UserFaculty UserFaculty { get; set; }
        public IEnumerable<ApplicationUser> User { get; set; }
        public IEnumerable<Faculty> Faculty { get; set; }
    }
}