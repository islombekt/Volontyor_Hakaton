using Volontyor_Hakaton.Models;

namespace Volontyor_Hakaton.DTOs.Projects
{
    public class ProjectCreateDTO
    {
        public string ProjectName { get; set; }
        public string ProjectType { get; set; } = string.Empty; // Eco, ...
        public string? InitiatedBy { get; set; }
        public string Region { get; set; } = "Toshkent shahar";
        public string? AddressInfo { get; set; }
        public string? ExtraInfo { get; set; }
    }
}
