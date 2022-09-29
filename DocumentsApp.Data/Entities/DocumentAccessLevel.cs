using DocumentsApp.Data.Enums;

namespace DocumentsApp.Data.Entities;

public class DocumentAccessLevel
{
    public Guid Id { get; set; }

    public Guid AccountId { get; set; }

    public Guid DocumentId { get; set; }

    public AccessLevel AccessLevel { get; set; }

    public DateTime AccessLevelDateGranted { get; set; }

    public virtual Account Account { get; set; }
    public virtual Document Document { get; set; }
}