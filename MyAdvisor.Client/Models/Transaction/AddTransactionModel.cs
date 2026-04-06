using System.ComponentModel.DataAnnotations;

namespace MyAdvisor.Client.Models.Transaction;

public class AddTransactionModel
{
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero")]
    public decimal Amount { get; set; }

    [Required(ErrorMessage = "Description is required")]
    public string? Description { get; set; }

    [Required]
    public DateTime TransactionDate { get; set; } = DateTime.Today;

    [Required(ErrorMessage = "Payment method is required")]
    public string? PaymentMethod { get; set; }

    [Required(ErrorMessage = "Category is required")]
    public int? CategoryId { get; set; }
}
