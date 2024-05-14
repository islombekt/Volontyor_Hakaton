using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Volontyor_Hakaton.Models
{
    public class User_Project
    {
        [Key]
        public int up_Id { get; set; }
        [ForeignKey("ProjectId")]
        public Projects? Project { get; set; }
        public int ProjectId { get; set; }
        [ForeignKey("UserId")]
        public Users? User { get; set; }
        public int UserId { get; set; }
        public int Score { get; set; } = 1; // 1 ~ 5
    }
}
