using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity;

namespace DocumentsApp.Data.Entities;

public class Account : IdentityUser
{
    [AllowNull]
    public string RefreshToken { get; set; }
    [AllowNull]
    public DateTime RefreshTokenExp { get; set; }
    public ICollection<Document> Documents { get; set; }
}