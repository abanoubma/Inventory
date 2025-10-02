using Application.Common.Extensions;
using Application.Common.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.SalesOrderManager;

public class SalesOrderService
{
    private readonly ICommandRepository<SalesOrder> _salesOrderRepository;
    private readonly ICommandRepository<SalesOrderItem> _salesOrderItemRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SalesOrderService(
        ICommandRepository<SalesOrder> salesOrderRepository,
        ICommandRepository<SalesOrderItem> salesOrderItemRepository,
        IUnitOfWork unitOfWork
        )
    {
        _salesOrderRepository = salesOrderRepository;
        _salesOrderItemRepository = salesOrderItemRepository;
        _unitOfWork = unitOfWork;
    }

    public void Recalculate(string salesOrderId)
    {
        var salesOrder = _salesOrderRepository.GetQuery()
            .Include(so => so.SalesOrderItemList)
                .ThenInclude(item => item.Product)
                    .ThenInclude(p => p.Tax)
            .Include(so => so.SalesOrderItemList)
                .ThenInclude(item => item.Product)
                    .ThenInclude(p => p.Vat)
            .FirstOrDefault(x => x.Id == salesOrderId);

        if (salesOrder == null) return;

        double subtotal = 0;
        double totalTax = 0;
        double totalVat = 0;

        foreach (var item in salesOrder.SalesOrderItemList ?? new List<SalesOrderItem>())
        {
            var itemTotal = (item.Quantity ?? 0) * (item.UnitPrice ?? 0);
            subtotal += itemTotal;

            // Calculate tax from product
            if (item.Product?.Tax?.Percentage != null)
            {
                totalTax += itemTotal * (item.Product.Tax.Percentage.Value / 100);
            }

            // Calculate VAT from product
            if (item.Product?.Vat?.Percentage != null)
            {
                totalVat += itemTotal * (item.Product.Vat.Percentage.Value / 100);
            }
        }

        salesOrder.BeforeTaxAmount = subtotal;
        salesOrder.TaxAmount = totalTax + totalVat;
        salesOrder.AfterTaxAmount = subtotal + totalTax + totalVat;

        _unitOfWork.Save();
    }

    //public void Recalculate2(string salesOrderId)
    //{
    //    var salesOrder = _salesOrderRepository.GetQuery()
    //        .Include(so => so.SalesOrderItemList)
    //        .Include(so => so.SalesOrderTaxes)
    //            .ThenInclude(st => st.Tax)
    //        .FirstOrDefault(so => so.Id == salesOrderId);

    //    if (salesOrder == null) return;

    //    // Calculate subtotal
    //    double? subtotal = salesOrder.SalesOrderItemList.Sum(item => item.Total);

    //    // Calculate tax from multiple tax rates
    //    decimal totalTax = 0;
    //    foreach (var salesOrderTax in salesOrder.SalesOrderTaxes)
    //    {
    //        totalTax += subtotal * (salesOrderTax.Tax.Rate / 100m);
    //    }

    //    // Update sales order totals
    //    salesOrder.BeforeTaxAmount = subtotal;
    //    salesOrder.TaxAmount = totalTax;
    //    salesOrder.AfterTaxAmount = subtotal + totalTax;

    //    _salesOrderRepository.Update(salesOrder);
    //    _unitOfWork.Save();
    //}
}
