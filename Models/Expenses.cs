using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Volontyor_Hakaton.Models
{
    public class Expenses
    {
        [Key]
        public int E_Id { get; set; }
        public decimal E_Price { get; set; }
     
        public string? Reason { get; set; }
        public DateTime DateTime { get; set; }
        [ForeignKey("ProjectId")]
        public Projects? Project { get; set; }
        public int ProjectId { get; set; }
    }
}
