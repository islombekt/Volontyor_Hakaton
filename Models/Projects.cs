using System.ComponentModel.DataAnnotations;

namespace Volontyor_Hakaton.Models
{
    public class Projects
    {
        [Key]
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectType { get; set; } = string.Empty; // Eco, ...
        public string? InitiatedBy { get; set; }    
        public Status Status { get; set; }
        public string Region { get; set; } = "Toshkent shahar";
        public string? AddressInfo {  get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string? ExtraInfo {  get; set; }
        public virtual ICollection<Expenses>? Expenses { get; set; }
        public virtual ICollection<User_Project>? User_Project { get; set; }

    }

    public enum Status
    {
        New,
        Verifiyed,
        Active,
        Finished
    }
}
