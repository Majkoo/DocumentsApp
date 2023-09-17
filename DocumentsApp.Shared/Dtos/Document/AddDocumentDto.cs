using System.Diagnostics.CodeAnalysis;

namespace DocumentsApp.Shared.Dtos.Document;

public class AddDocumentDto
{
    public string Name { get; set; }
    
    [AllowNull]
    public string Description { get; set; }
    public string Content { get; set; }
}