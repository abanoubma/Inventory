using Domain.Common;
using Domain.Enums;
using System.Collections.Generic;

namespace Domain.Entities;

public class SalesOrder : BaseEntity
{
    public string? Number { get; set; }
    public DateTime? OrderDate { get; set; }
    public SalesOrderStatus? OrderStatus { get; set; }
    public string? Description { get; set; }
    public string? CustomerId { get; set; }
    public Customer? Customer { get; set; }
    //public List<string>? TaxId { get; set; }
    //public Tax? Tax { get; set; }
    public double? BeforeTaxAmount { get; set; }
    public double? TaxAmount { get; set; }
    public double? AfterTaxAmount { get; set; }
    public ICollection<SalesOrderItem> SalesOrderItemList { get; set; } = new List<SalesOrderItem>();
    public virtual ICollection<SalesOrderTax> SalesOrderTaxes { get; set; } = new List<SalesOrderTax>();

}
