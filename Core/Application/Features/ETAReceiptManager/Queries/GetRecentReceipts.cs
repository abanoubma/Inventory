//using Application.Common.Services.ETAReceiptManager;
//using ETA.eReceipt.IntegrationToolkit.Application.Dtos;
//using FluentValidation;
//using MediatR;

//namespace Application.Features.ETAReceiptManager.Queries;

//public class GetRecentReceiptsResult
//{
//    public GetRecentReceiptsResponseDto? Data { get; set; }
//}

//public class GetRecentReceiptsRequest : IRequest<GetRecentReceiptsResult>
//{
//    public int? MaxCount { get; init; }
//    public DateTime? FromDate { get; init; }
//    public DateTime? ToDate { get; init; }
//}

//public class GetRecentReceiptsValidator : AbstractValidator<GetRecentReceiptsRequest>
//{
//    public GetRecentReceiptsValidator()
//    {
//        RuleFor(x => x.MaxCount)
//            .GreaterThan(0)
//            .When(x => x.MaxCount.HasValue)
//            .WithMessage("Max count must be greater than 0");
//    }
//}

//public class GetRecentReceiptsHandler : IRequestHandler<GetRecentReceiptsRequest, GetRecentReceiptsResult>
//{
//    private readonly IETAReceiptService _etaReceiptService;

//    public GetRecentReceiptsHandler(IETAReceiptService etaReceiptService)
//    {
//        _etaReceiptService = etaReceiptService;
//    }

//    public async Task<GetRecentReceiptsResult> Handle(GetRecentReceiptsRequest request, CancellationToken cancellationToken)
//    {
//        var requestDto = new GetRecentReceiptsRequestDto
//        {
//            MaxCount = request.MaxCount,
//            FromDate = request.FromDate,
//            ToDate = request.ToDate
//        };

//        var response = await _etaReceiptService.GetRecentReceiptsAsync(requestDto);

//        return new GetRecentReceiptsResult
//        {
//            Data = response
//        };
//    }
//} 