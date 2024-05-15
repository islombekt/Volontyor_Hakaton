namespace Volontyor_Hakaton.DTOs.Donats
{
    public class CreateDonat
    {
        public string? Donater { get; set; }
        public decimal DonatAmount { get; set; }
        public string DotanetFor { get; set; } = "Barchasi";
    }
}
