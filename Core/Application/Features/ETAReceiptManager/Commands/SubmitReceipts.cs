using Application.Common.Services.ETAReceiptManager;
using ETA.eReceipt.IntegrationToolkit.Application.Dtos;
using FluentValidation;
using MediatR;

namespace Application.Features.ETAReceiptManager.Commands;

public class SubmitReceiptsResult
{
    public SubmitReceiptsResponseDto? Data { get; set; }
}

public class SubmitReceiptsRequest : IRequest<SubmitReceiptsResult>
{
    public int ReceiptCount { get; init; }
}

public class SubmitReceiptsValidator : AbstractValidator<SubmitReceiptsRequest>
{
    public SubmitReceiptsValidator()
    {
        RuleFor(x => x.ReceiptCount).GreaterThan(0).WithMessage("Receipt count must be greater than 0");
    }
}

public class SubmitReceiptsHandler : IRequestHandler<SubmitReceiptsRequest, SubmitReceiptsResult>
{
    private readonly IETAReceiptService _etaReceiptService;

    public SubmitReceiptsHandler(IETAReceiptService etaReceiptService)
    {
        _etaReceiptService = etaReceiptService;
    }

    public async Task<SubmitReceiptsResult> Handle(SubmitReceiptsRequest request, CancellationToken cancellationToken)
    {
        var submitReceiptsRequestDto = new SubmitReceiptsRequestDto
        {
            ReceiptCount = request.ReceiptCount
        };

        var response = await _etaReceiptService.SubmitReceiptsAsync(submitReceiptsRequestDto);

        return new SubmitReceiptsResult
        {
            Data = response
        };
    }
} 