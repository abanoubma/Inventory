using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Location.Queries
{
    public class LocationLookupDto
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
    }

    public class LocationListResult
    {
        public List<LocationLookupDto> Data { get; init; } = new();
    }
    public class GetCountriesList : IRequest<LocationListResult>
    {
    }
    // Application/Features/Location/Queries/GetCountriesQueryHandler.cs

    public class GetCountriesQueryHandler : IRequestHandler<GetCountriesList, LocationListResult>
    {
        // Hardcoded data – perfect for demo / development
        private static readonly List<LocationLookupDto> Countries = new()
    {
        new() { Id = 1, Name = "United Arab Emirates" },
        new() { Id = 2, Name = "Saudi Arabia" },
        new() { Id = 3, Name = "Egypt" },
        new() { Id = 4, Name = "Qatar" },
        new() { Id = 5, Name = "Kuwait" }
    };

        public Task<LocationListResult> Handle(GetCountriesList request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new LocationListResult
            {
                Data = Countries
            });
        }
    }
}
