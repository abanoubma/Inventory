using Application.Common.Services.ETAReceiptManager;
using ETA.eReceipt.IntegrationToolkit.Application.Dtos;
using ETA.eReceipt.IntegrationToolkit.Application.Services;
using Infrastructure.ETAReceiptManager.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.ETAReceiptManager;

public class ETAReceiptService : IETAReceiptService
{
    private readonly IToolkitHandler _toolkitHandler;
    private readonly IJsonHelper _jsonHelper;
    private readonly ILogger<ETAReceiptService> _logger;
    private readonly ETAToolkitConfiguration _configuration;
    private bool _isInitialized = false;
    private bool _isAuthenticated = false;

    public ETAReceiptService(
        IToolkitHandler toolkitHandler,
        IJsonHelper jsonHelper,
        ILogger<ETAReceiptService> logger,
        IOptions<ETAToolkitConfiguration> configuration)
    {
        _toolkitHandler = toolkitHandler;
        _jsonHelper = jsonHelper;
        _logger = logger;
        _configuration = configuration.Value;
    }

    public async Task<InitializeResponseDto> InitializeAsync(InitializeRequestDto? request = null)
    {
        try
        {
            _logger.LogInformation("Initializing ETA Receipt Toolkit for environment: {Environment}", _configuration.Environment);
            
            var initializeRequest = request ?? CreateInitializeRequestFromConfiguration();
            
            var response = await _toolkitHandler.Initialize(initializeRequest);
            
            _isInitialized = IsSuccessResponse(response.Code, response.Message);
            
            _logger.LogInformation("ETA Receipt Toolkit initialized. Code: {Code}, Message: {Message}", 
                response.Code, response.Message);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to initialize ETA Receipt Toolkit");
            throw;
        }
    }

    public async Task<AuthenticateResponseDto> AuthenticateAsync(AuthenticateRequestDto? request = null)
    {
        try
        {
            // Ensure toolkit is initialized first
            if (!_isInitialized)
            {
                await InitializeAsync();
            }

            _logger.LogInformation("Authenticating with ETA Receipt Toolkit using POS Serial: {PosSerial}", 
                _configuration.Credentials.PosSerial);
            
            var authRequest = request ?? CreateAuthRequestFromConfiguration();
            
            var response = await _toolkitHandler.Authenticate(authRequest);
            
            _isAuthenticated = IsSuccessResponse(response.Code, response.Message);
            
            _logger.LogInformation("Authentication completed. Code: {Code}, Message: {Message}", 
                response.Code, response.Message);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to authenticate with ETA Receipt Toolkit");
            throw;
        }
    }

    public async Task<RefreshCacheResponseDto> RefreshCacheAsync()
    {
        try
        {
            await EnsureAuthenticatedAsync();
            
            _logger.LogInformation("Refreshing ETA Receipt Toolkit local cache");
            var response = await _toolkitHandler.RefreshCache();
            _logger.LogInformation("Local cache refreshed. Code: {Code}, Message: {Message}", 
                response.Code, response.Message);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to refresh ETA Receipt Toolkit cache");
            throw;
        }
    }

    public async Task<GenerateUuidResponseDto> GenerateUuidAsync(string receiptJson)
    {
        try
        {
            await EnsureAuthenticatedAsync();
            
            _logger.LogInformation("Generating UUID for receipt");
            var response = await _toolkitHandler.GenerateUuid(receiptJson);
            _logger.LogInformation("UUID generation completed. Code: {Code}", response.Code);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to generate UUID for receipt");
            throw;
        }
    }

    public async Task<GenerateQrCodeResponseDto> GenerateQrCodeAsync(string receiptWithUuid)
    {
        try
        {
            await EnsureAuthenticatedAsync();
            
            _logger.LogInformation("Generating QR code for receipt");
            var response = await _toolkitHandler.GenerateQrCode(receiptWithUuid);
            _logger.LogInformation("QR code generation completed. Code: {Code}", response.Code);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to generate QR code for receipt");
            throw;
        }
    }

    public async Task<IssueReceiptResponseDto> IssueReceiptAsync(string receiptToIssue)
    {
        try
        {
            await EnsureAuthenticatedAsync();
            
            _logger.LogInformation("Issuing receipt through ETA");
            var response = await _toolkitHandler.IssueReceipt(receiptToIssue);
            _logger.LogInformation("Receipt issuance completed. Code: {Code}", response.Code);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to issue receipt through ETA");
            throw;
        }
    }

    public async Task<SubmitReceiptsResponseDto> SubmitReceiptsAsync(SubmitReceiptsRequestDto request)
    {
        try
        {
            await EnsureAuthenticatedAsync();
            
            _logger.LogInformation("Submitting {ReceiptCount} receipts to ETA", request.ReceiptCount);
            var response = await _toolkitHandler.SubmitReceipts(request);
            _logger.LogInformation("Receipt submission completed. Code: {Code}", response.Code);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to submit receipts to ETA");
            throw;
        }
    }

    public async Task<SyncSubmissionResponseDto> SyncSubmissionAsync(SyncSubmissionRequestDto request)
    {
        try
        {
            await EnsureAuthenticatedAsync();
            
            _logger.LogInformation("Syncing submissions with ETA");
            var response = await _toolkitHandler.SyncSubmission(request);
            _logger.LogInformation("Submission sync completed. Code: {Code}", response.Code);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to sync submissions with ETA");
            throw;
        }
    }

    public async Task<IActionResult> ExportReceiptsAsync(ExportReceiptsRequestDto request)
    {
        try
        {
            await EnsureAuthenticatedAsync();
            
            _logger.LogInformation("Exporting {ReceiptCount} receipts from ETA", request.ReceiptCount);
            var response = await _toolkitHandler.ExportReceipts(request);
            _logger.LogInformation("Receipt export completed");
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to export receipts from ETA");
            throw;
        }
    }

    public async Task<SearchReceiptsResponseDto> SearchReceiptsAsync(SearchReceiptsRequestDto request)
    {
        try
        {
            await EnsureAuthenticatedAsync();
            
            _logger.LogInformation("Searching receipts in local store");
            var response = await _toolkitHandler.SearchReceipts(request);
            _logger.LogInformation("Receipt search completed. Code: {Code}", response.Code);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to search receipts");
            throw;
        }
    }

    private async Task EnsureAuthenticatedAsync()
    {
        if (!_isAuthenticated)
        {
            await AuthenticateAsync();
        }
    }

    private static bool IsSuccessResponse(string? code, string? message)
    {
        if (string.IsNullOrEmpty(code) || code == "0" || code.Equals("success", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        if (!string.IsNullOrEmpty(message))
        {
            var successMessages = new[] { "success", "completed", "initialized", "authenticated" };
            return successMessages.Any(s => message.Contains(s, StringComparison.OrdinalIgnoreCase));
        }

        return false;
    }

    private InitializeRequestDto CreateInitializeRequestFromConfiguration()
    {
        var settings = _configuration.Initialize;
        
        return new InitializeRequestDto
        {
            SaveCredential = settings.SaveCredential,
            ResumeWithInvalidCache = settings.ResumeWithInvalidCache,
            MaximumSubmissionDocumentCount = settings.MaximumSubmissionDocumentCount,
            CachLookupDurationInHours = settings.CachLookupDurationInHours,
            RetentionSchedule = null,
            SubmitSchedule = null,
            SyncSchedule = null
        };
    }

    private AuthenticateRequestDto CreateAuthRequestFromConfiguration()
    {
        var credentials = _configuration.Credentials;
        
        return new AuthenticateRequestDto
        {
            ClientId = credentials.ClientId,
            ClientSecret = credentials.ClientSecret,
            PosSerial = credentials.PosSerial,
            PosOsVersion = credentials.PosOsVersion,
            PosModelFramework = credentials.PosModelFramework,
            PresharedKey = credentials.PresharedKey
        };
    }
} 