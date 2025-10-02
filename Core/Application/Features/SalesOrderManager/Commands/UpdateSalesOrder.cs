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
    public string? UpdatedById { get; init; }
}

public class UpdateSalesOrderValidator : AbstractValidator<UpdateSalesOrderRequest>
{
    public UpdateSalesOrderValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.OrderStatus).NotEmpty();
        RuleFor(x => x.CustomerId).NotEmpty();
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

        entity.UpdatedById = request.UpdatedById;
        entity.OrderStatus = (SalesOrderStatus)int.Parse(request.OrderStatus!);
        entity.Description = request.Description;
        entity.CustomerId = request.CustomerId;

        _repository.Update(entity);
        await _unitOfWork.SaveAsync(cancellationToken);

        _salesOrderService.Recalculate(entity.Id);

        return new UpdateSalesOrderResult
        {
            Data = entity
        };
        
    }
}

