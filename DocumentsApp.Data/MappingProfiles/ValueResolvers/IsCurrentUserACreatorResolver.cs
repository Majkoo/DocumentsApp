using AutoMapper;
using DocumentsApp.Data.Auth.Interfaces;
using DocumentsApp.Data.Entities;
using DocumentsApp.Shared.Dtos.DocumentDtos;

namespace DocumentsApp.Data.MappingProfiles.ValueResolvers;

public class IsCurrentUserACreatorResolver : IValueResolver<Document, GetDocumentDto, bool>
{
    private readonly IAuthenticationContextProvider _contextProvider;

    public IsCurrentUserACreatorResolver(IAuthenticationContextProvider contextProvider)
    {
        _contextProvider = contextProvider;
    }
    public bool Resolve(Document source, GetDocumentDto destination, bool destMember, ResolutionContext context)
    {
        var userId = _contextProvider.GetUserId().Result;
        if (source.AccountId == userId)
        {
            return true;
        }

        return false;
    }
}