using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Data.Models
{
    public class Expense
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The Title field is required.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "The Date field is required.")]
        public DateOnly Date { get; set; }

        [Required(ErrorMessage = "The Amount field is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public float Amount { get; set; }

        public bool Planned { get; set; }

        [Required(ErrorMessage = "The Category field is required.")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
