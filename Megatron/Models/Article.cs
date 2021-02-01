using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Megatron.Models
{
    public class Article
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Content { get; set; }
        public string Image { get; set; }

        [Display(Name = "Faculty")]
        public int FacultyId { get; set; }

        [ForeignKey("FacultyId")]
        public virtual Faculty Faculty { get; set; }

        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }

        public Article()
        {
            CreateAt = DateTime.Now;
        }
    }
}