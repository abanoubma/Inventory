using Application.Common.Services.ETAReceiptManager;
using ETA.eReceipt.IntegrationToolkit.Application.Dtos;
using FluentValidation;
using MediatR;

namespace Application.Features.ETAReceiptManager.Queries;

public class GenerateQrCodeResult
{
    public GenerateQrCodeResponseDto? Data { get; set; }
}

public class GenerateQrCodeRequest : IRequest<GenerateQrCodeResult>
{
    public string? ReceiptWithUuid { get; init; }
}

public class GenerateQrCodeValidator : AbstractValidator<GenerateQrCodeRequest>
{
    public GenerateQrCodeValidator()
    {
        RuleFor(x => x.ReceiptWithUuid).NotEmpty().WithMessage("Receipt with UUID is required");
    }
}

public class GenerateQrCodeHandler : IRequestHandler<GenerateQrCodeRequest, GenerateQrCodeResult>
{
    private readonly IETAReceiptService _etaReceiptService;

    public GenerateQrCodeHandler(IETAReceiptService etaReceiptService)
    {
        _etaReceiptService = etaReceiptService;
    }

    public async Task<GenerateQrCodeResult> Handle(GenerateQrCodeRequest request, CancellationToken cancellationToken)
    {
        var response = await _etaReceiptService.GenerateQrCodeAsync(request.ReceiptWithUuid ?? string.Empty);

        return new GenerateQrCodeResult
        {
            Data = response
        };
    }
} 