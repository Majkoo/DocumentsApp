using DocumentsApp.Data.Entities;
using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services;

namespace DocumentsApp.Api.Helpers;

public class DocumentsAppSieveProcessor : SieveProcessor
{
    public DocumentsAppSieveProcessor(IOptions<SieveOptions> options) : base(options){}

    protected override SievePropertyMapper MapProperties(SievePropertyMapper mapper)
    {
        mapper.Property<Document>(d => d.Name)
            .CanFilter()
            .CanSort();

        mapper.Property<Document>(d => d.Description)
            .CanFilter();

        mapper.Property<Document>(d => d.Content)
            .CanFilter();
        
        mapper.Property<Document>(d => d.DateCreated)
            .CanFilter()
            .CanSort();

        mapper.Property<Document>(d => d.Account.UserName)
            .CanFilter()
            .CanSort();

        return mapper;
    }
}