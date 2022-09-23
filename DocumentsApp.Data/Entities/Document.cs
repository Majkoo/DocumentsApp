using System.ComponentModel.DataAnnotations;

namespace DocumentsApp.Data.Entities;

public class Document
{
    public Guid Id { get; set; }
    
    [MaxLength(50)]
    public string Description { get; set; }
    [Required]
    public string Content { get; set; }

    public Guid CreatorId { get; set; }
    public virtual User Creator { get; set; }
}