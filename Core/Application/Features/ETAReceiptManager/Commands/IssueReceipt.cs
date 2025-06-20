using Application.Common.Services.ETAReceiptManager;
using ETA.eReceipt.IntegrationToolkit.Application.Dtos;
using FluentValidation;
using MediatR;

namespace Application.Features.ETAReceiptManager.Commands;

public class IssueReceiptResult
{
    public IssueReceiptResponseDto? Data { get; set; }
}

public class IssueReceiptRequest : IRequest<IssueReceiptResult>
{
    public string? ReceiptJson { get; init; }
}

public class IssueReceiptValidator : AbstractValidator<IssueReceiptRequest>
{
    public IssueReceiptValidator()
    {
        RuleFor(x => x.ReceiptJson).NotEmpty().WithMessage("Receipt JSON is required");
    }
}

public class IssueReceiptHandler : IRequestHandler<IssueReceiptRequest, IssueReceiptResult>
{
    private readonly IETAReceiptService _etaReceiptService;

    public IssueReceiptHandler(IETAReceiptService etaReceiptService)
    {
        _etaReceiptService = etaReceiptService;
    }

    public async Task<IssueReceiptResult> Handle(IssueReceiptRequest request, CancellationToken cancellationToken)
    {
        var response = await _etaReceiptService.IssueReceiptAsync(request.ReceiptJson ?? string.Empty);

        return new IssueReceiptResult
        {
            Data = response
        };
    }
} 