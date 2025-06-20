using Application.Common.Services.ETAReceiptManager;
using ETA.eReceipt.IntegrationToolkit.Application.Dtos;
using MediatR;

namespace Application.Features.ETAReceiptManager.Queries;

public class RefreshCacheResult
{
    public RefreshCacheResponseDto? Data { get; set; }
}

public class RefreshCacheRequest : IRequest<RefreshCacheResult>
{
    // No parameters needed for refresh cache
}

public class RefreshCacheHandler : IRequestHandler<RefreshCacheRequest, RefreshCacheResult>
{
    private readonly IETAReceiptService _etaReceiptService;

    public RefreshCacheHandler(IETAReceiptService etaReceiptService)
    {
        _etaReceiptService = etaReceiptService;
    }

    public async Task<RefreshCacheResult> Handle(RefreshCacheRequest request, CancellationToken cancellationToken)
    {
        var response = await _etaReceiptService.RefreshCacheAsync();

        return new RefreshCacheResult
        {
            Data = response
        };
    }
} 