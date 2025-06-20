using Application.Common.Services.ETAReceiptManager;
using ETA.eReceipt.IntegrationToolkit.Application.Dtos;
using FluentValidation;
using MediatR;

namespace Application.Features.ETAReceiptManager.Queries;

public class GenerateUuidResult
{
    public GenerateUuidResponseDto? Data { get; set; }
}

public class GenerateUuidRequest : IRequest<GenerateUuidResult>
{
    public string? ReceiptJson { get; init; }
}

public class GenerateUuidValidator : AbstractValidator<GenerateUuidRequest>
{
    public GenerateUuidValidator()
    {
        RuleFor(x => x.ReceiptJson).NotEmpty().WithMessage("Receipt JSON is required");
    }
}

public class GenerateUuidHandler : IRequestHandler<GenerateUuidRequest, GenerateUuidResult>
{
    private readonly IETAReceiptService _etaReceiptService;

    public GenerateUuidHandler(IETAReceiptService etaReceiptService)
    {
        _etaReceiptService = etaReceiptService;
    }

    public async Task<GenerateUuidResult> Handle(GenerateUuidRequest request, CancellationToken cancellationToken)
    {
        var response = await _etaReceiptService.GenerateUuidAsync(request.ReceiptJson ?? string.Empty);

        return new GenerateUuidResult
        {
            Data = response
        };
    }
} 