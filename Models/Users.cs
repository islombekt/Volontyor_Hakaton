using System.ComponentModel.DataAnnotations;

namespace Volontyor_Hakaton.Models
{
    public class Users
    {
        [Key]
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string FIO { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public string UserRole { get; set; } = "Volontyor"; // Admin, Manager
        public string Description {  get; set; } = string.Empty;
        public virtual ICollection<User_Project>? User_Project { get; set; }
    }
}
