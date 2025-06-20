//using Application.Common.Services.ETAReceiptManager;
//using ETA.eReceipt.IntegrationToolkit.Application.Dtos;
//using MediatR;

//namespace Application.Features.ETAReceiptManager.Queries;

//public class GetPackageRequestsResult
//{
//    public GetPackageRequestsResponseDto? Data { get; set; }
//}

//public class GetPackageRequestsRequest : IRequest<GetPackageRequestsResult>
//{
//    // No parameters needed for this endpoint
//}

//public class GetPackageRequestsHandler : IRequestHandler<GetPackageRequestsRequest, GetPackageRequestsResult>
//{
//    private readonly IETAReceiptService _etaReceiptService;

//    public GetPackageRequestsHandler(IETAReceiptService etaReceiptService)
//    {
//        _etaReceiptService = etaReceiptService;
//    }

//    public async Task<GetPackageRequestsResult> Handle(GetPackageRequestsRequest request, CancellationToken cancellationToken)
//    {
//        var response = await _etaReceiptService.GetPackageRequestsAsync();

//        return new GetPackageRequestsResult
//        {
//            Data = response
//        };
//    }
//} 