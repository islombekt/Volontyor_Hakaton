namespace Volontyor_Hakaton.DTOs.Dashboard
{
    public class DonatsDTO
    {
        public  decimal OverallDonated { get{
            return DonatsChart.Sum(d=>d.Amount);
            } } 
        public decimal OverallExpenses {
            get
            {
                return ExpensesChart.Sum(d => d.Amount);
            }
        }
       public List<DonatsChart> DonatsChart { get; set; } = new List<DonatsChart>();
        public List<ExpensesChart> ExpensesChart { get; set; } = new List<ExpensesChart>();
    }
    public class DonatsChart
    {
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
     
    }
    public class ExpensesChart
    {
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }

    }
}
