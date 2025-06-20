using Application.Common.Services.ETAReceiptManager;
using ETA.eReceipt.IntegrationToolkit.Application.Dtos;
using FluentValidation;
using MediatR;

namespace Application.Features.ETAReceiptManager.Commands;

public class SyncSubmissionResult
{
    public SyncSubmissionResponseDto? Data { get; set; }
}

public class SyncSubmissionRequest : IRequest<SyncSubmissionResult>
{
    public bool All { get; init; } = true;
    public List<string>? SubmissionUuids { get; init; }
}

public class SyncSubmissionValidator : AbstractValidator<SyncSubmissionRequest>
{
    public SyncSubmissionValidator()
    {
        // Add validation rules as needed
    }
}

public class SyncSubmissionHandler : IRequestHandler<SyncSubmissionRequest, SyncSubmissionResult>
{
    private readonly IETAReceiptService _etaReceiptService;

    public SyncSubmissionHandler(IETAReceiptService etaReceiptService)
    {
        _etaReceiptService = etaReceiptService;
    }

    public async Task<SyncSubmissionResult> Handle(SyncSubmissionRequest request, CancellationToken cancellationToken)
    {
        var syncSubmissionRequestDto = new SyncSubmissionRequestDto
        {
            All = request.All,
            SubmissionUuids = request.SubmissionUuids
        };

        var response = await _etaReceiptService.SyncSubmissionAsync(syncSubmissionRequestDto);

        return new SyncSubmissionResult
        {
            Data = response
        };
    }
} 