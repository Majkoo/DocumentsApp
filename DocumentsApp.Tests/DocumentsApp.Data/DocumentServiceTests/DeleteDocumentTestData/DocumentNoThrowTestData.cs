using System.Collections;
using DocumentsApp.Data.Entities;
using DocumentsApp.Shared.Enums;

namespace DocumentsApp.Tests.DocumentServiceTests.DeleteDocumentTestData;

public class DocumentNoThrowTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[]
        {
            "creatorId",
            "documentId",
            new Document()
            {
                Id = "documentId",
                AccountId = "creatorId"
            },
            null
        };
        
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}