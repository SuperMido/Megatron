using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Megatron.Models
{
    public class ArticleDocument
    {
        public ArticleDocument()
        {
            CreateAt = DateTime.Now;
            UpdateAt = DateTime.Now;
        }
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Article")]
        public int ArticleId { get; set; }

        [ForeignKey("ArticleId")]
        public virtual Article Article { get; set; }

        public string DocumentFile { get; set; }
        public DateTime CreateAt { get; set; }

        public DateTime? UpdateAt { get; set; }
    }
}