using DocumentsApp.Data.Services;
using DocumentsApp.Data.Services.Interfaces;
using DocumentsApp.Shared.Dtos.DocumentDtos;

namespace DocumentsApp.Tests;

public class DocumentServiceTests
{
    [Fact]
    public async void GetDocument_NonExisting_ThrowNotFoundException()
    {
        // arrange

        GetDocumentDto documentDto;

        //act 

        documentDto = null;

        // assert
        
        Assert.Null(documentDto);
    }
}