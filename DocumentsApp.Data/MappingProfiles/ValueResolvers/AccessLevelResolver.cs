using AutoMapper;
using DocumentsApp.Data.Auth.Interfaces;
using DocumentsApp.Data.Entities;
using DocumentsApp.Shared.Dtos.DocumentDtos;
using DocumentsApp.Shared.Enums;

namespace DocumentsApp.Data.MappingProfiles.ValueResolvers;

public class AccessLevelResolver : IValueResolver<Document, GetDocumentDto, AccessLevelEnum?>
{
    private readonly IAuthenticationContextProvider _contextProvider;

    public AccessLevelResolver(IAuthenticationContextProvider contextProvider)
    {
        _contextProvider = contextProvider;
    }
    
    public AccessLevelEnum? Resolve(Document source, GetDocumentDto destination, AccessLevelEnum? destMember,
        ResolutionContext context)
    {
        var userId = _contextProvider.GetUserId().Result;
        var accessLevel = source.AccessLevels.SingleOrDefault(a => a.AccountId == userId && a.DocumentId == source.Id);
        return accessLevel?.AccessLevelEnum;
    }
    
}