using DocumentsApp.Shared.Enums;

namespace DocumentsApp.Shared.Dtos.ShareDocument;

public class ShareDocumentDto
{
    public string SharedToUserName { get; set; }
    
    public AccessLevelEnum AccessLevelEnum { get; set; }
}