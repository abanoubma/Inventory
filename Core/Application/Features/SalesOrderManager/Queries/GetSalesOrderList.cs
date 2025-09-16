using Application.Common.CQS.Queries;
using Application.Common.Extensions;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.SalesOrderManager.Queries;

// csharp
public record GetSalesOrderListDto
{
    public string? Id { get; init; }
    public string? Number { get; init; }
    public DateTime? OrderDate { get; init; }
    public SalesOrderStatus? OrderStatus { get; init; }
    public string? OrderStatusName { get; init; }
    public string? Description { get; init; }
    public string? CustomerId { get; init; }
    public string? CustomerName { get; init; }

    // new: lists + combined string for display
    public List<string>? TaxIds { get; init; }
    public List<string>? TaxNames { get; init; }

    // kept for direct grid binding (server-side joined display)
    public string TaxNamesCombined { get; init; } = string.Empty;

    public double? BeforeTaxAmount { get; init; }
    public double? TaxAmount { get; init; }
    public double? AfterTaxAmount { get; init; }
    public DateTime? CreatedAtUtc { get; init; }
}


// csharp
public class GetSalesOrderListProfile : Profile
{
    public GetSalesOrderListProfile()
    {
        // map SalesOrderTax -> tax id / name (if you want a DTO for the tax join you can create one;
        // here we map directly to lists on the destination)
        CreateMap<SalesOrder, GetSalesOrderListDto>()
            .ForMember(
                dest => dest.CustomerName,
                opt => opt.MapFrom(src => src.Customer != null ? src.Customer.Name : string.Empty)
            )
            // TaxIds list
            .ForMember(
                dest => dest.TaxIds,
                opt => opt.MapFrom(src =>
                    src.SalesOrderTaxes != null
                        ? src.SalesOrderTaxes.Select(st => st.TaxId).ToList()
                        : new List<string>()
                )
            )
            // TaxNames list (extract tax.Name from included Tax navigation)
            .ForMember(
                dest => dest.TaxNames,
                opt => opt.MapFrom(src =>
                    src.SalesOrderTaxes != null
                        ? src.SalesOrderTaxes.Select(st => st.Tax != null ? st.Tax.Name : string.Empty).ToList()
                        : new List<string>()
                )
            )
            // Combined names for easy grid display
            .ForMember(
                dest => dest.TaxNamesCombined,
                opt => opt.MapFrom(src =>
                    src.SalesOrderTaxes != null && src.SalesOrderTaxes.Any()
                        ? string.Join(", ", src.SalesOrderTaxes.Select(st => st.Tax != null ? st.Tax.Name : string.Empty).Where(n => !string.IsNullOrWhiteSpace(n)))
                        : string.Empty
                )
            )
            .ForMember(
                dest => dest.OrderStatusName,
                opt => opt.MapFrom(src => src.OrderStatus.HasValue ? src.OrderStatus.Value.ToFriendlyName() : string.Empty)
            );
    }
}


public class GetSalesOrderListResult
{
    public List<GetSalesOrderListDto>? Data { get; init; }
}

public class GetSalesOrderListRequest : IRequest<GetSalesOrderListResult>
{
    public bool IsDeleted { get; init; } = false;
}


public class GetSalesOrderListHandler : IRequestHandler<GetSalesOrderListRequest, GetSalesOrderListResult>
{
    private readonly IMapper _mapper;
    private readonly IQueryContext _context;

    public GetSalesOrderListHandler(IMapper mapper, IQueryContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<GetSalesOrderListResult> Handle(GetSalesOrderListRequest request, CancellationToken cancellationToken)
    {
        var query = _context
            .SalesOrder
            .AsNoTracking()
            .ApplyIsDeletedFilter(request.IsDeleted)
            .Include(x => x.Customer)
            .Include(x => x.SalesOrderTaxes) 
                .ThenInclude(st => st.Tax)   
            .AsQueryable();

        var entities = await query.ToListAsync(cancellationToken);

        var dtos = _mapper.Map<List<GetSalesOrderListDto>>(entities);

        return new GetSalesOrderListResult
        {
            Data = dtos
        };
    }


}



