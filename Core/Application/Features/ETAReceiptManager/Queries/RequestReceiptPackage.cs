//using Application.Common.Services.ETAReceiptManager;
//using ETA.eReceipt.IntegrationToolkit.Application.Dtos;
//using FluentValidation;
//using MediatR;

//namespace Application.Features.ETAReceiptManager.Queries;

//public class RequestReceiptPackageResult
//{
//    public RequestReceiptPackageResponseDto? Data { get; set; }
//}

//public class RequestReceiptPackageRequest : IRequest<RequestReceiptPackageResult>
//{
//    public DateTime? FromDate { get; init; }
//    public DateTime? ToDate { get; init; }
//    public string? Format { get; init; } = "JSON"; // JSON or CSV
//    public int? MaxRecords { get; init; }
//}

//public class RequestReceiptPackageValidator : AbstractValidator<RequestReceiptPackageRequest>
//{
//    public RequestReceiptPackageValidator()
//    {
//        RuleFor(x => x.Format)
//            .Must(x => x == "JSON" || x == "CSV")
//            .When(x => !string.IsNullOrEmpty(x.Format))
//            .WithMessage("Format must be either JSON or CSV");
//    }
//}

//public class RequestReceiptPackageHandler : IRequestHandler<RequestReceiptPackageRequest, RequestReceiptPackageResult>
//{
//    private readonly IETAReceiptService _etaReceiptService;

//    public RequestReceiptPackageHandler(IETAReceiptService etaReceiptService)
//    {
//        _etaReceiptService = etaReceiptService;
//    }

//    public async Task<RequestReceiptPackageResult> Handle(RequestReceiptPackageRequest request, CancellationToken cancellationToken)
//    {
//        var requestDto = new RequestReceiptPackageRequestDto
//        {
//            FromDate = request.FromDate,
//            ToDate = request.ToDate,
//            Format = request.Format,
//            MaxRecords = request.MaxRecords
//        };

//        var response = await _etaReceiptService.RequestReceiptPackageAsync(requestDto);

//        return new RequestReceiptPackageResult
//        {
//            Data = response
//        };
//    }
//} 