using System.ComponentModel.DataAnnotations;
using DocumentsApp.Shared.Enums;

namespace DocumentsApp.Data.Entities;

public class EncryptionKey
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Required] public EncryptionKeyTypeEnum EncryptionKeyType { get; set; }

    [Required] public byte[] Key { get; set; }

    [Required] public byte[] Vector { get; set; }

    [Required] public DateTime DateExpired { get; set; }

    [Required] public DateTime DateCreated { get; set; } = DateTime.Now;
}