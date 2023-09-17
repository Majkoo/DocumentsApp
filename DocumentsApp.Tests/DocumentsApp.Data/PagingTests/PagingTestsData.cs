using System.Collections;
using DocumentsApp.Data.Entities;
using DocumentsApp.Shared.Dtos;
using Sieve.Models;

namespace DocumentsApp.Tests.DocumentsApp.Data.PagingTests;

public class PagingTestsData : IEnumerable<object[]>
{
    private readonly IQueryable<Document> _documents = new List<Document>()
    {
        new()
        {
            Name = "c",
            DateCreated = new DateTime(1999, 01, 12)
        },
        new()
        {
            Name = "a",
            DateCreated = new DateTime(1999, 01, 13)
        },
        new()
        {
            Name = "b",
            DateCreated = new DateTime(1999, 01, 1)
        },
        new()
        {
            Name = "g",
            DateCreated = new DateTime(1999, 01, 21)
        },
        new()
        {
            Name = "i",
            DateCreated = new DateTime(1999, 01, 14)
        },
        new()
        {
            Name = "o",
            DateCreated = new DateTime(1999, 01, 16)
        },
        new()
        {
            Name = "a",
            DateCreated = new DateTime(1999, 01, 2)
        }
    }.AsQueryable();
        
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[]
        {
            _documents,
            new SieveModel()
            {
                Sorts = "-DateCreated",
                PageSize = 3,
                Page = 1
            },
            new PagedResults<Document>( 
                new List<Document>()
                {
                    new()
                    {
                        Name = "g",
                        DateCreated = new DateTime(1999, 01, 21)
                    },
                    new()
                    {
                        Name = "o",
                        DateCreated = new DateTime(1999, 01, 16)
                    },
                    new()
                    {
                        Name = "i",
                        DateCreated = new DateTime(1999, 01, 14)
                    },
                },
                7,
                3,
                1)
        };

        yield return new object[]
        {
            _documents,
            new SieveModel()
            {
                Sorts = "-DateCreated",
                PageSize = 3,
                Page = 2
            },
            new PagedResults<Document>( 
                new List<Document>()
                {
                    new()
                    {
                        Name = "a",
                        DateCreated = new DateTime(1999, 01, 13)
                    },
                    new()
                    {
                        Name = "c",
                        DateCreated = new DateTime(1999, 01, 12)
                    },
                    new()
                    {
                        Name = "a",
                        DateCreated = new DateTime(1999, 01, 2)
                    }
                },
                7,
                3,
                2)
        };

        yield return new object[]
        {
            _documents,
            new SieveModel()
            {
                Sorts = "Name",
                PageSize = 3,
                Page = 1
            },
            new PagedResults<Document>( 
                new List<Document>()
                {
                    new()
                    {
                        Name = "a",
                        DateCreated = new DateTime(1999, 01, 13)
                    },
                    new()
                    {
                        Name = "a",
                        DateCreated = new DateTime(1999, 01, 2)
                    },
                    new()
                    {
                        Name = "b",
                        DateCreated = new DateTime(1999, 01, 1)
                    }
                },
                7,
                3,
                1)
        };
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}