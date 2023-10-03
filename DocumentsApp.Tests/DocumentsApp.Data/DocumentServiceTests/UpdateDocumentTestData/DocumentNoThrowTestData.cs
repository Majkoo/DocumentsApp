using System.Collections;
using DocumentsApp.Data.Entities;
using DocumentsApp.Shared.Dtos.Document;
using DocumentsApp.Shared.Enums;

namespace DocumentsApp.Tests.DocumentsApp.Data.DocumentServiceTests.UpdateDocumentTestData;

public class DocumentNoThrowTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[]
        {
            "creatorId",
            "documentId",
            new UpdateDocumentDto(),
            null,
            new Document()
            {
                Id = "documentId",
                AccountId = "creatorId"
            }
        };

        yield return new object[]
        {
            "someUserId",
            "documentId",
            new UpdateDocumentDto(),
            new DocumentAccessLevel()
            {
                AccountId = "someUserId",
                DocumentId = "documentId",
                AccessLevelEnum = AccessLevelEnum.Write
            },
            new Document()
            {
                Id = "documentId",
                AccountId = "creatorId"
            }
        };
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}