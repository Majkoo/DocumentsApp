using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace DocumentsApp.Data.Entities;

public class Document
{
    public Guid Id { get; set; }

    [Required]
    [MaxLength(20)]
    public string Name { get; set; }

    [AllowNull]
    [MaxLength(150)]
    public string Description { get; set; }
    
    [Required]
    [MaxLength(5000)]
    public string Content { get; set; }

    // ogolem w bazach danych obowiazuje konwencja ze nazywamy pola wiążące relacją tak jak inna tabelka w miare mozliwosci,
    // np tutaj byloby "UserId"
    public Guid CreatorId { get; set; }


    // a tutaj "User"
    public virtual User Creator { get; set; }
}