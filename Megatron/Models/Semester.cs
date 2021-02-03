using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Megatron.Models
{
    public class Semester
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Semester")]
        public string SemesterName { get; set; }

        [Display(Name = "Semester Start")]
        public DateTime SemesterStartDate { get; set; }

        [Display(Name = "Semester End")]
        public DateTime SemesterEndDate { get; set; }

        private DateTime CreateAt { get; }
        public DateTime? UpdateAt { get; set; }

        public Semester()
        {
            CreateAt = DateTime.Now;
            UpdateAt = DateTime.Now;
        }
    }
}