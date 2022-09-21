using System.ComponentModel.DataAnnotations;

namespace DocumentsApp.Data.Entities;

public class Document
{
    public int Id { get; set; }
    
    [MaxLength(50)]
    public string Description { get; set; }
    [Required]
    public string Content { get; set; }

    public int CreatorId { get; set; }
    public virtual User Creator { get; set; }
}