using System.Collections;
using DocumentsApp.Data.Entities;

namespace DocumentsApp.Tests.DocumentsApp.Data.DocumentServiceTests.GetDocumentTestData;

public class DocumentThrowTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[]
        {
            "userId",
            new Document
            {
                Id = "documentId",
                Name = "Document Name",
                Description = null,
                Content = "Document Content",
                AccountId = "creatorId"
            },
            null
        };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}