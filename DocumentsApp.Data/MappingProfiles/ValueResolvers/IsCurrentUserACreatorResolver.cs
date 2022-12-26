using AutoMapper;
using DocumentsApp.Data.Auth.Interfaces;
using DocumentsApp.Data.Entities;
using DocumentsApp.Shared.Dtos.DocumentDtos;

namespace DocumentsApp.Data.MappingProfiles.ValueResolvers;

public class IsCurrentUserACreatorResolver : IValueResolver<Document, GetDocumentDto, bool>
{
    private readonly string _userId;

    public IsCurrentUserACreatorResolver(IAuthenticationContextProvider contextProvider)
    {
        _userId = contextProvider.GetUserId().Result;
    }
    
    public bool Resolve(Document source, GetDocumentDto destination, bool destMember, ResolutionContext context)
    {
        return source.Id == _userId;
    }
}