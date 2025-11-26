using Application.Features.Location.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.CustomerManager.Queries
{
    public class GetCitiesList : IRequest<LocationListResult>
    {
        public int GovernorateId { get; init; }
    }
    public class GetCitiesQueryHandler : IRequestHandler<GetCitiesList, LocationListResult>
    {
        private static readonly Dictionary<int, List<LocationLookupDto>> Cities = new()
        {
            // Dubai (101)
            [101] = new()
        {
            new() { Id = 1001, Name = "Downtown Dubai" },
            new() { Id = 1002, Name = "Dubai Marina" },
            new() { Id = 1003, Name = "Jumeirah" },
            new() { Id = 1004, Name = "Business Bay" }
        },
            // Abu Dhabi (102)
            [102] = new()
        {
            new() { Id = 1005, Name = "Al Reem Island" },
            new() { Id = 1006, Name = "Saadiyat Island" },
            new() { Id = 1007, Name = "Khalifa City" }
        },
            // Riyadh (201)
            [201] = new()
        {
            new() { Id = 2001, Name = "Olaya" },
            new() { Id = 2002, Name = "Al Malaz" },
            new() { Id = 2003, Name = "Riyadh Season" }
        },
            // Cairo (301)
            [301] = new()
        {
            new() { Id = 3001, Name = "Maadi" },
            new() { Id = 3002, Name = "Nasr City" },
            new() { Id = 3003, Name = "Heliopolis" },
            new() { Id = 3004, Name = "New Cairo" }
        }
        };

        public Task<LocationListResult> Handle(GetCitiesList request, CancellationToken cancellationToken)
        {
            var data = Cities.TryGetValue(request.GovernorateId, out var list)
                ? list
                : new List<LocationLookupDto>();

            return Task.FromResult(new LocationListResult { Data = data });
        }
    }
}
