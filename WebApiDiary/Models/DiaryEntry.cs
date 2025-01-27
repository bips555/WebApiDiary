using System.ComponentModel.DataAnnotations;

namespace WebApiDiary.Models
{
    public class DiaryEntry
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public DateTime DateCreated { get; set; } = DateTime.Now;

    }
}
