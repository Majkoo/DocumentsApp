using DocumentsApp.Shared.Enums;

namespace DocumentsApp.Shared.Dtos.AccessLevel;

public class GetAccessLevelDto
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public AccessLevelEnum AccessLevelEnum { get; set; }

    public DateTime AccessLevelGranted { get; set; } = DateTime.Now;
    
    public string UserName { get; set; }

    public string UserEmail { get; set; }
}