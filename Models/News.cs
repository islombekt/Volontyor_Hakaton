using System.ComponentModel.DataAnnotations;

namespace Volontyor_Hakaton.Models
{
    public class News
    {
        [Key]
        public int NewId { get; set; }
        public string NewName { get; set; }
        public string NewDescription { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string ImageUrl {  get; set; }= string.Empty;

    }
}
