using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Megatron.Models
{
    public class UserFaculty
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "User")]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        [Required]
        [Display(Name = "Faculty")]
        public int FacultyId { get; set; }

        [ForeignKey("FacultyId")]
        public virtual Faculty Faculty { get; set; }

        private DateTime CreateAt { get; }
        public DateTime UpdateAt { get; set; }

        public UserFaculty()
        {
            CreateAt = DateTime.Now;
        }
    }
}