using System.ComponentModel.DataAnnotations;

namespace DocumentsApp.Data.Entities;

// tu jest git, ale generalnie User jest klasą z pewnego Nugeta i dlatego czytelniej byloby nazwac to Account.
// dodalem pare propek, moze sie przydadza a jak nie to sie je usunie
public class User
{
    public Guid Id { get; set; }
    
    [Required]
    [MaxLength(20)]
    public string UserName { get; set; }
    
    [Required]
    [EmailAddress]
    [MaxLength(150)]
    public string Email { get; set; }

    [Required]
    public string PasswordHash { get; set; }

    [Required]
    public Guid EmailConfirmationToken { get; set; } = Guid.NewGuid();

    [Required]
    public Guid PasswordResetToken { get; set; } = Guid.NewGuid();

    [Required]
    public bool AccountConfirmed { get; set; } = false;

    // nav props
    public ICollection<Document> Documents { get; set; }
}