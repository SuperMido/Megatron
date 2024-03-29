﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Megatron.Models
{
    public class Semester
    {
        [Key] public int Id { get; set; }

        [Display(Name = "Semester")] public string SemesterName { get; set; }

        [Display(Name = "Semester Start")] public DateTime SemesterStartDate { get; set; }

        [Display(Name = "Semester Closure Date")]
        public DateTime SemesterClosureDate { get; set; }

        [Display(Name = "Semester Final Date")]
        public DateTime SemesterEndDate { get; set; }

        public DateTime CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }

        public Semester()
        {
            CreateAt = DateTime.Now;
            UpdateAt = DateTime.Now;
        }
    }
}