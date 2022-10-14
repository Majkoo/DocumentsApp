using System.Diagnostics.CodeAnalysis;

namespace DocumentsApp.Shared.Dtos.DocumentDtos;

public class GetDocumentDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    
    [AllowNull]
    public string Description { get; set; }
    public string Content { get; set; }

    public DateTime DateCreated { get; set; }

    public string AccountName { get; set; }
}