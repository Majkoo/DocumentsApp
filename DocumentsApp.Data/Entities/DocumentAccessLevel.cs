using DocumentsApp.Data.Enums;

namespace DocumentsApp.Data.Entities;

public class DocumentAccessLevel
{
    public string Id { get; set; }

    public string AccountId { get; set; }

    public string DocumentId { get; set; }

    public AccessLevel AccessLevel { get; set; }

    public DateTime AccessLevelGranted { get; set; } = DateTime.Now;

    public virtual Account Account { get; set; }
    public virtual Document Document { get; set; }
}