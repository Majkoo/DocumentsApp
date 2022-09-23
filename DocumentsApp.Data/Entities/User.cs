using System.ComponentModel.DataAnnotations;

namespace DocumentsApp.Data.Entities;

public class User
{
    public Guid Id { get; set; }
    
    [Required]
    [MaxLength(20)]
    public string UserName { get; set; }
    
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    public string PasswordHash { get; set; }

    public ICollection<Document> Documents { get; set; }
}