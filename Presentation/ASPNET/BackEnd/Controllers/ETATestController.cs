using Application.Features.ETAReceiptManager.Commands;
using Application.Features.ETAReceiptManager.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASPNET.BackEnd.Controllers;

[Authorize]
[ApiController]
[Route("api/eta-test")]
public class ETATestController : ControllerBase
{
    private readonly IMediator _mediator;

    public ETATestController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("initialize")]
    public async Task<IActionResult> TestInitialize()
    {
        try
        {
            var result = await _mediator.Send(new InitializeETARequest());
            return Ok(new { Status = "Success", Data = result.Data });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Status = "Error", Message = ex.Message });
        }
    }

    [HttpPost("authenticate")]
    public async Task<IActionResult> TestAuthenticate()
    {
        try
        {
            var result = await _mediator.Send(new AuthenticateETARequest());
            return Ok(new { Status = "Success", Data = result.Data });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Status = "Error", Message = ex.Message });
        }
    }

    [HttpPost("refresh-cache")]
    public async Task<IActionResult> TestRefreshCache()
    {
        try
        {
            var result = await _mediator.Send(new RefreshCacheRequest());
            return Ok(new { Status = "Success", Data = result.Data });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Status = "Error", Message = ex.Message });
        }
    }

    [HttpPost("full-test")]
    public async Task<IActionResult> FullTest()
    {
        try
        {
            // Test initialization
            var initResult = await _mediator.Send(new InitializeETARequest());
            
            // Test authentication
            var authResult = await _mediator.Send(new AuthenticateETARequest());
            
            // Test cache refresh
            var cacheResult = await _mediator.Send(new RefreshCacheRequest());
            
            return Ok(new { 
                Status = "Success", 
                Tests = new {
                    Initialize = initResult.Data,
                    Authenticate = authResult.Data,
                    RefreshCache = cacheResult.Data
                }
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Status = "Error", Message = ex.Message });
        }
    }
} 