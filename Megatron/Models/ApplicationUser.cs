using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Megatron.Models
{
    public class ApplicationUser : IdentityUser
    {
        private DateTime CreateAt { get; }
        public DateTime? UpdateAt { get; set; }

        public ApplicationUser()
        {
            CreateAt = DateTime.Now;
            UpdateAt = DateTime.Now;
        }
    }
}