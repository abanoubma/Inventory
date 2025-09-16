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
        var salesOrder = _salesOrderRepository
            .GetQuery()
            .ApplyIsDeletedFilter()
            .Where(x => x.Id == salesOrderId)
            .Include(x => x.SalesOrderTaxes)
               .ThenInclude(st => st.Tax)
            .Include(x => x.SalesOrderItemList) // Include the SalesOrderItemList
            .SingleOrDefault();

        if (salesOrder == null)
            return;

        // Calculate subtotal from all sales order items
        salesOrder.BeforeTaxAmount = salesOrder.SalesOrderItemList.Sum(x => x.Total ?? 0);

        // Calculate total tax from all associated taxes
        double totalTaxAmount = 0;
        foreach (var salesOrderTax in salesOrder.SalesOrderTaxes)
        {
            if (salesOrderTax.Tax != null)
            {
                // Use the appropriate property name from your Tax entity
                // If your Tax entity has a Percentage property, use that
                // If it has a Rate property, use that instead
                double taxPercentage = (double)salesOrderTax.Tax.Percentage; // or salesOrderTax.Tax.Rate
                totalTaxAmount += (salesOrder.BeforeTaxAmount ?? 0) * taxPercentage / 100;
            }
        }

        salesOrder.TaxAmount = totalTaxAmount;
        salesOrder.AfterTaxAmount = (salesOrder.BeforeTaxAmount ?? 0) + totalTaxAmount;

        _salesOrderRepository.Update(salesOrder);
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
