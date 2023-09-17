using System.Diagnostics.CodeAnalysis;
using DocumentsApp.Shared.Enums;

namespace DocumentsApp.Shared.Dtos.Document;

public class GetDocumentDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    
    [AllowNull]
    public string Description { get; set; }
    public string Content { get; set; }

    public DateTime DateCreated { get; set; }

    public string AccountName { get; set; }

    public AccessLevelEnum? AccessLevel { get; set; }

    public bool isCurrentUserACreator { get; set; }
    public bool isModifiable { get; set; }
}