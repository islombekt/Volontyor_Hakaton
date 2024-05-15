using System.ComponentModel.DataAnnotations.Schema;

namespace Volontyor_Hakaton.DTOs.Expenses
{
    public class ExpenseCreate
    {
        public decimal E_Price { get; set; }

        public string? Reason { get; set; }
    
        public int ProjectId { get; set; }
    }
}
