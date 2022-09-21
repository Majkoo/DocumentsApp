using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace DocumentsApp.Data.Entities;

public class DocAuthorization
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    public virtual User User { get; set; }
    
    public int DocumentId { get; set; }
    public virtual Document Document { get; set; }
    
    public AccessLevelEnum AccessLevelEnum { get; set; }
}