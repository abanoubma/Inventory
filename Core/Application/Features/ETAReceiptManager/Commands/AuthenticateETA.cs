using Application.Common.Services.ETAReceiptManager;
using ETA.eReceipt.IntegrationToolkit.Application.Dtos;
using FluentValidation;
using MediatR;

namespace Application.Features.ETAReceiptManager.Commands;

public class AuthenticateETAResult
{
    public AuthenticateResponseDto? Data { get; set; }
}

public class AuthenticateETARequest : IRequest<AuthenticateETAResult>
{
    public string? ClientId { get; init; }
    public string? ClientSecret { get; init; }
    public string? PosSerial { get; init; }
    public string? PosOsVersion { get; init; }
    public string? PosModelFramework { get; init; }
    public string? PresharedKey { get; init; }
}

public class AuthenticateETAValidator : AbstractValidator<AuthenticateETARequest>
{
    public AuthenticateETAValidator()
    {
        // Add validation rules as needed
    }
}

public class AuthenticateETAHandler : IRequestHandler<AuthenticateETARequest, AuthenticateETAResult>
{
    private readonly IETAReceiptService _etaReceiptService;

    public AuthenticateETAHandler(IETAReceiptService etaReceiptService)
    {
        _etaReceiptService = etaReceiptService;
    }

    public async Task<AuthenticateETAResult> Handle(AuthenticateETARequest request, CancellationToken cancellationToken)
    {
        var authenticateRequestDto = string.IsNullOrWhiteSpace(request.ClientId)
            && string.IsNullOrWhiteSpace(request.ClientSecret)
            && string.IsNullOrWhiteSpace(request.PosSerial)
            && string.IsNullOrWhiteSpace(request.PosOsVersion)
            && string.IsNullOrWhiteSpace(request.PosModelFramework)
            && string.IsNullOrWhiteSpace(request.PresharedKey)
            ? null
            : new AuthenticateRequestDto
            {
                ClientId = request.ClientId,
                ClientSecret = request.ClientSecret,
                PosSerial = request.PosSerial,
                PosOsVersion = request.PosOsVersion,
                PosModelFramework = request.PosModelFramework,
                PresharedKey = request.PresharedKey
            };

        var response = await _etaReceiptService.AuthenticateAsync(authenticateRequestDto);

        return new AuthenticateETAResult
        {
            Data = response
        };
    }
} 