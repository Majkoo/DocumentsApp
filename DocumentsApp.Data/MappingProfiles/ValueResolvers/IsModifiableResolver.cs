using AutoMapper;
using DocumentsApp.Data.Auth.Interfaces;
using DocumentsApp.Data.Entities;
using DocumentsApp.Shared.Dtos.DocumentDtos;
using DocumentsApp.Shared.Enums;

namespace DocumentsApp.Data.MappingProfiles.ValueResolvers;

public class IsModifiableResolver : IValueResolver<Document, GetDocumentDto, bool>
{
    private readonly string _userId;

    public IsModifiableResolver(IAuthenticationContextProvider contextProvider)
    {
        _userId = contextProvider.GetUserId().Result;
    }
    public bool Resolve(Document source, GetDocumentDto destination, bool destMember, ResolutionContext context)
    {
        var accessLevel = source.AccessLevels?.SingleOrDefault(a => a.AccountId == _userId && a.DocumentId == source.Id);
        return accessLevel?.AccessLevelEnum == AccessLevelEnum.Write;
    }
}