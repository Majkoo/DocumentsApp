namespace DocumentsApp.Shared.Dtos.Document;

public class UpdateDocumentDto
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Content { get; set; } = null!;
}