using System.ComponentModel.DataAnnotations;

namespace MyAdvisor.Client.Models.Diary;

public class CreateDiaryModel
{
    [Required]
    public DateTime Date { get; set; } = DateTime.Today;
    public string? Notes { get; set; }
}
