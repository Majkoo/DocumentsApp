using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace DocumentsApp.Data.Entities;

public class Account : IdentityUser
{
    public ICollection<Document> Documents { get; set; }
}