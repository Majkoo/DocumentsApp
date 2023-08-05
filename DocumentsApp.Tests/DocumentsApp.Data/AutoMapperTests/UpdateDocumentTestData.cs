using System.Collections;
using DocumentsApp.Data.Entities;
using DocumentsApp.Shared.Dtos.DocumentDtos;

namespace DocumentsApp.Tests.DocumentsApp.Data.AutoMapperTests;

public class UpdateDocumentTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[]
        {
            new Document()
            {
                Name = "Document Name",
                Description = "Document Description",
                Content = "Document Content"
            },
            new UpdateDocumentDto()
            {
                Name = "New Name",
                Description = "New Description",
                Content = "New Content"
            },
            new Document()
            {
                Name = "New Name",
                Description = "New Description",
                Content = "New Content"
            }
        };
        
        yield return new object[]
        {
            new Document()
            {
                Name = "Document Name",
                Description = "Document Description",
                Content = "Document Content"
            },
            new UpdateDocumentDto()
            {
                Name = "New Name",
                Description = null,
                Content = null
            },
            new Document()
            {
                Name = "New Name",
                Description = "Document Description",
                Content = "Document Content"
            }
        };
        
        yield return new object[]
        {
            new Document()
            {
                Name = "Document Name",
                Description = null,
                Content = "Document Content"
            },
            new UpdateDocumentDto()
            {
                Name = null,
                Description = "New Description",
                Content = null
            },
            new Document()
            {
                Name = "Document Name",
                Description = "New Description",
                Content = "Document Content"
            }
        };
        
        yield return new object[]
        {
            new Document()
            {
                Name = "Document Name",
                Description = "Document Description",
                Content = "Document Content"
            },
            new UpdateDocumentDto()
            {
                Name = null,
                Description = null,
                Content = null
            },
            new Document()
            {
                Name = "Document Name",
                Description = "Document Description",
                Content = "Document Content"
            }
        };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}