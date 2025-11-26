using Application.Common.Repositories;
using Application.Features.NumberSequenceManager;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Infrastructure.SeedManager.Demos;

public class VendorSeeder
{
    private readonly ICommandRepository<Vendor> _vendorRepository;
    private readonly ICommandRepository<VendorGroup> _groupRepository;
    private readonly ICommandRepository<VendorCategory> _categoryRepository;
    private readonly NumberSequenceService _numberSequenceService;
    private readonly IUnitOfWork _unitOfWork;

    public VendorSeeder(
        ICommandRepository<Vendor> vendorRepository,
        ICommandRepository<VendorGroup> groupRepository,
        ICommandRepository<VendorCategory> categoryRepository,
        NumberSequenceService numberSequenceService,
        IUnitOfWork unitOfWork
    )
    {
        _vendorRepository = vendorRepository;
        _groupRepository = groupRepository;
        _categoryRepository = categoryRepository;
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


        
        var random = new Random();

        var vendors = new List<Vendor>
        {
            new Vendor { Name = "Quantum Industries" },
            new Vendor { Name = "Apex Ventures" },
            new Vendor { Name = "Horizon Enterprises" },
            new Vendor { Name = "Nova Innovations" },
            new Vendor { Name = "Phoenix Holdings" },
            new Vendor { Name = "Titan Group" },
            new Vendor { Name = "Zenith Corporation" },
            new Vendor { Name = "Prime Solutions" },
            new Vendor { Name = "Cascade Enterprises" },
            new Vendor { Name = "Aurora Holdings" },
            new Vendor { Name = "Vanguard Industries" },
            new Vendor { Name = "Empyrean Ventures" },
            new Vendor { Name = "Genesis Corporation" },
            new Vendor { Name = "Equinox Enterprises" },
            new Vendor { Name = "Summit Holdings" },
            new Vendor { Name = "Sovereign Solutions" },
            new Vendor { Name = "Spectrum Corporation" },
            new Vendor { Name = "Elysium Enterprises" },
            new Vendor { Name = "Infinity Holdings" },
            new Vendor { Name = "Momentum Ventures" }
        };

        foreach (var vendor in vendors)
        {
            vendor.Number = _numberSequenceService.GenerateNumber(nameof(Vendor), "", "CST");
            vendor.VendorGroupId = groups.Length > 0 ? groups[random.Next(groups.Length)] : null;
            vendor.TRN = random.Next(10000000, 99999999).ToString();

            // assign demo location ids/names (adjust to your actual location id scheme)
            vendor.CountryId = GetRandomString(countries, random);
            vendor.GovernorateId = GetRandomString(governorates, random);
            vendor.CityId = GetRandomString(cities, random);

            vendor.BuildingNumber = GetRandomString(buildings, random);
            vendor.Floor = GetRandomString(floors, random);
            vendor.FlatNumber = GetRandomString(flats, random);
            vendor.Street = GetRandomString(streets, random);
            vendor.PostalCode = GetRandomString(postalCodes, random);

            vendor.Mobile = GetRandomString(mobileNumbers, random);
            await _vendorRepository.CreateAsync(vendor);
        }

        await _unitOfWork.SaveAsync();
    }

    private static T GetRandomValue<T>(T[] array, Random random)
    {
        return array[random.Next(array.Length)];
    }

    private static string GetRandomString(string[] array, Random random)
    {
        return array[random.Next(array.Length)];
    }
}
