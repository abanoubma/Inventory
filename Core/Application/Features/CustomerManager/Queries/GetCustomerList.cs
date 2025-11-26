using Application.Common.CQS.Queries;
using Application.Common.Extensions;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.CustomerManager.Queries;

public record GetCustomerListDto
{
    public string? Id { get; init; }
    public string? Name { get; set; }
    public string? Number { get; set; }
    public string? TRN { get; set; }

    // address ids (you can also map names if you keep nav props)
    public string? CountryId { get; set; }
    public string? GovernorateId { get; set; }
    public string? CityId { get; set; }

    public string? BuildingNumber { get; set; }
    public string? Floor { get; set; }
    public string? FlatNumber { get; set; }
    public string? Street { get; set; }
    public string? PostalCode { get; set; }

    public string? Mobile { get; set; }

    public string? CustomerGroupId { get; set; }
    public string? CustomerGroupName { get; set; }

    public DateTime? CreatedAtUtc { get; init; }
}

public class GetCustomerListProfile : Profile
{
    public GetCustomerListProfile()
    {
        CreateMap<Customer, GetCustomerListDto>()
            .ForMember(dest => dest.CustomerGroupName, opt =>
                opt.MapFrom(src => src.CustomerGroup != null ? src.CustomerGroup.Name : string.Empty)
            );
    }
}

public class GetCustomerListResult
{
    public List<GetCustomerListDto>? Data { get; init; }
}

public class GetCustomerListRequest : IRequest<GetCustomerListResult>
{
    public bool IsDeleted { get; init; } = false;
}


public class GetCustomerListHandler : IRequestHandler<GetCustomerListRequest, GetCustomerListResult>
{
    private readonly IMapper _mapper;
    private readonly IQueryContext _context;

    public GetCustomerListHandler(IMapper mapper, IQueryContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<GetCustomerListResult> Handle(GetCustomerListRequest request, CancellationToken cancellationToken)
    {
        var query = _context
            .Customer
            .AsNoTracking()
            .ApplyIsDeletedFilter(request.IsDeleted)
            .Include(x => x.CustomerGroup)
            .AsQueryable();

        var entities = await query.ToListAsync(cancellationToken);

        var dtos = _mapper.Map<List<GetCustomerListDto>>(entities);

        return new GetCustomerListResult
        {
            Data = dtos
        };
    }
}



