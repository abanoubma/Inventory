using Application.Common.Services.ETAReceiptManager;
using ETA.eReceipt.IntegrationToolkit.Application.Dtos;
using FluentValidation;
using MediatR;

namespace Application.Features.ETAReceiptManager.Commands;

public class InitializeETAResult
{
    public InitializeResponseDto? Data { get; set; }
}

public class InitializeETARequest : IRequest<InitializeETAResult>
{
    public bool? SaveCredential { get; init; }
    public bool? ResumeWithInvalidCache { get; init; }
    public int? MaximumSubmissionDocumentCount { get; init; }
    public float? CachLookupDurationInHours { get; init; }
    public RetentionScheduleDto? RetentionSchedule { get; init; }
    public ScheduleDto? SubmitSchedule { get; init; }
    public ScheduleDto? SyncSchedule { get; init; }
}

public class InitializeETAValidator : AbstractValidator<InitializeETARequest>
{
    public InitializeETAValidator()
    {
        // Add validation rules as needed
    }
}

public class InitializeETAHandler : IRequestHandler<InitializeETARequest, InitializeETAResult>
{
    private readonly IETAReceiptService _etaReceiptService;

    public InitializeETAHandler(IETAReceiptService etaReceiptService)
    {
        _etaReceiptService = etaReceiptService;
    }

    public async Task<InitializeETAResult> Handle(InitializeETARequest request, CancellationToken cancellationToken)
    {
        var initializeRequestDto = new InitializeRequestDto
        {
            SaveCredential = request.SaveCredential,
            ResumeWithInvalidCache = request.ResumeWithInvalidCache,
            MaximumSubmissionDocumentCount = request.MaximumSubmissionDocumentCount,
            CachLookupDurationInHours = request.CachLookupDurationInHours,
            RetentionSchedule = request.RetentionSchedule,
            SubmitSchedule = request.SubmitSchedule,
            SyncSchedule = request.SyncSchedule
        };

        var response = await _etaReceiptService.InitializeAsync(initializeRequestDto);

        return new InitializeETAResult
        {
            Data = response
        };
    }
} 