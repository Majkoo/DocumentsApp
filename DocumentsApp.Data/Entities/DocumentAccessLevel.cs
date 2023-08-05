using System.ComponentModel.DataAnnotations;
using DocumentsApp.Shared.Enums;

namespace DocumentsApp.Data.Entities;

public class DocumentAccessLevel
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Required] public AccessLevelEnum AccessLevelEnum { get; set; }

    [Required] public DateTime AccessLevelGranted { get; set; } = DateTime.Now;

    public string AccountId { get; set; }
    public string DocumentId { get; set; }

    public virtual Account Account { get; set; }
    public virtual Document Document { get; set; }
}