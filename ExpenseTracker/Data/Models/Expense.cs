namespace ExpenseTracker.Data.Models
{
    public class Expense
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public DateOnly Date {  get; set; }

        public float Amount { get; set; }

        public Boolean Planned { get; set; }


    }
}
