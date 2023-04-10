using System.Collections;
using DocumentsApp.Data.Entities;
using DocumentsApp.Shared.Enums;

namespace DocumentsApp.Tests.DocumentServiceTests.DeleteDocumentTestData;

public class DocumentThrowTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[]
        {
            "someUserId",
            "documentId",
            new Document()
            {
                Id = "documentId",
                AccountId = "creatorId"
            },
            null
        };
        
        yield return new object[]
        {
            "someUserId",
            "documentId",
            new Document()
            {
                Id = "documentId",
                AccountId = "creatorId"
            },
            new DocumentAccessLevel()
            {
                AccountId = "someUserId",
                DocumentId = "documentId",
                AccessLevelEnum = AccessLevelEnum.Write
            }
        };
        
        yield return new object[]
        {
            "someUserId",
            "documentId",
            new Document()
            {
                Id = "documentId",
                AccountId = "creatorId"
            },
            new DocumentAccessLevel()
            {
                AccountId = "someUserId",
                DocumentId = "documentId",
                AccessLevelEnum = AccessLevelEnum.Read
            }
        };
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}