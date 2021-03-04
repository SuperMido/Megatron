using System.ComponentModel.DataAnnotations;

namespace Megatron.Models
{
    public class ArticleDocument
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ArticleId { get; set; }

        public string DocumentFile { get; set; }
    }
}