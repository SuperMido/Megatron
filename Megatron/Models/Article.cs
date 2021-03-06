using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Megatron.Models
{
    public class Article
    {
        public Article()
        {
            CreateAt = DateTime.Now;
            UpdateAt = DateTime.Now;
        }

        [Key] public int Id { get; set; }

        [Required] public string Title { get; set; }

        [Required] public string Author { get; set; }

        public string Content { get; set; }

        public string Image { get; set; }

        [Display(Name = "Faculty")] public int FacultyId { get; set; }

        [ForeignKey("FacultyId")] public virtual Faculty Faculty { get; set; }

        public bool Status { get; set; }

        public string StatusMessage { get; set; }
        public string ChangeStatusBy { get; set; }

        public DateTime CreateAt { get; set; }

        public DateTime? UpdateAt { get; set; }
    }
}