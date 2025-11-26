using Application.Common.Repositories;
using Application.Features.NumberSequenceManager;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.CustomerManager.Commands;

public class CreateCustomerResult
{
    public Customer? Data { get; set; }
}

public class CreateCustomerRequest : IRequest<CreateCustomerResult>
{
    public string? CustomerGroupId { get; set; }
    public string? Name { get; set; }
    public string? TRN { get; set; }
    // address
    public string? CountryId { get; set; }
    public string? GovernorateId { get; set; }
    public string? CityId { get; set; }
    public string? BuildingNumber { get; set; }
    public string? Floor { get; set; }
    public string? FlatNumber { get; set; }
    public string? Street { get; set; }
    public string? PostalCode { get; set; }
    public string? Mobile { get; set; }

    public string? CreatedById { get; init; }
}

public class CreateCustomerValidator : AbstractValidator<CreateCustomerRequest>
{
    public CreateCustomerValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.CustomerGroupId).NotEmpty();
        RuleFor(x => x.CountryId).NotEmpty();
        RuleFor(x => x.GovernorateId).NotEmpty();
        RuleFor(x => x.CityId).NotEmpty();
    }
}

public class CreateCustomerHandler : IRequestHandler<CreateCustomerRequest, CreateCustomerResult>
{
    private readonly ICommandRepository<Customer> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly NumberSequenceService _numberSequenceService;

    public CreateCustomerHandler(
        ICommandRepository<Customer> repository,
        IUnitOfWork unitOfWork,
        NumberSequenceService numberSequenceService
        )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _numberSequenceService = numberSequenceService;
    }

    public async Task<CreateCustomerResult> Handle(CreateCustomerRequest request, CancellationToken cancellationToken = default)
    {
        var entity = new Customer();
        entity.CreatedById = request.CreatedById;

        entity.CustomerGroupId = request.CustomerGroupId;
        entity.Name = request.Name;
        entity.TRN = request.TRN;
        entity.Number = _numberSequenceService.GenerateNumber(nameof(Customer), "", "CST");

        entity.CountryId = request.CountryId;
        entity.GovernorateId = request.GovernorateId;
        entity.CityId = request.CityId;
        entity.BuildingNumber = request.BuildingNumber;
        entity.Floor = request.Floor;
        entity.FlatNumber = request.FlatNumber;
        entity.Street = request.Street;
        entity.PostalCode = request.PostalCode;

        entity.Mobile = request.Mobile;

        await _repository.CreateAsync(entity, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);

        return new CreateCustomerResult
        {
            Data = entity
        };
    }
}