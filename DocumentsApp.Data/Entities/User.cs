using System.ComponentModel.DataAnnotations;

namespace DocumentsApp.Data.Entities;

public class User
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(20)]
    public string UserName { get; set; }
    
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    public string PasswordHash { get; set; }
}