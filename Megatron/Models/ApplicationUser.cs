using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Megatron.Models
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }

        public ApplicationUser()
        {
            CreateAt = DateTime.Now;
        }
    }
}