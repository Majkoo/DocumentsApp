using DocumentsApp.Data.Enums;

namespace DocumentsApp.Data.Entities;

public class DocumentAccessLevel
{
    public Guid Id { get; set; }
    
    public Guid UserId { get; set; }
    public virtual User User { get; set; }
    
    public Guid DocumentId { get; set; }
    public virtual Document Document { get; set; }
    
    public AccessLevel AccessLevel { get; set; }
}