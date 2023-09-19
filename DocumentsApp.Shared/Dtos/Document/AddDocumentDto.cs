using System.Diagnostics.CodeAnalysis;

namespace DocumentsApp.Shared.Dtos.Document;

public class AddDocumentDto
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Content { get; set; } = null!;
}