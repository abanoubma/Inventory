using Application.Common.Repositories;
using Domain.Entities;
using Domain.Enums;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.SalesOrderManager.Commands;

public class UpdateSalesOrderResult
{
    public SalesOrder? Data { get; set; }
}

public class UpdateSalesOrderRequest : IRequest<UpdateSalesOrderResult>
{
    public string? Id { get; init; }
    // public DateTime? OrderDate { get; init; }
    public string? OrderStatus { get; init; }
    public string? Description { get; init; }
    public string? CustomerId { get; init; }
    public List<string>? TaxId { get; init; }
    public string? UpdatedById { get; init; }
}

public class UpdateSalesOrderValidator : AbstractValidator<UpdateSalesOrderRequest>
{
    public UpdateSalesOrderValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
       // RuleFor(x => x.OrderDate).NotEmpty();
        RuleFor(x => x.OrderStatus).NotEmpty();
        RuleFor(x => x.CustomerId).NotEmpty();
        //RuleFor(x => x.TaxId).NotEmpty();RuleFor(x => x.TaxIds)
        RuleFor(x => x.TaxId).NotEmpty().WithMessage("At least one tax must be selected.")
        .Must(taxIds => taxIds != null && taxIds.Any()).WithMessage("At least one tax must be selected.");
    }
}

public class UpdateSalesOrderHandler : IRequestHandler<UpdateSalesOrderRequest, UpdateSalesOrderResult>
{
    private readonly ICommandRepository<SalesOrder> _repository;
    private readonly ICommandRepository<SalesOrderTax> _salesOrderTaxRepository;

    private readonly IUnitOfWork _unitOfWork;
    private readonly SalesOrderService _salesOrderService;

    public UpdateSalesOrderHandler(
        ICommandRepository<SalesOrder> repository,
        ICommandRepository<SalesOrderTax> salesOrderTaxRepository,
        SalesOrderService salesOrderService,
        IUnitOfWork unitOfWork
        )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _salesOrderService = salesOrderService;
        _salesOrderTaxRepository = salesOrderTaxRepository;
    }

    //public async Task<UpdateSalesOrderResult> Handle(UpdateSalesOrderRequest request, CancellationToken cancellationToken)
    //{

    //    var entity = await _repository.GetAsync(request.Id ?? string.Empty, cancellationToken);

    //    if (entity == null)
    //    {
    //        throw new Exception($"Entity not found: {request.Id}");
    //    }

    //    entity.UpdatedById = request.UpdatedById;

    //    entity.OrderDate = request.OrderDate;
    //    entity.OrderStatus = (SalesOrderStatus)int.Parse(request.OrderStatus!);
    //    entity.Description = request.Description;
    //    entity.CustomerId = request.CustomerId;
    //    entity.TaxId = request.TaxId;

    //    _repository.Update(entity);
    //    await _unitOfWork.SaveAsync(cancellationToken);

    //    _salesOrderService.Recalculate(entity.Id);

    //    return new UpdateSalesOrderResult
    //    {
    //        Data = entity
    //    };
    //}


    // csharp
    public async Task<UpdateSalesOrderResult> Handle(UpdateSalesOrderRequest request, CancellationToken cancellationToken)
    {
        // load the sales order including the join collection
        var entity = await _repository
            .GetQuery()
            .Include(x => x.SalesOrderTaxes)
            .ThenInclude(st => st.Tax) // optional but useful
            .FirstOrDefaultAsync(x => x.Id == (request.Id ?? string.Empty), cancellationToken);

        if (entity == null)
            throw new Exception($"Entity not found: {request.Id}");

        // update scalar properties
        entity.UpdatedById = request.UpdatedById;
        entity.OrderStatus = (SalesOrderStatus)int.Parse(request.OrderStatus!);
        entity.Description = request.Description;
        entity.CustomerId = request.CustomerId;

        // ensure collection exists and operate on the tracked collection to avoid duplicate tracking
        if (entity.SalesOrderTaxes == null)
            entity.SalesOrderTaxes = new List<SalesOrderTax>();

        // 1) Normalize incoming tax ids to a HashSet (so duplicates are ignored)
        var incomingTaxIds = (request.TaxId ?? new List<string>())
            .Where(id => !string.IsNullOrWhiteSpace(id))
            .Select(id => id!.Trim())
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        // 2) Remove any existing tracked join rows that are NOT in incoming set
        //    Removing from the collection will mark them as Deleted in the change tracker.
        var existing = entity.SalesOrderTaxes.ToList(); // snapshot to avoid modifying during enumeration
        foreach (var st in existing)
        {
            if (!incomingTaxIds.Contains(st.TaxId))
            {
                entity.SalesOrderTaxes.Remove(st);
                // If you prefer soft-delete, set st.IsDeleted = true instead of removing:
                // st.IsDeleted = true; st.UpdatedAtUtc = DateTime.UtcNow; st.UpdatedById = request.UpdatedById;
            }
            else
            {
                // keep it — ensure incoming doesn't create duplicate
                incomingTaxIds.Remove(st.TaxId);
            }
        }

        // 3) incomingTaxIds now contains only new tax ids to add
        foreach (var taxId in incomingTaxIds)
        {
            if (string.IsNullOrWhiteSpace(taxId)) continue;

            var newJoin = new SalesOrderTax
            {
                SalesOrderId = entity.Id,
                TaxId = taxId,
                Id = Guid.NewGuid().ToString() // adapt to your Id policy if needed
            };

            entity.SalesOrderTaxes.Add(newJoin);
        }

        // 4) update parent and save
        _repository.Update(entity);
        await _unitOfWork.SaveAsync(cancellationToken);

        // 5) recalc totals
        _salesOrderService.Recalculate(entity.Id);

        return new UpdateSalesOrderResult { Data = entity };
    }
}

