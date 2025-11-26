using Application.Features.Location.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.CustomerManager.Queries
{
    public class GetGovernoratesList : IRequest<LocationListResult>
    {
        public int CountryId { get; init; }
    }
    public class GetGovernoratesListHandler : IRequestHandler<GetGovernoratesList, LocationListResult>
    {
        private static readonly Dictionary<int, List<LocationLookupDto>> Governorates = new()
        {
            // UAE
            [1] = new()
        {
            new() { Id = 101, Name = "Dubai" },
            new() { Id = 102, Name = "Abu Dhabi" },
            new() { Id = 103, Name = "Sharjah" },
            new() { Id = 104, Name = "Ajman" },
            new() { Id = 105, Name = "Ras Al Khaimah" }
        },
            // Saudi Arabia
            [2] = new()
        {
            new() { Id = 201, Name = "Riyadh" },
            new() { Id = 202, Name = "Makkah" },
            new() { Id = 203, Name = "Madinah" },
            new() { Id = 204, Name = "Eastern Province" }
        },
            // Egypt
            [3] = new()
        {
            new() { Id = 301, Name = "Cairo" },
            new() { Id = 302, Name = "Alexandria" },
            new() { Id = 303, Name = "Giza" },
            new() { Id = 304, Name = "Dakahlia" }
        }
        };

        public Task<LocationListResult> Handle(GetGovernoratesList request, CancellationToken cancellationToken)
        {
            var data = Governorates.TryGetValue(request.CountryId, out var list)
                ? list
                : new List<LocationLookupDto>();

            return Task.FromResult(new LocationListResult { Data = data });
        }
    }
}
