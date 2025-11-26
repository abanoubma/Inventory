using Domain.Common;

namespace Domain.Entities;

public class Customer : BaseEntity
{
    // Basic
    public string? Name { get; set; }
    public string? Number { get; set; }          // generated sequence
    public string? TRN { get; set; }             // Tax Registration Number

    // Address (hierarchy)
    public string? CountryId { get; set; }       // dropdown
    public string? GovernorateId { get; set; }   // filtered by CountryId
    public string? CityId { get; set; }          // filtered by GovernorateId

    public string? BuildingNumber { get; set; }
    public string? Floor { get; set; }
    public string? FlatNumber { get; set; }
    public string? Street { get; set; }
    public string? PostalCode { get; set; }

    // Communication
    public string? Mobile { get; set; }

    // Grouping
    public string? CustomerGroupId { get; set; }
    public CustomerGroup? CustomerGroup { get; set; }

    // Contacts
    public ICollection<CustomerContact> CustomerContactList { get; set; } = new List<CustomerContact>();
}
