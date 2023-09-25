using DocumentsApp.Shared.Enums;

namespace DocumentsApp.Shared.Dtos.AccessLevel;

public class GetAccessLevelDto
{
    public string Id { get; set; } = null!;
    public AccessLevelEnum AccessLevelEnum { get; set; }
    public DateTime AccessLevelGranted { get; set; }
    public string UserName { get; set; } = null!;
    public string UserEmail { get; set; } = null!;
}