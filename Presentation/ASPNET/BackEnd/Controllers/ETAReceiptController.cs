using Application.Features.ETAReceiptManager.Commands;
using Application.Features.ETAReceiptManager.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASPNET.BackEnd.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ETAReceiptController : ControllerBase
{
    private readonly IMediator _mediator;

    public ETAReceiptController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("initialize")]
    public async Task<IActionResult> Initialize([FromBody] InitializeETARequest request)
    {
        var result = await _mediator.Send(request);
        return Ok(result.Data);
    }

    [HttpPost("authenticate")]
    public async Task<IActionResult> Authenticate([FromBody] AuthenticateETARequest request)
    {
        var result = await _mediator.Send(request);
        return Ok(result.Data);
    }

    [HttpPost("refresh-cache")]
    public async Task<IActionResult> RefreshCache()
    {
        var result = await _mediator.Send(new RefreshCacheRequest());
        return Ok(result.Data);
    }

    [HttpPost("generate-uuid")]
    public async Task<IActionResult> GenerateUuid([FromBody] GenerateUuidRequest request)
    {
        var result = await _mediator.Send(request);
        return Ok(result.Data);
    }

    [HttpPost("generate-qr-code")]
    public async Task<IActionResult> GenerateQrCode([FromBody] GenerateQrCodeRequest request)
    {
        var result = await _mediator.Send(request);
        return Ok(result.Data);
    }

    [HttpPost("issue-receipt")]
    public async Task<IActionResult> IssueReceipt([FromBody] IssueReceiptRequest request)
    {
        var result = await _mediator.Send(request);
        return Ok(result.Data);
    }

    [HttpPost("submit-receipts")]
    public async Task<IActionResult> SubmitReceipts([FromBody] SubmitReceiptsRequest request)
    {
        var result = await _mediator.Send(request);
        return Ok(result.Data);
    }

    [HttpPost("sync-submission")]
    public async Task<IActionResult> SyncSubmission([FromBody] SyncSubmissionRequest request)
    {
        var result = await _mediator.Send(request);
        return Ok(result.Data);
    }

    [HttpPost("export-receipts")]
    public async Task<IActionResult> ExportReceipts([FromBody] ExportReceiptsRequest request)
    {
        var result = await _mediator.Send(request);
        return result.Data ?? Ok();
    }

    [HttpPost("search-receipts")]
    public async Task<IActionResult> SearchReceipts([FromBody] SearchReceiptsRequest request)
    {
        var result = await _mediator.Send(request);
        return Ok(result.Data);
    }

    //[HttpGet("details/{receiptId}")]
    //public async Task<IActionResult> GetReceiptDetails(string receiptId)
    //{
    //    var result = await _mediator.Send(new GetReceiptDetailsRequest { ReceiptId = receiptId });
    //    return Ok(result.Data);
    //}

    //[HttpGet("{receiptId}")]
    //public async Task<IActionResult> GetReceipt(string receiptId)
    //{
    //    var result = await _mediator.Send(new GetReceiptRequest { ReceiptId = receiptId });
    //    return Ok(result.Data);
    //}

    //[AllowAnonymous]
    //[HttpGet("details-anonymous/{receiptId}")]
    //public async Task<IActionResult> GetReceiptDetailsAnonymously(string receiptId)
    //{
    //    var result = await _mediator.Send(new GetReceiptDetailsAnonymouslyRequest { ReceiptId = receiptId });
    //    return Ok(result.Data);
    //}

    //[HttpGet("submission/{submissionId}")]
    //public async Task<IActionResult> GetReceiptSubmission(string submissionId)
    //{
    //    var result = await _mediator.Send(new GetReceiptSubmissionRequest { SubmissionId = submissionId });
    //    return Ok(result.Data);
    //}

    //[HttpPost("recent")]
    //public async Task<IActionResult> GetRecentReceipts([FromBody] GetRecentReceiptsRequest request)
    //{
    //    var result = await _mediator.Send(request);
    //    return Ok(result.Data);
    //}

    //[HttpPost("request-package")]
    //public async Task<IActionResult> RequestReceiptPackage([FromBody] RequestReceiptPackageRequest request)
    //{
    //    var result = await _mediator.Send(request);
    //    return Ok(result.Data);
    //}

    //[HttpGet("package-requests")]
    //public async Task<IActionResult> GetPackageRequests()
    //{
    //    var result = await _mediator.Send(new GetPackageRequestsRequest());
    //    return Ok(result.Data);
    //}

    //[HttpGet("package/{packageId}")]
    //public async Task<IActionResult> GetReceiptPackage(string packageId)
    //{
    //    var result = await _mediator.Send(new GetReceiptPackageRequest { PackageId = packageId });
    //    return result.Data ?? Ok();
    //}
} 