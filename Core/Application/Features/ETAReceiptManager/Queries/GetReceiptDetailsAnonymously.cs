//using Application.Common.Services.ETAReceiptManager;
//using ETA.eReceipt.IntegrationToolkit.Application.Dtos;
//using FluentValidation;
//using MediatR;

//namespace Application.Features.ETAReceiptManager.Queries;

//public class GetReceiptDetailsAnonymouslyResult
//{
//    public GetReceiptDetailsResponseDto? Data { get; set; }
//}

//public class GetReceiptDetailsAnonymouslyRequest : IRequest<GetReceiptDetailsAnonymouslyResult>
//{
//    public string? ReceiptId { get; init; }
//}

//public class GetReceiptDetailsAnonymouslyValidator : AbstractValidator<GetReceiptDetailsAnonymouslyRequest>
//{
//    public GetReceiptDetailsAnonymouslyValidator()
//    {
//        RuleFor(x => x.ReceiptId).NotEmpty().WithMessage("Receipt ID is required");
//    }
//}

//public class GetReceiptDetailsAnonymouslyHandler : IRequestHandler<GetReceiptDetailsAnonymouslyRequest, GetReceiptDetailsAnonymouslyResult>
//{
//    private readonly IETAReceiptService _etaReceiptService;

//    public GetReceiptDetailsAnonymouslyHandler(IETAReceiptService etaReceiptService)
//    {
//        _etaReceiptService = etaReceiptService;
//    }

//    public async Task<GetReceiptDetailsAnonymouslyResult> Handle(GetReceiptDetailsAnonymouslyRequest request, CancellationToken cancellationToken)
//    {
//        var response = await _etaReceiptService.GetReceiptDetailsAnonymouslyAsync(request.ReceiptId ?? string.Empty);

//        return new GetReceiptDetailsAnonymouslyResult
//        {
//            Data = response
//        };
//    }
//} 