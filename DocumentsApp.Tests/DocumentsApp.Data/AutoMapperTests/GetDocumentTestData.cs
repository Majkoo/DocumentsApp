using System.Collections;
using DocumentsApp.Data.Entities;
using DocumentsApp.Shared.Dtos.Document;
using DocumentsApp.Shared.Enums;

namespace DocumentsApp.Tests.DocumentsApp.Data.AutoMapperTests;

public class GetDocumentTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        // creator
        yield return new object[]
        {
            "creatorId",
            new Document
            {
                Id = "documentId",
                Name = "DocumentName",
                Description = null,
                Content = "Document Content",
                AccountId = "creatorId"
            },
            new GetDocumentDto
            {
                isCurrentUserACreator = true,
                isModifiable = true,
                AccessLevel = AccessLevelEnum.Write
            }
        };
        
        // writer
        yield return new object[]
        {
            "someUserId",
            new Document
            {
                Id = "documentId",
                Name = "DocumentName",
                Description = null,
                Content = "Document Content",
                AccountId = "creatorId",
                AccessLevels = new List<DocumentAccessLevel>
                {
                    new()
                    {
                        AccountId = "someUserId",
                        DocumentId = "documentId",
                        AccessLevelEnum = AccessLevelEnum.Write 
                    }
                }
            },
            new GetDocumentDto
            {
                isCurrentUserACreator = false,
                isModifiable = true,
                AccessLevel = AccessLevelEnum.Write
            }
        };
            
        // reader
        yield return new object[]
        {
            "someUserId",
            new Document
            {
                Id = "documentId",
                Name = "DocumentName",
                Description = null,
                Content = "Document Content",
                AccountId = "creatorId",
                AccessLevels = new List<DocumentAccessLevel>
                {
                    new()
                    {
                        AccountId = "someUserId",
                        DocumentId = "documentId",
                        AccessLevelEnum = AccessLevelEnum.Read 
                    }
                }
            },
            new GetDocumentDto
            {
                isCurrentUserACreator = false,
                isModifiable = false,
                AccessLevel = AccessLevelEnum.Read
            }
        };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}