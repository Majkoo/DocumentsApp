using AutoMapper;
using DocumentsApp.Api.MappingProfiles;
using DocumentsApp.Api.MappingProfiles.ValueResolvers;
using DocumentsApp.Api.Providers;
using DocumentsApp.Data.Entities;
using DocumentsApp.Shared.Dtos.Document;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace DocumentsApp.Tests.DocumentsApp.Data.AutoMapperTests;

public class AutoMapperTests
{
    [Theory]
    [ClassData(typeof(GetDocumentTestData))]
    public void Map_ForDocument_ReturnCorrectGetDocumentDto(string userId, Document document, GetDocumentDto expected)
    {
        // arrange
        
        IServiceCollection services = new ServiceCollection();
        
        //moq
        var authProviderMock = new Mock<IAuthenticationContextProvider>();
        authProviderMock.Setup(m => m.GetUserId()).Returns(userId);
        
        //add services and resolvers
        services.AddSingleton(authProviderMock.Object);
        services.AddScoped<AccessLevelResolver>();
        services.AddScoped<IsCurrentUserACreatorResolver>();
        services.AddScoped<IsModifiableResolver>();
        
        //automapper
        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<DtoMappingProfile>();
        });
        
        //build services
        IServiceProvider serviceProvider = services.BuildServiceProvider();
        
        //get automapper
        var mapper = serviceProvider.GetService<IMapper>();
        
        //act 
        
        var result = mapper.Map<GetDocumentDto>(document);
        
        // assert
        
        result.isModifiable.Should().Be(expected.isModifiable);
        result.isCurrentUserACreator.Should().Be(expected.isCurrentUserACreator);
        result.AccessLevel.Should().Be(expected.AccessLevel);
    }

    [Theory]
    [ClassData(typeof(UpdateDocumentTestData))]
    public void Map_ForUpdateDocument_IgnoreNullValues(Document document, UpdateDocumentDto changes, Document expected)
    {
        //arrange
        
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<DtoMappingProfile>();
        });
        
        var mapper = config.CreateMapper();
        
        // act
        
        var result = mapper.Map(changes, document);
            
        // assert
        
        result.Name.Should().Be(expected.Name);
        result.Description.Should().Be(expected.Description);
        result.Content.Should().Be(expected.Content);
    }

}