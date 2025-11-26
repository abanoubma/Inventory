using Application.Common.Repositories;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.CustomerManager.Commands;

public class UpdateCustomerResult
{
    public Customer? Data { get; set; }
}

public class UpdateCustomerRequest : IRequest<UpdateCustomerResult>
{
    public string? Id { get; init; }
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

    public string? UpdatedById { get; init; }
}

public class UpdateCustomerValidator : AbstractValidator<UpdateCustomerRequest>
{
    public UpdateCustomerValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.CustomerGroupId).NotEmpty();

        // Street, Mobile and other address fields are optional on update as well.
    }
}

public class UpdateCustomerHandler : IRequestHandler<UpdateCustomerRequest, UpdateCustomerResult>
{
    private readonly ICommandRepository<Customer> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCustomerHandler(
        ICommandRepository<Customer> repository,
        IUnitOfWork unitOfWork
        )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<UpdateCustomerResult> Handle(UpdateCustomerRequest request, CancellationToken cancellationToken)
    {

        var entity = await _repository.GetAsync(request.Id ?? string.Empty, cancellationToken);

        if (entity == null)
        {
            throw new Exception($"Entity not found: {request.Id}");
        }

        entity.UpdatedById = request.UpdatedById;

        entity.CustomerGroupId = request.CustomerGroupId;
        entity.Name = request.Name;
        entity.TRN = request.TRN;

        entity.CountryId = request.CountryId;
        entity.GovernorateId = request.GovernorateId;
        entity.CityId = request.CityId;
        entity.BuildingNumber = request.BuildingNumber;
        entity.Floor = request.Floor;
        entity.FlatNumber = request.FlatNumber;
        entity.Street = request.Street;
        entity.PostalCode = request.PostalCode;

        entity.Mobile = request.Mobile;

        _repository.Update(entity);
        await _unitOfWork.SaveAsync(cancellationToken);

        return new UpdateCustomerResult
        {
            Data = entity
        };
    }
}

