using AutoMapper;
using DocumentsApp.Data.Auth.Interfaces;
using DocumentsApp.Data.Entities;
using DocumentsApp.Data.Exceptions;
using DocumentsApp.Data.Repos.Interfaces;
using DocumentsApp.Data.Services;
using DocumentsApp.Shared.Dtos.DocumentDtos;
using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using Sieve.Models;
using Sieve.Services;

namespace DocumentsApp.Tests.DocumentServiceTests;

public class DocumentServiceTests
{
    [Theory]
    [ClassData(typeof(GetDocumentTestData.DocumentNoThrowTestData))]
    public async void GetDocumentById_ForExistingAndAuthorized_ReturnGetDocumentDto(string userId, Document document,
        DocumentAccessLevel accessLevel, GetDocumentDto expected)
    {
        // arrange

        // init mocks
        var autoMapperMock = GetMapperMock<GetDocumentDto, Document>(expected);
        var authProviderMock = GetAuthProviderMock(userId);
        var documentRepoMock = new Mock<IDocumentRepo>();
        var accessLevelRepoMock = new Mock<IAccessLevelRepo>();
        
        //setup mocks
        documentRepoMock.Setup(m => m.GetDocumentByIdAsync(It.IsAny<string>())).ReturnsAsync(document);
        accessLevelRepoMock.Setup(m => m.GetDocumentAccessLevelAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(accessLevel);

        //tested service
        var documentService = new DocumentService(autoMapperMock.Object, documentRepoMock.Object, null,
            accessLevelRepoMock.Object, authProviderMock.Object);
        
        //act 

        var result = await documentService.GetDocumentByIdAsync(document.Id);

        // assert

        result.Id.Should().Be(expected.Id);
    }

    [Theory]
    [ClassData(typeof(GetDocumentTestData.DocumentThrowTestData))]
    public async void GetDocumentById_ForUnauthorizedUser_ThrowNotAuthorizedException(string userId, Document document,
        DocumentAccessLevel accessLevel)
    {
        // arrange
        
        // init mocks
        var autoMapperMock = GetMapperMock<GetDocumentDto, Document>(null);
        var authProviderMock = GetAuthProviderMock(userId);
        var documentRepoMock = new Mock<IDocumentRepo>();
        var accessLevelRepoMock = new Mock<IAccessLevelRepo>();
        
        //setup mocks
        documentRepoMock.Setup(m => m.GetDocumentByIdAsync(It.IsAny<string>())).ReturnsAsync(document);
        accessLevelRepoMock.Setup(m => m.GetDocumentAccessLevelAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(accessLevel);
        
        //tested service
        var documentService = new DocumentService(autoMapperMock.Object, documentRepoMock.Object, null,
            accessLevelRepoMock.Object, authProviderMock.Object);

        //act 

        var result = async () => await documentService.GetDocumentByIdAsync(null);
        
        // assert

        await result.Should().ThrowAsync<NotAuthorizedException>();
    }
    
    [Fact]
    public async void GetDocumentById_ForNonExistingDocument_ThrowNotFoundException()
    {
        // arrange
        
        // init mocks
        var autoMapperMock = GetMapperMock<GetDocumentDto, Document>(null);
        var authProviderMock = GetAuthProviderMock(null);
        var documentRepoMock = new Mock<IDocumentRepo>();
        var accessLevelRepoMock = new Mock<IAccessLevelRepo>();
        
        //setup mocks
        documentRepoMock.Setup(m => m.GetDocumentByIdAsync(It.IsAny<string>())).ReturnsAsync(value: null);
        accessLevelRepoMock.Setup(m => m.GetDocumentAccessLevelAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(value: null);
        
        //tested service
        var documentService = new DocumentService(autoMapperMock.Object, documentRepoMock.Object, null,
            accessLevelRepoMock.Object, authProviderMock.Object);

        //act 

        var result = async () => await documentService.GetDocumentByIdAsync(null);
        
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
        var authProviderMock = GetAuthProviderMock(userId);
        var documentRepoMock = new Mock<IDocumentRepo>();
        var accessLevelRepoMock = new Mock<IAccessLevelRepo>();
        
        //setup mocks
        documentRepoMock.Setup(m => m.GetDocumentByIdAsync(It.IsAny<string>())).ReturnsAsync(document);
        documentRepoMock.Setup(m => m.UpdateDocumentAsync(It.IsAny<Document>())).ReturnsAsync(document);
        accessLevelRepoMock.Setup(m => m.GetDocumentAccessLevelAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(accessLevel);

        //tested service
        var documentService = new DocumentService(autoMapperMock.Object, documentRepoMock.Object, null,
            accessLevelRepoMock.Object, authProviderMock.Object);
        
        //act 

        var result = async () => await documentService.UpdateDocumentAsync(documentId, dto);

        // assert

        await result.Should().NotThrowAsync();
    }

    [Theory]
    [ClassData(typeof(UpdateDocumentTestData.DocumentThrowTestData))]
    public async void UpdateDocument_ForUnAuthorizedUser_ThrowNotAuthorizedException(string userId, string documentId,
        UpdateDocumentDto dto, DocumentAccessLevel accessLevel, Document document)
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
            accessLevelRepoMock.Object, authProviderMock.Object);

        //act 

        var result = async () => await documentService.UpdateDocumentAsync(documentId, dto);
        
        // assert

        await result.Should().ThrowAsync<NotAuthorizedException>();
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
            accessLevelRepoMock.Object, authProviderMock.Object);

        //act 

        var result = async () => await documentService.UpdateDocumentAsync(null, null);
        
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
        var authProviderMock = GetAuthProviderMock(userId);
        var documentRepoMock = new Mock<IDocumentRepo>();
        var accessLevelRepoMock = new Mock<IAccessLevelRepo>();
        
        //setup mocks
        documentRepoMock.Setup(m => m.GetDocumentByIdAsync(It.IsAny<string>())).ReturnsAsync(document);
        documentRepoMock.Setup(m => m.UpdateDocumentAsync(It.IsAny<Document>())).ReturnsAsync(document);
        accessLevelRepoMock.Setup(m => m.GetDocumentAccessLevelAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(accessLevel);

        //tested service
        var documentService = new DocumentService(autoMapperMock.Object, documentRepoMock.Object, null,
            accessLevelRepoMock.Object, authProviderMock.Object);
        
        //act 

        var result = async () => await documentService.DeleteDocumentAsync(documentId);

        // assert

        await result.Should().NotThrowAsync();
    }

    [Theory]
    [ClassData(typeof(DeleteDocumentTestData.DocumentThrowTestData))]
    public async void DeleteDocument_ForUnAuthorizedUser_ThrowNotAuthorizedException(string userId, string documentId,
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
            accessLevelRepoMock.Object, authProviderMock.Object);

        //act 

        var result = async () => await documentService.DeleteDocumentAsync(documentId);
        
        // assert

        await result.Should().ThrowAsync<NotAuthorizedException>();
    }
    
    [Fact]
    public async void DeleteDocument_ForNonExistingDocument_ThrowNotFoundException()
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
            accessLevelRepoMock.Object, authProviderMock.Object);

        //act 

        var result = async () => await documentService.DeleteDocumentAsync(null);
        
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
        var authProviderMock = GetAuthProviderMock(null);
        var documentRepoMock = new Mock<IDocumentRepo>();
        
        //setup mocks
        documentRepoMock.Setup(m => m.GetDocumentByIdAsync(It.IsAny<string>())).ReturnsAsync(value: null);
        autoMapperMock.Setup(m => m.Map<Document>(It.IsAny<AddDocumentDto>())).Returns(document);
        
        //tested service
        var documentService = new DocumentService(autoMapperMock.Object, documentRepoMock.Object, null,
            null, authProviderMock.Object);
        
        //act 

        var result = await documentService.AddDocumentAsync(addDocument);

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
        var authProviderMock = GetAuthProviderMock(null);
        var documentRepoMock = new Mock<IDocumentRepo>();
        var accessLevelRepoMock = new Mock<IAccessLevelRepo>();
        var sieveMock = new Mock<ISieveProcessor>();
        
        //setup mocks
        documentRepoMock.Setup(m => m.GetAllUserDocumentsAsQueryable(It.IsAny<string>())).Returns(dataMock);
        sieveMock.Setup(m => m.Apply(It.IsAny<SieveModel>(), It.IsAny<IQueryable<Document>>(), null, true, true, true))
            .Returns(dataMock);
        
        //tested service
        var documentService = new DocumentService(autoMapperMock.Object, documentRepoMock.Object, sieveMock.Object,
            accessLevelRepoMock.Object, authProviderMock.Object);
        
        // act

        var result = async () => await documentService.GetAllDocumentsAsync(new SieveModel());
        
        // assert

        await result.Should().NotThrowAsync();
    }

    [Fact]
    public async void GetAllDocuments_ForNoDocuments_ThrowNotFoundException()
    {
        // arrange

        var documents = new List<Document>().AsQueryable();
        
        // init mocks
        var autoMapperMock = GetMapperMock<GetDocumentDto, Document>(null);
        var authProviderMock = GetAuthProviderMock(null);
        var documentRepoMock = new Mock<IDocumentRepo>();
        var accessLevelRepoMock = new Mock<IAccessLevelRepo>();
        var sieveMock = new Mock<ISieveProcessor>();
        
        //setup mocks
        documentRepoMock.Setup(m => m.GetAllUserDocumentsAsQueryable(It.IsAny<string>())).Returns(documents);
        sieveMock.Setup(m => m.Apply(It.IsAny<SieveModel>(), It.IsAny<IQueryable<Document>>(), null, true, true, true))
            .Returns(value: null);
        
        //tested service
        var documentService = new DocumentService(autoMapperMock.Object, documentRepoMock.Object, sieveMock.Object,
            accessLevelRepoMock.Object, authProviderMock.Object);
        
        // act

        var result = async () => await documentService.GetAllDocumentsAsync(new SieveModel());
        
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
        mock.Setup(m => m.GetUserId()).ReturnsAsync(userId);
        return mock; 
    }

}