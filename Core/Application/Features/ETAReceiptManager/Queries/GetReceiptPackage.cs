using Application.Common.Services.ETAReceiptManager;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Features.ETAReceiptManager.Queries;

public class GetReceiptPackageResult
{
    public IActionResult? Data { get; set; }
}

public class GetReceiptPackageRequest : IRequest<GetReceiptPackageResult>
{
    public string? PackageId { get; init; }
}

public class GetReceiptPackageValidator : AbstractValidator<GetReceiptPackageRequest>
{
    public GetReceiptPackageValidator()
    {
        RuleFor(x => x.PackageId).NotEmpty().WithMessage("Package ID is required");
    }
}

public class GetReceiptPackageHandler : IRequestHandler<GetReceiptPackageRequest, GetReceiptPackageResult>
{
    private readonly IETAReceiptService _etaReceiptService;

    public GetReceiptPackageHandler(IETAReceiptService etaReceiptService)
    {
        _etaReceiptService = etaReceiptService;
    }

    public async Task<GetReceiptPackageResult> Handle(GetReceiptPackageRequest request, CancellationToken cancellationToken)
    {
        var response = await _etaReceiptService.GetReceiptPackageAsync(request.PackageId ?? string.Empty);

        return new GetReceiptPackageResult
        {
            Data = response
        };
    }
} 