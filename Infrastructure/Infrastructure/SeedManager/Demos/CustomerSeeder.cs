using Application.Common.Repositories;
using Application.Features.NumberSequenceManager;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SeedManager.Demos;

public class CustomerSeeder
{
    private readonly ICommandRepository<Customer> _customerRepository;
    private readonly ICommandRepository<CustomerGroup> _groupRepository;
    private readonly NumberSequenceService _numberSequenceService;
    private readonly IUnitOfWork _unitOfWork;

    public CustomerSeeder(
        ICommandRepository<Customer> customerRepository,
        ICommandRepository<CustomerGroup> groupRepository,
        NumberSequenceService numberSequenceService,
        IUnitOfWork unitOfWork
    )
    {
        _customerRepository = customerRepository;
        _groupRepository = groupRepository;
        _numberSequenceService = numberSequenceService;
        _unitOfWork = unitOfWork;
    }

    public async Task GenerateDataAsync()
    {
        var groups = (await _groupRepository.GetQuery().ToListAsync()).Select(x => x.Id).ToArray();

        // Demo location identifiers/names for seeding. Replace with real location ids if available.
        var countries = new string[] { "EG", "US", "AE", "SA" };
        var governorates = new string[] { "Cairo", "California", "Dubai", "Riyadh" };
        var cities = new string[] { "Nasr City", "Los Angeles", "Jumeirah", "Olaya" };

        var streets = new string[] { "Main St", "Broadway", "Market St", "Elm St" };
        var buildings = new string[] { "1", "2", "10", "25" };
        var floors = new string[] { "1", "2", "3", "10" };
        var flats = new string[] { "101", "202", "303", "404" };
        var postalCodes = new string[] { "10001", "90001", "94101", "60601" };
        var mobileNumbers = new string[] { "01001112233", "01122334455", "01233445566", "01555666777" };
        var emailDomains = new string[] { "example.com", "demo.com", "test.com", "sample.com" };

        var random = new Random();

        var customers = new List<Customer>
        {
            new Customer { Name = "Citadel LLC" },
            new Customer { Name = "Ironclad LLC" },
            new Customer { Name = "Armada LLC" },
            new Customer { Name = "Shield LLC" },
            new Customer { Name = "Alpha LLC" },
            new Customer { Name = "Capitol LLC" },
            new Customer { Name = "Federal LLC" },
            new Customer { Name = "Statewide LLC" },
            new Customer { Name = "Harmony LLC" },
            new Customer { Name = "Hope LLC" },
            new Customer { Name = "Unity LLC" },
            new Customer { Name = "Prosperity LLC" },
            new Customer { Name = "Global LLC" },
            new Customer { Name = "Sunset LLC" },
            new Customer { Name = "Luxe LLC" },
            new Customer { Name = "Serenity LLC" },
            new Customer { Name = "Oasis LLC" },
            new Customer { Name = "Grandeur LLC" },
            new Customer { Name = "Bright LLC" },
            new Customer { Name = "Stellar LLC" }
        };

        foreach (var customer in customers)
        {
            customer.Number = _numberSequenceService.GenerateNumber(nameof(Customer), "", "CST");
                customer.CustomerGroupId = groups.Length > 0 ? groups[random.Next(groups.Length)] : null;
            customer.TRN = random.Next(10000000, 99999999).ToString();

            // assign demo location ids/names (adjust to your actual location id scheme)
            customer.CountryId = GetRandomString(countries, random);
            customer.GovernorateId = GetRandomString(governorates, random);
            customer.CityId = GetRandomString(cities, random);

            customer.BuildingNumber = GetRandomString(buildings, random);
            customer.Floor = GetRandomString(floors, random);
            customer.FlatNumber = GetRandomString(flats, random);
            customer.Street = GetRandomString(streets, random);
            customer.PostalCode = GetRandomString(postalCodes, random);

            customer.Mobile = GetRandomString(mobileNumbers, random);

            await _customerRepository.CreateAsync(customer);
        }

        await _unitOfWork.SaveAsync();
    }

    private static string GetRandomString(string[] array, Random random)
    {
        return array[random.Next(array.Length)];
    }
}
