using DocumentsApp.Shared.Enums;

namespace DocumentsApp.Shared.Dtos.ShareDocument;

public class ShareDocumentDto
{
    public string ShareToNameOrEmail { get; set; } = null!;
    
    public AccessLevelEnum AccessLevelEnum { get; set; }
}