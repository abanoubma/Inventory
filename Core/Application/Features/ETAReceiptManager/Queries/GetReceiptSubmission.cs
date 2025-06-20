//using Application.Common.Services.ETAReceiptManager;
//using ETA.eReceipt.IntegrationToolkit.Application.Dtos;
//using FluentValidation;
//using MediatR;

//namespace Application.Features.ETAReceiptManager.Queries;

//public class GetReceiptSubmissionResult
//{
//    public GetReceiptSubmissionResponseDto? Data { get; set; }
//}

//public class GetReceiptSubmissionRequest : IRequest<GetReceiptSubmissionResult>
//{
//    public string? SubmissionId { get; init; }
//}

//public class GetReceiptSubmissionValidator : AbstractValidator<GetReceiptSubmissionRequest>
//{
//    public GetReceiptSubmissionValidator()
//    {
//        RuleFor(x => x.SubmissionId).NotEmpty().WithMessage("Submission ID is required");
//    }
//}

//public class GetReceiptSubmissionHandler : IRequestHandler<GetReceiptSubmissionRequest, GetReceiptSubmissionResult>
//{
//    private readonly IETAReceiptService _etaReceiptService;

//    public GetReceiptSubmissionHandler(IETAReceiptService etaReceiptService)
//    {
//        _etaReceiptService = etaReceiptService;
//    }

//    public async Task<GetReceiptSubmissionResult> Handle(GetReceiptSubmissionRequest request, CancellationToken cancellationToken)
//    {
//        var response = await _etaReceiptService.GetReceiptSubmissionAsync(request.SubmissionId ?? string.Empty);

//        return new GetReceiptSubmissionResult
//        {
//            Data = response
//        };
//    }
//} 