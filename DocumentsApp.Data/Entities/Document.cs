using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace DocumentsApp.Data.Entities;

public class Document
{
    public Guid Id { get; set; }

    [Required]
    [MaxLength(20)]
    public string Name { get; set; }

    [AllowNull]
    [MaxLength(150)]
    public string Description { get; set; }
    
    [Required]
    [MaxLength(5000)]
    public string Content { get; set; }

    [Required] public DateTime DateCreated { get; set; } = DateTime.Now;

    public Guid AccountId { get; set; }
    public virtual Account Account { get; set; }
}