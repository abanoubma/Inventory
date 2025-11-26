using Application.Features.CustomerManager.Queries;
using Application.Features.Location.Queries;
using ASPNET.BackEnd.Common.Base;
using ASPNET.BackEnd.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace ASPNET.BackEnd.Controllers
{

}[Route("api/[controller]")]
[ApiController]
public class LocationController : BaseApiController
{
    public LocationController(ISender sender) : base(sender)
    {
    }

    [AllowAnonymous] // or [Authorize] if you want auth
    [HttpGet("GetCountries")]
    public async Task<ActionResult<ApiSuccessResult<LocationListResult>>> GetCountries(
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetCountriesList(), cancellationToken);

        return Ok(new ApiSuccessResult<LocationListResult>
        {
            Code = StatusCodes.Status200OK,
            Message = "Countries retrieved successfully",
            Content = result
        });
    }

    [AllowAnonymous]
    [HttpGet("GetGovernorates")]
    public async Task<ActionResult<ApiSuccessResult<LocationListResult>>> GetGovernorates(
        [FromQuery] int countryId,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetGovernoratesList { CountryId = countryId }, cancellationToken);

        return Ok(new ApiSuccessResult<LocationListResult>
        {
            Code = StatusCodes.Status200OK,
            Message = "Governorates retrieved successfully",
            Content = result
        });
    }

    [AllowAnonymous]
    [HttpGet("GetCities")]
    public async Task<ActionResult<ApiSuccessResult<LocationListResult>>> GetCities(
        [FromQuery] int governorateId,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetCitiesList { GovernorateId = governorateId }, cancellationToken);

        return Ok(new ApiSuccessResult<LocationListResult>
        {
            Code = StatusCodes.Status200OK,
            Message = "Cities retrieved successfully",
            Content = result
        });
    }
}
