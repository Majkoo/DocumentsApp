using DocumentsApp.Shared.Enums;

namespace DocumentsApp.Data.Dtos.ShareDocumentDtos;

public class ShareDocumentDto
{
    public string SharedToUserName { get; set; }
    
    public AccessLevelEnum AccessLevelEnum { get; set; }
}