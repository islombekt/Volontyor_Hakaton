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
        public string DotanetFor { get; set; } = "Barchasi";
    }
}
