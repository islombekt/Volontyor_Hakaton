namespace Volontyor_Hakaton.DTOs.Identity
{
    public class RegisterUser
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; }
        public string FIO { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public string UserRole { get; set; } = "Volontyor"; // Admin, Manager
        public string Description { get; set; } = string.Empty;
    }
}
