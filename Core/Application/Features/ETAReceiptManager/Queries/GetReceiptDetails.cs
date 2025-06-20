//using Application.Common.Services.ETAReceiptManager;
//using ETA.eReceipt.IntegrationToolkit.Application.Dtos;
//using FluentValidation;
//using MediatR;

//namespace Application.Features.ETAReceiptManager.Queries;

//public class GetReceiptDetailsResult
//{
//    public GetReceiptDetailsResponseDto? Data { get; set; }
//}

//public class GetReceiptDetailsRequest : IRequest<GetReceiptDetailsResult>
//{
//    public string? ReceiptId { get; init; }
//}

//public class GetReceiptDetailsValidator : AbstractValidator<GetReceiptDetailsRequest>
//{
//    public GetReceiptDetailsValidator()
//    {
//        RuleFor(x => x.ReceiptId).NotEmpty().WithMessage("Receipt ID is required");
//    }
//}

//public class GetReceiptDetailsHandler : IRequestHandler<GetReceiptDetailsRequest, GetReceiptDetailsResult>
//{
//    private readonly IETAReceiptService _etaReceiptService;

//    public GetReceiptDetailsHandler(IETAReceiptService etaReceiptService)
//    {
//        _etaReceiptService = etaReceiptService;
//    }

//    public async Task<GetReceiptDetailsResult> Handle(GetReceiptDetailsRequest request, CancellationToken cancellationToken)
//    {
//        var response = await _etaReceiptService.GetReceiptDetailsAsync(request.ReceiptId ?? string.Empty);

//        return new GetReceiptDetailsResult
//        {
//            Data = response
//        };
//    }
//} 