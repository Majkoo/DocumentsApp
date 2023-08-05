using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using DocumentsApp.Shared.Enums;

namespace DocumentsApp.Data.Entities;

public class Document
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Required] [MaxLength(20)] public string Name { get; set; }

    [AllowNull] [MaxLength(255)] public string Description { get; set; }

    [Required] public string Content { get; set; }

    [Required] public DateTime DateCreated { get; set; } = DateTime.Now;

    public string AccountId { get; set; }
    public virtual Account Account { get; set; }
    public virtual IEnumerable<DocumentAccessLevel> AccessLevels { get; set; }
}