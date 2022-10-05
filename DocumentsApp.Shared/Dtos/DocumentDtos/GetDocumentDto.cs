﻿namespace DocumentsApp.Data.Dtos.DocumentDtos;

public class GetDocumentDto
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public string Content { get; set; }

    public DateTime DateCreated { get; set; }
}