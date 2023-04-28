using System.Collections;
using DocumentsApp.Data.Entities;
using DocumentsApp.Shared.Dtos.DocumentDtos;
using DocumentsApp.Shared.Enums;

namespace DocumentsApp.Tests.DocumentServiceTests.GetDocumentTestData;

public class DocumentNoThrowTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[]
        {
            "creatorId",
            new Document()
            {
                Id = "DocumentId",
                AccountId = "creatorId" 
            },
            null,
            new GetDocumentDto()
            {
                Id = "DocumentId"
            }
        };

        yield return new object[]
        {
            "userId",
            new Document()
            {
                Id = "documentId",
                AccountId = "creatorId"
            },
            new DocumentAccessLevel()
            {
                AccountId = "userId",
                DocumentId = "DocumentId",
                AccessLevelEnum = AccessLevelEnum.Write
            },
            new GetDocumentDto()
            {
                Id = "DocumentId"
            }
        };
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}