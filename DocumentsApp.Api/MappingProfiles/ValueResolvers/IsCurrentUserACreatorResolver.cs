using AutoMapper;
using DocumentsApp.Api.Providers;
using DocumentsApp.Data.Entities;
using DocumentsApp.Shared.Dtos.Document;

namespace DocumentsApp.Api.MappingProfiles.ValueResolvers;

public class IsCurrentUserACreatorResolver : IValueResolver<Document, GetDocumentDto, bool>
{
    private readonly string _userId;

    public IsCurrentUserACreatorResolver(IAuthenticationContextProvider contextProvider)
    {
        _userId = contextProvider.GetUserId();
    }
    
    public bool Resolve(Document source, GetDocumentDto destination, bool destMember, ResolutionContext context)
    {
        return source.AccountId == _userId;
    }
}