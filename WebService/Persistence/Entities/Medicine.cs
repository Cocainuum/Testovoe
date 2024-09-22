using System.ComponentModel.DataAnnotations;

namespace WebService.Persistence.Entities;

public class Medicine
{
    public long Id { get; set; }
    [Required]
    public string Name { get; set; }
    public string? Description { get; set; }
    [Required]
    public double Price { get; set; }
}