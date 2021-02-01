﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Megatron.Models
{
    public class ArticleFaculty
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Article")]
        public int ArticleId { get; set; }

        [ForeignKey("ArticleId")]
        public virtual Article Article { get; set; }

        [Required]
        [Display(Name = "Faculty")]
        public int FacultyId { get; set; }

        [ForeignKey("FacultyId")]
        public virtual Faculty Faculty { get; set; }

        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }

        public ArticleFaculty()
        {
            CreateAt = DateTime.Now;
        }
    }
}