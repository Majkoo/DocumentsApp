using AutoMapper;
using DocumentsApp.Data.Auth.Interfaces;
using DocumentsApp.Data.Entities;
using DocumentsApp.Data.Repos.Interfaces;
using DocumentsApp.Shared.Dtos.DocumentDtos;
using DocumentsApp.Shared.Enums;

namespace DocumentsApp.Data.MappingProfiles.ValueResolvers;

public class IsModifiableResolver : IValueResolver<Document, GetDocumentDto, bool>
{
    private readonly IAuthenticationContextProvider _contextProvider;

    public IsModifiableResolver(IAuthenticationContextProvider contextProvider, IAccessLevelRepo accessLevelRepo)
    {
        _contextProvider = contextProvider;
    }
    public bool Resolve(Document source, GetDocumentDto destination, bool destMember, ResolutionContext context)
    {
        var userId = _contextProvider.GetUserId().Result;
        var accessLevel = source.AccessLevels.SingleOrDefault(a => a.AccountId == userId && a.DocumentId == source.Id);
        
        return accessLevel?.AccessLevelEnum == AccessLevelEnum.Write;
    }
}