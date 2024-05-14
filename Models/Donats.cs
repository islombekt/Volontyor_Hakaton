using System.ComponentModel.DataAnnotations;

namespace Volontyor_Hakaton.Models
{
    public class Donats
    {
        [Key]
        public int DonatId { get; set; }
        public string? Donater {  get; set; }
        public decimal DonatAmount {  get; set; }
        public DateTime DonatedAt { get; set; }
        public List<string> DotanetFor {  get; set; } = new List<string>();

    }
}
