//using Application.Common.Services.ETAReceiptManager;
//using ETA.eReceipt.IntegrationToolkit.Application.Dtos;
//using FluentValidation;
//using MediatR;

//namespace Application.Features.ETAReceiptManager.Queries;

//public class GetReceiptResult
//{
//    public GetReceiptResponseDto? Data { get; set; }
//}

//public class GetReceiptRequest : IRequest<GetReceiptResult>
//{
//    public string? ReceiptId { get; init; }
//}

//public class GetReceiptValidator : AbstractValidator<GetReceiptRequest>
//{
//    public GetReceiptValidator()
//    {
//        RuleFor(x => x.ReceiptId).NotEmpty().WithMessage("Receipt ID is required");
//    }
//}

//public class GetReceiptHandler : IRequestHandler<GetReceiptRequest, GetReceiptResult>
//{
//    private readonly IETAReceiptService _etaReceiptService;

//    public GetReceiptHandler(IETAReceiptService etaReceiptService)
//    {
//        _etaReceiptService = etaReceiptService;
//    }

//    public async Task<GetReceiptResult> Handle(GetReceiptRequest request, CancellationToken cancellationToken)
//    {
//        var response = await _etaReceiptService.GetReceiptAsync(request.ReceiptId ?? string.Empty);

//        return new GetReceiptResult
//        {
//            Data = response
//        };
//    }
//} 