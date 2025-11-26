using Application.Common.CQS.Queries;
using Application.Common.Extensions;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.VendorManager.Queries;

public record GetVendorListDto
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

    public string? VendorGroupId { get; set; }
    public string? VendorGroupName { get; set; }

    public DateTime? CreatedAtUtc { get; init; }
}

public class GetVendorListProfile : Profile
{
    public GetVendorListProfile()
    {
        CreateMap<Vendor, GetVendorListDto>()
            .ForMember(
                dest => dest.VendorGroupName,
                opt => opt.MapFrom(src => src.VendorGroup != null ? src.VendorGroup.Name : string.Empty)
            );

    }
}

public class GetVendorListResult
{
    public List<GetVendorListDto>? Data { get; init; }
}

public class GetVendorListRequest : IRequest<GetVendorListResult>
{
    public bool IsDeleted { get; init; } = false;
}


public class GetVendorListHandler : IRequestHandler<GetVendorListRequest, GetVendorListResult>
{
    private readonly IMapper _mapper;
    private readonly IQueryContext _context;

    public GetVendorListHandler(IMapper mapper, IQueryContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<GetVendorListResult> Handle(GetVendorListRequest request, CancellationToken cancellationToken)
    {
        var query = _context
            .Vendor
            .AsNoTracking()
            .ApplyIsDeletedFilter(request.IsDeleted)
            .Include(x => x.VendorGroup)
            .AsQueryable();

        var entities = await query.ToListAsync(cancellationToken);

        var dtos = _mapper.Map<List<GetVendorListDto>>(entities);

        return new GetVendorListResult
        {
            Data = dtos
        };
    }


}



