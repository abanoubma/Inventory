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

    [HttpPost("test-credentials")]
    public async Task<IActionResult> TestCredentials()
    {
        try
        {
            // Test initialization with the credentials from screenshot
            var initResult = await _mediator.Send(new InitializeETARequest());
            
            if (initResult.Data == null)
            {
                return BadRequest(new { Status = "Error", Message = "Initialize failed - no response data" });
            }

            // Test authentication
            var authResult = await _mediator.Send(new AuthenticateETARequest());
            
            if (authResult.Data == null)
            {
                return BadRequest(new { Status = "Error", Message = "Authentication failed - no response data" });
            }

            return Ok(new { 
                Status = "Success", 
                Message = "Credentials from screenshot are working!",
                Initialize = new {
                    Code = initResult.Data.Code,
                    Message = initResult.Data.Message
                },
                Authenticate = new {
                    Code = authResult.Data.Code,
                    Message = authResult.Data.Message
                }
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { 
                Status = "Error", 
                Message = ex.Message,
                Details = ex.InnerException?.Message 
            });
        }
    }

    [HttpPost("full-workflow-test")]
    public async Task<IActionResult> FullWorkflowTest()
    {
        try
        {
            var results = new Dictionary<string, object>();

            // 1. Initialize
            var initResult = await _mediator.Send(new InitializeETARequest());
            results.Add("Initialize", new { 
                Code = initResult.Data?.Code, 
                Message = initResult.Data?.Message,
                Success = IsSuccess(initResult.Data?.Code)
            });

            // 2. Authenticate
            var authResult = await _mediator.Send(new AuthenticateETARequest());
            results.Add("Authenticate", new { 
                Code = authResult.Data?.Code, 
                Message = authResult.Data?.Message,
                Success = IsSuccess(authResult.Data?.Code)
            });

            // 3. Refresh Cache
            var cacheResult = await _mediator.Send(new RefreshCacheRequest());
            results.Add("RefreshCache", new { 
                Code = cacheResult.Data?.Code, 
                Message = cacheResult.Data?.Message,
                Success = IsSuccess(cacheResult.Data?.Code)
            });

            // 4. Test UUID Generation (with sample receipt JSON)
            try
            {
                var sampleReceiptJson = """
                {
                    "receiptType": "R",
                    "header": {
                        "dateTimeIssued": "2024-01-15T10:30:00Z",
                        "receiptNumber": "REC001"
                    }
                }
                """;

                var uuidResult = await _mediator.Send(new GenerateUuidRequest 
                { 
                    ReceiptJson = sampleReceiptJson 
                });
                results.Add("GenerateUUID", new { 
                    Code = uuidResult.Data?.Code, 
                    HasUUID = !string.IsNullOrEmpty(uuidResult.Data?.UpdatedReceiptJson?.ToString()),
                    Success = IsSuccess(uuidResult.Data?.Code)
                });
            }
            catch (Exception ex)
            {
                results.Add("GenerateUUID", new { Error = ex.Message });
            }

            return Ok(new { 
                Status = "Workflow Test Complete", 
                Results = results,
                OverallSuccess = results.Values.All(r => 
                {
                    if (r.GetType().GetProperty("Success") != null)
                        return (bool)r.GetType().GetProperty("Success")!.GetValue(r)!;
                    return r.GetType().GetProperty("Error") == null;
                })
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { 
                Status = "Workflow Test Failed", 
                Message = ex.Message 
            });
        }
    }

    private static bool IsSuccess(string? code)
    {
        return string.IsNullOrEmpty(code) || 
               code == "0" || 
               code.Equals("success", StringComparison.OrdinalIgnoreCase);
    }
} 