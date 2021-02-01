using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Megatron.Models
{
    public class SemesterArticle
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Semester")]
        public int SemesterId { get; set; }

        [ForeignKey("SemesterId")]
        public virtual Semester Semester { get; set; }

        [Required]
        [Display(Name = "Article")]
        public int ArticleId { get; set; }

        [ForeignKey("ArticleId")]
        public virtual Article Article { get; set; }

        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }

        public SemesterArticle()
        {
            CreateAt = DateTime.Now;
        }
    }
}