using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Megatron.Models
{
    public class Faculty
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Faculty Name")]
        public string FacultyName { get; set; }

        public string Description { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }

        public Faculty()
        {
            CreateAt = DateTime.Now;
        }
    }
}