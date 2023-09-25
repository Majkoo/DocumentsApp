using AutoMapper;
using DocumentsApp.Api.Providers;
using DocumentsApp.Api.Services;
using DocumentsApp.Data.Entities;
using DocumentsApp.Data.Repos.Interfaces;
using DocumentsApp.Shared.Dtos.Document;
using DocumentsApp.Shared.Exceptions;
using DocumentsApp.Tests.DocumentsApp.Data.DocumentServiceTests.GetDocumentTestData;
using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using Sieve.Models;
using Sieve.Services;

namespace DocumentsApp.Tests.DocumentsApp.Data.DocumentServiceTests;

public class DocumentServiceTests
{
    [Theory]
    [ClassData(typeof(DocumentNoThrowTestData))]
    public async void GetDocumentById_ForExistingAndAuthorized_ReturnGetDocumentDto(string userId, Document document,
        DocumentAccessLevel accessLevel, GetDocumentDto expected)
    {
        // arrange

        // init mocks
        var autoMapperMock = GetMapperMock<GetDocumentDto, Document>(expected);
        var documentRepoMock = new Mock<IDocumentRepo>();
        var accessLevelRepoMock = new Mock<IAccessLevelRepo>();
        
        //setup mocks
        documentRepoMock.Setup(m => m.GetDocumentByIdAsync(It.IsAny<string>())).ReturnsAsync(document);
        accessLevelRepoMock.Setup(m => m.GetDocumentAccessLevelAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(accessLevel);

        //tested service
        var documentService = new DocumentService(autoMapperMock.Object, documentRepoMock.Object, null,
            accessLevelRepoMock.Object);
        
        //act 

        var result = await documentService.GetDocumentByIdAsync(userId, document.Id);

        // assert

        result.Id.Should().Be(expected.Id);
    }

    [Theory]
    [ClassData(typeof(DocumentThrowTestData))]
    public async void GetDocumentById_ForUnauthorizedUser_ThrowUnauthorizedException(string userId, Document document,
        DocumentAccessLevel accessLevel)
    {
        // arrange
        
        // init mocks
        var autoMapperMock = GetMapperMock<GetDocumentDto, Document>(null);
        var documentRepoMock = new Mock<IDocumentRepo>();
        var accessLevelRepoMock = new Mock<IAccessLevelRepo>();
        
        //setup mocks
        documentRepoMock.Setup(m => m.GetDocumentByIdAsync(It.IsAny<string>())).ReturnsAsync(document);
        accessLevelRepoMock.Setup(m => m.GetDocumentAccessLevelAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(accessLevel);
        
        //tested service
        var documentService = new DocumentService(autoMapperMock.Object, documentRepoMock.Object, null,
            accessLevelRepoMock.Object);

        //act 

        var result = async () => await documentService.GetDocumentByIdAsync(userId, null);
        
        // assert

        await result.Should().ThrowAsync<UnauthorizedException>();
    }
    
    [Fact]
    public async void GetDocumentById_ForNonExistingDocument_ThrowNotFoundException()
    {
        // arrange
        
        // init mocks
        var autoMapperMock = GetMapperMock<GetDocumentDto, Document>(null);
        var documentRepoMock = new Mock<IDocumentRepo>();
        var accessLevelRepoMock = new Mock<IAccessLevelRepo>();
        
        //setup mocks
        documentRepoMock.Setup(m => m.GetDocumentByIdAsync(It.IsAny<string>())).ReturnsAsync(value: null);
        accessLevelRepoMock.Setup(m => m.GetDocumentAccessLevelAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(value: null);
        
        //tested service
        var documentService = new DocumentService(autoMapperMock.Object, documentRepoMock.Object, null,
            accessLevelRepoMock.Object);

        //act 

        var result = async () => await documentService.GetDocumentByIdAsync(null, null);
        
        // assert

        await result.Should().ThrowAsync<NotFoundException>();
    }
    

    [Theory]
    [ClassData(typeof(UpdateDocumentTestData.DocumentNoThrowTestData))]
    public async void UpdateDocument_ForExistingAndAuthorized_NoExceptionThrown(string userId, string documentId,
        UpdateDocumentDto dto, DocumentAccessLevel accessLevel, Document document)
    {
        // arrange

        // init mocks
        var autoMapperMock = GetMapperMock<Document, UpdateDocumentDto>(document);
        var documentRepoMock = new Mock<IDocumentRepo>();
        var accessLevelRepoMock = new Mock<IAccessLevelRepo>();
        
        //setup mocks
        documentRepoMock.Setup(m => m.GetDocumentByIdAsync(It.IsAny<string>())).ReturnsAsync(document);
        documentRepoMock.Setup(m => m.UpdateDocumentAsync(It.IsAny<Document>())).ReturnsAsync(document);
        accessLevelRepoMock.Setup(m => m.GetDocumentAccessLevelAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(accessLevel);

        //tested service
        var documentService = new DocumentService(autoMapperMock.Object, documentRepoMock.Object, null,
            accessLevelRepoMock.Object);
        
        //act 

        var result = async () => await documentService.UpdateDocumentAsync(userId, documentId, dto);

        // assert

        await result.Should().NotThrowAsync();
    }

    [Theory]
    [ClassData(typeof(UpdateDocumentTestData.DocumentThrowTestData))]
    public async void UpdateDocument_ForUnAuthorizedUser_ThrowUnauthorizedException(string userId, string documentId,
        UpdateDocumentDto dto, DocumentAccessLevel accessLevel, Document document)
    {
        // arrange
        
        // init mocks
        var autoMapperMock = GetMapperMock<Document, UpdateDocumentDto>(document);
        var documentRepoMock = new Mock<IDocumentRepo>();
        var accessLevelRepoMock = new Mock<IAccessLevelRepo>();
        
        //setup mocks
        documentRepoMock.Setup(m => m.GetDocumentByIdAsync(It.IsAny<string>())).ReturnsAsync(document);
        accessLevelRepoMock.Setup(m => m.GetDocumentAccessLevelAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(accessLevel);
        
        //tested service
        var documentService = new DocumentService(autoMapperMock.Object, documentRepoMock.Object, null,
            accessLevelRepoMock.Object);

        //act 

        var result = async () => await documentService.UpdateDocumentAsync(userId, documentId, dto);
        
        // assert

        await result.Should().ThrowAsync<UnauthorizedException>();
    }
    
    [Fact]
    public async void UpdateDocument_ForNonExistingDocument_ThrowNotFoundException()
    {
        // arrange
        
        // init mocks
        var autoMapperMock = GetMapperMock<GetDocumentDto, Document>(null);
        var authProviderMock = GetAuthProviderMock(null);
        var documentRepoMock = new Mock<IDocumentRepo>();
        var accessLevelRepoMock = new Mock<IAccessLevelRepo>();
        
        //setup mocks
        documentRepoMock.Setup(m => m.GetDocumentByIdAsync(It.IsAny<string>())).ReturnsAsync(value: null);
        documentRepoMock.Setup(m => m.UpdateDocumentAsync(It.IsAny<Document>())).ReturnsAsync(value: null);
        accessLevelRepoMock.Setup(m => m.GetDocumentAccessLevelAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(value: null);
        
        //tested service
        var documentService = new DocumentService(autoMapperMock.Object, documentRepoMock.Object, null,
            accessLevelRepoMock.Object);

        //act 

        var result = async () => await documentService.UpdateDocumentAsync(null, null, null);
        
        // assert

        await result.Should().ThrowAsync<NotFoundException>();
    }

    [Theory]
    [ClassData(typeof(DeleteDocumentTestData.DocumentNoThrowTestData))]
    public async void DeleteDocument_ForExistingAndAuthorized_NoExceptionThrown(string userId, string documentId,
        Document document, DocumentAccessLevel accessLevel)
    {
        // arrange

        // init mocks
        var autoMapperMock = GetMapperMock<Document, UpdateDocumentDto>(document);
        var documentRepoMock = new Mock<IDocumentRepo>();
        var accessLevelRepoMock = new Mock<IAccessLevelRepo>();
        
        //setup mocks
        documentRepoMock.Setup(m => m.GetDocumentByIdAsync(It.IsAny<string>())).ReturnsAsync(document);
        documentRepoMock.Setup(m => m.UpdateDocumentAsync(It.IsAny<Document>())).ReturnsAsync(document);
        accessLevelRepoMock.Setup(m => m.GetDocumentAccessLevelAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(accessLevel);

        //tested service
        var documentService = new DocumentService(autoMapperMock.Object, documentRepoMock.Object, null,
            accessLevelRepoMock.Object);
        
        //act 

        var result = async () => await documentService.DeleteDocumentAsync(userId, documentId);

        // assert

        await result.Should().NotThrowAsync();
    }

    [Theory]
    [ClassData(typeof(DeleteDocumentTestData.DocumentThrowTestData))]
    public async void DeleteDocument_ForUnAuthorizedUser_ThrowUnauthorizedException(string userId, string documentId,
        Document document, DocumentAccessLevel accessLevel)
    {
        // arrange
        
        // init mocks
        var autoMapperMock = GetMapperMock<Document, UpdateDocumentDto>(document);
        var authProviderMock = GetAuthProviderMock(userId);
        var documentRepoMock = new Mock<IDocumentRepo>();
        var accessLevelRepoMock = new Mock<IAccessLevelRepo>();
        
        //setup mocks
        documentRepoMock.Setup(m => m.GetDocumentByIdAsync(It.IsAny<string>())).ReturnsAsync(document);
        accessLevelRepoMock.Setup(m => m.GetDocumentAccessLevelAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(accessLevel);
        
        //tested service
        var documentService = new DocumentService(autoMapperMock.Object, documentRepoMock.Object, null,
            accessLevelRepoMock.Object);

        //act 

        var result = async () => await documentService.DeleteDocumentAsync(null, documentId);
        
        // assert

        await result.Should().ThrowAsync<UnauthorizedException>();
    }
    
    [Fact]
    public async void DeleteDocument_ForNonExistingDocument_ThrowNotFoundException()
    {
        // arrange
        
        // init mocks
        var autoMapperMock = GetMapperMock<GetDocumentDto, Document>(null);
        var documentRepoMock = new Mock<IDocumentRepo>();
        var accessLevelRepoMock = new Mock<IAccessLevelRepo>();
        
        //setup mocks
        documentRepoMock.Setup(m => m.GetDocumentByIdAsync(It.IsAny<string>())).ReturnsAsync(value: null);
        documentRepoMock.Setup(m => m.UpdateDocumentAsync(It.IsAny<Document>())).ReturnsAsync(value: null);
        accessLevelRepoMock.Setup(m => m.GetDocumentAccessLevelAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(value: null);
        
        //tested service
        var documentService = new DocumentService(autoMapperMock.Object, documentRepoMock.Object, null,
            accessLevelRepoMock.Object);

        //act 

        var result = async () => await documentService.DeleteDocumentAsync(null, null);
        
        // assert

        await result.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async void AddDocument_ForSuccessfulAdd_ReturnGetDocumentDto()
    {
        // arrange

        var addDocument = new AddDocumentDto()
        {
            Name = "name",
            Description = "description",
            Content = "content"
        };

        var document = new Document();

        var expected = new GetDocumentDto()
        {
            Name = "name",
            Description = "description",
            Content = "content"
        };

        // init mocks
        var autoMapperMock = GetMapperMock<GetDocumentDto, Document>(expected);
        var documentRepoMock = new Mock<IDocumentRepo>();
        
        //setup mocks
        documentRepoMock.Setup(m => m.GetDocumentByIdAsync(It.IsAny<string>())).ReturnsAsync(value: null);
        autoMapperMock.Setup(m => m.Map<Document>(It.IsAny<AddDocumentDto>())).Returns(document);
        
        //tested service
        var documentService = new DocumentService(autoMapperMock.Object, documentRepoMock.Object, null,
            null);
        
        //act 

        var result = await documentService.AddDocumentAsync(null, addDocument);

        // assert

        result.Name.Should().Be(expected.Name);
    }
    
    [Fact]
    public async void GetAllDocuments_ForAnyDocuments_NoExceptionThrown()
    {
        // arrange

        var documents = new List<Document>()
        {
            new Document()
        };

        // MockQueryable extension 
        var dataMock = documents.BuildMock();
        
        // init mocks
        var autoMapperMock = GetMapperMock<GetDocumentDto, Document>(new GetDocumentDto());
        var documentRepoMock = new Mock<IDocumentRepo>();
        var accessLevelRepoMock = new Mock<IAccessLevelRepo>();
        var sieveMock = new Mock<ISieveProcessor>();
        
        //setup mocks
        documentRepoMock.Setup(m => m.GetAllUserDocumentsAsQueryable(It.IsAny<string>())).Returns(dataMock);
        sieveMock.Setup(m => m.Apply(It.IsAny<SieveModel>(), It.IsAny<IQueryable<Document>>(), null, true, true, true))
            .Returns(dataMock);
        
        //tested service
        var documentService = new DocumentService(autoMapperMock.Object, documentRepoMock.Object, sieveMock.Object,
            accessLevelRepoMock.Object);
        
        // act

        var result = async () => await documentService.GetAllUserDocumentsAsync(null, new SieveModel());
        
        // assert

        await result.Should().NotThrowAsync();
    }

    [Fact]
    public async void GetAllDocuments_ForNoDocuments_ThrowNotFoundException()
    {
        // arrange

        var documents = new List<Document>();
        
        // MockQueryable extension 
        var dataMock = documents.BuildMock();
        
        // init mocks
        var autoMapperMock = GetMapperMock<GetDocumentDto, Document>(null);
        var documentRepoMock = new Mock<IDocumentRepo>();
        var accessLevelRepoMock = new Mock<IAccessLevelRepo>();
        var sieveMock = new Mock<ISieveProcessor>();
        
        //setup mocks
        documentRepoMock.Setup(m => m.GetAllUserDocumentsAsQueryable(It.IsAny<string>())).Returns(dataMock);
        sieveMock.Setup(m => m.Apply(It.IsAny<SieveModel>(), It.IsAny<IQueryable<Document>>(), null, true, true, true))
            .Returns(dataMock);
        
        //tested service
        var documentService = new DocumentService(autoMapperMock.Object, documentRepoMock.Object, sieveMock.Object,
            accessLevelRepoMock.Object);
        
        // act

        var result = async () => await documentService.GetAllUserDocumentsAsync(null, new SieveModel());
        
        // assert

        await result.Should().ThrowAsync<NotFoundException>();
    }
    
    
    
    private Mock<IMapper> GetMapperMock<TA,TB>(TA returnValue)
    {
        var mock = new Mock<IMapper>();
        mock.Setup(m => m.Map<TA>(It.IsAny<TB>())).Returns(returnValue);
        return mock; 
    }

    private Mock<IAuthenticationContextProvider> GetAuthProviderMock(string userId)
    { 
        var mock = new Mock<IAuthenticationContextProvider>();
        mock.Setup(m => m.GetUserId()).Returns(userId);
        return mock; 
    }

}