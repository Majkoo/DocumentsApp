using AutoMapper;
using DocumentsApp.Api.Providers;
using DocumentsApp.Data.Entities;
using DocumentsApp.Shared.Dtos.Document;
using DocumentsApp.Shared.Enums;

namespace DocumentsApp.Api.MappingProfiles.ValueResolvers;

public class AccessLevelResolver : IValueResolver<Document, GetDocumentDto, AccessLevelEnum?>
{
    private readonly string _userId;

    public AccessLevelResolver(IAuthenticationContextProvider contextProvider)
    {
        _userId = contextProvider.GetUserId();
    }
    
    public AccessLevelEnum? Resolve(Document source, GetDocumentDto destination, AccessLevelEnum? destMember,
        ResolutionContext context)
    {
        if (source.AccountId == _userId)
            return AccessLevelEnum.Write;
        
        var accessLevel = source.AccessLevels?.SingleOrDefault(a => a.AccountId == _userId && a.DocumentId == source.Id);
        return accessLevel?.AccessLevelEnum;
    }
    
}