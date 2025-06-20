using Application.Common.Services.ETAReceiptManager;
using ETA.eReceipt.IntegrationToolkit.Application.Dtos;
using ETA.eReceipt.IntegrationToolkit.Domain.Enums;
using FluentValidation;
using MediatR;

namespace Application.Features.ETAReceiptManager.Queries;

public class SearchReceiptsResult
{
    public SearchReceiptsResponseDto? Data { get; set; }
}

public class SearchReceiptsRequest : IRequest<SearchReceiptsResult>
{
    public string? BuyerId { get; init; }
    public string? BuyerType { get; init; }
    public string? ReceiptNumber { get; init; }
    public string? Uuid { get; init; }
    public string? ReferenceUuid { get; init; }
    public string? SubmissionUuid { get; init; }
    public DateTime? DateTimeIssued { get; init; }
    public ReceiptStatus? LocalStatus { get; init; }
    public decimal? TotalAmountEGP { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class SearchReceiptsValidator : AbstractValidator<SearchReceiptsRequest>
{
    public SearchReceiptsValidator()
    {
        RuleFor(x => x.PageNumber).GreaterThan(0).WithMessage("Page number must be greater than 0");
        RuleFor(x => x.PageSize).GreaterThan(0).WithMessage("Page size must be greater than 0");
    }
}

public class SearchReceiptsHandler : IRequestHandler<SearchReceiptsRequest, SearchReceiptsResult>
{
    private readonly IETAReceiptService _etaReceiptService;

    public SearchReceiptsHandler(IETAReceiptService etaReceiptService)
    {
        _etaReceiptService = etaReceiptService;
    }

    public async Task<SearchReceiptsResult> Handle(SearchReceiptsRequest request, CancellationToken cancellationToken)
    {
        var searchReceiptsRequestDto = new SearchReceiptsRequestDto
        {
            BuyerId = request.BuyerId,
            BuyerType = request.BuyerType,
            ReceiptNumber = request.ReceiptNumber,
            Uuid = request.Uuid,
            ReferenceUuid = request.ReferenceUuid,
            SubmissionUuid = request.SubmissionUuid,
            DateTimeIssued = request.DateTimeIssued,
            LocalStatus = request.LocalStatus,
            TotalAmountEGP = request.TotalAmountEGP,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };

        var response = await _etaReceiptService.SearchReceiptsAsync(searchReceiptsRequestDto);

        return new SearchReceiptsResult
        {
            Data = response
        };
    }
} 