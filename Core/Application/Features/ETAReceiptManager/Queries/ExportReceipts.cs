using Application.Common.Services.ETAReceiptManager;
using ETA.eReceipt.IntegrationToolkit.Application.Dtos;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Features.ETAReceiptManager.Queries;

public class ExportReceiptsResult
{
    public IActionResult? Data { get; set; }
}

public class ExportReceiptsRequest : IRequest<ExportReceiptsResult>
{
    public int ReceiptCount { get; init; }
}

public class ExportReceiptsValidator : AbstractValidator<ExportReceiptsRequest>
{
    public ExportReceiptsValidator()
    {
        RuleFor(x => x.ReceiptCount).GreaterThan(0).WithMessage("Receipt count must be greater than 0");
    }
}

public class ExportReceiptsHandler : IRequestHandler<ExportReceiptsRequest, ExportReceiptsResult>
{
    private readonly IETAReceiptService _etaReceiptService;

    public ExportReceiptsHandler(IETAReceiptService etaReceiptService)
    {
        _etaReceiptService = etaReceiptService;
    }

    public async Task<ExportReceiptsResult> Handle(ExportReceiptsRequest request, CancellationToken cancellationToken)
    {
        var exportReceiptsRequestDto = new ExportReceiptsRequestDto
        {
            ReceiptCount = request.ReceiptCount
        };

        var response = await _etaReceiptService.ExportReceiptsAsync(exportReceiptsRequestDto);

        return new ExportReceiptsResult
        {
            Data = response
        };
    }
} 