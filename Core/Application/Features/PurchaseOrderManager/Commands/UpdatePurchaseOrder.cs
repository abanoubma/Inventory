using Application.Common.Extensions;
using Application.Common.Repositories;
using Domain.Entities;
using Domain.Enums;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.PurchaseOrderManager.Commands;

public class UpdatePurchaseOrderResult
{
    public PurchaseOrder? Data { get; set; }
}

public class UpdatePurchaseOrderRequest : IRequest<UpdatePurchaseOrderResult>
{
    public string? Id { get; init; }
  //  public DateTime? OrderDate { get; init; }
    public string? OrderStatus { get; init; }
    public string? Description { get; init; }
    public string? VendorId { get; init; }
    //public string? TaxId { get; init; }
    public string? UpdatedById { get; init; }
    public List<string>? TaxId { get; init; }
}

public class UpdatePurchaseOrderValidator : AbstractValidator<UpdatePurchaseOrderRequest>
{
    public UpdatePurchaseOrderValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
       // RuleFor(x => x.OrderDate).NotEmpty();
        RuleFor(x => x.OrderStatus).NotEmpty();
        RuleFor(x => x.VendorId).NotEmpty();
      //  RuleFor(x => x.TaxId).NotEmpty();
    }
}

public class UpdatePurchaseOrderHandler : IRequestHandler<UpdatePurchaseOrderRequest, UpdatePurchaseOrderResult>
{
    private readonly ICommandRepository<PurchaseOrder> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly PurchaseOrderService _purchaseOrderService;

    public UpdatePurchaseOrderHandler(
        ICommandRepository<PurchaseOrder> repository,
        IUnitOfWork unitOfWork,
        PurchaseOrderService purchaseOrderService
        )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _purchaseOrderService = purchaseOrderService;
    }

    // csharp
    // csharp
    public async Task<UpdatePurchaseOrderResult> Handle(UpdatePurchaseOrderRequest request, CancellationToken cancellationToken)
    {
        var id = request.Id ?? string.Empty;

        // Load PO with its PurchaseOrderTaxes collection
        var entity = await _repository
            .GetQuery()
            .ApplyIsDeletedFilter()
            .Include(p => p.PurchaseOrderTaxes)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

        if (entity == null)
            throw new Exception($"Entity not found: {request.Id}");

        // update simple properties
        entity.UpdatedById = request.UpdatedById;
        entity.OrderStatus = (PurchaseOrderStatus)int.Parse(request.OrderStatus!);
        entity.Description = request.Description;
        entity.VendorId = request.VendorId;
        // note: no TaxId single property anymore

        // normalize incoming tax ids (request.TaxId is List<string>?)
        var incomingTaxIds = (request.TaxId ?? new List<string>())
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => x!.Trim())
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        // ensure collection exists
        if (entity.PurchaseOrderTaxes == null)
            entity.PurchaseOrderTaxes = new List<PurchaseOrderTax>();

        // remove PurchaseOrderTax entries that are not in the incoming set
        var toRemove = entity.PurchaseOrderTaxes
            .Where(pt => !incomingTaxIds.Contains(pt.TaxId))
            .ToList();

        foreach (var rem in toRemove)
        {
            // If you use soft-delete, set rem.IsDeleted = true and set UpdatedAtUtc/UpdatedById
            // rem.IsDeleted = true;
            // rem.UpdatedAtUtc = DateTime.UtcNow;
            // rem.UpdatedById = request.UpdatedById;
            // otherwise remove from collection for a hard delete:
            entity.PurchaseOrderTaxes.Remove(rem);
        }

        // add new PurchaseOrderTax rows for tax ids that are not already present
        var existingTaxIds = entity.PurchaseOrderTaxes.Select(pt => pt.TaxId).ToHashSet(StringComparer.OrdinalIgnoreCase);

        foreach (var taxId in incomingTaxIds.Except(existingTaxIds, StringComparer.OrdinalIgnoreCase))
        {
            var newPoTax = new PurchaseOrderTax
            {
                PurchaseOrderId = entity.Id,
                TaxId = taxId,
                Id = Guid.NewGuid().ToString(),        // adapt to your ID generation policy
                IsDeleted = false,
                CreatedAtUtc = DateTime.UtcNow,
                CreatedById = request.UpdatedById
            };

            entity.PurchaseOrderTaxes.Add(newPoTax);
        }

        // persist changes
        _repository.Update(entity);
        await _unitOfWork.SaveAsync(cancellationToken);

        // recalc totals using PurchaseOrderTaxes
        _purchaseOrderService.Recalculate(entity.Id);

        return new UpdatePurchaseOrderResult
        {
            Data = entity
        };
    }

}

