using System.Diagnostics.CodeAnalysis;
using DocumentsApp.Shared.Enums;

namespace DocumentsApp.Shared.Dtos.Document;

public class GetDocumentDto
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Content { get; set; } = null!;

    public DateTime DateCreated { get; set; }

    public string AccountName { get; set; } = null!;

    public AccessLevelEnum? AccessLevel { get; set; }

    public bool isCurrentUserACreator { get; set; } 
    public bool isModifiable { get; set; }
}