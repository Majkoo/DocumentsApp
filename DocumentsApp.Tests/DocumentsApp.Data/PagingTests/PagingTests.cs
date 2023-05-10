using DocumentsApp.Data.Dtos;
using DocumentsApp.Data.Entities;
using DocumentsApp.Data.Sieve;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Sieve.Models;

namespace DocumentsApp.Tests.PagingTests;

public class PagingTests
{
    [Theory]
    [ClassData(typeof(PagingTestsData))]
    public void ApplyPagination_ForDocumentList_ReturnPagedGetDocumentDtoList(IQueryable<Document> documents,
        SieveModel query, PagedResults<Document> expected)
    {
        // arrange

        var sieveOptions = Options.Create(new SieveOptions());
        var sieveProcessor = new DocumentsAppSieveProcessor(sieveOptions);

        // act

        var pagedDocuments = sieveProcessor
            .Apply(query, documents)
            .ToList();

        var result = new PagedResults<Document>(
            pagedDocuments,
            documents.Count(),
            query.PageSize.GetValueOrDefault(),
            query.Page.GetValueOrDefault());

        // assert

        var resultDates = result.Items;
        var expectedDates = expected.Items;

        resultDates.Should().BeEquivalentTo(expectedDates, options => options.Excluding(d => d.Id));
    }
}