using AutoMapper;
using DocumentsApp.Api.Providers;
using DocumentsApp.Data.Entities;
using DocumentsApp.Shared.Dtos.Document;
using DocumentsApp.Shared.Enums;

namespace DocumentsApp.Api.MappingProfiles.ValueResolvers;

public class IsModifiableResolver : IValueResolver<Document, GetDocumentDto, bool>
{
    private readonly string _userId;

    public IsModifiableResolver(IAuthenticationContextProvider contextProvider)
    {
        _userId = contextProvider.GetUserId();
    }
    public bool Resolve(Document source, GetDocumentDto destination, bool destMember, ResolutionContext context)
    {
        if (source.AccountId == _userId)
            return true;
        
        var accessLevel = source.AccessLevels?.SingleOrDefault(a => a.AccountId == _userId && a.DocumentId == source.Id);
        return accessLevel?.AccessLevelEnum == AccessLevelEnum.Write;
    }
}