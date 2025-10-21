using ASPNET.BackEnd.Common.Base;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace ASPNET.BackEnd.Common.Localization
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocalizationController : BaseApiController
    {
        public LocalizationController(ISender sender) : base(sender)
        {
        }

        [HttpPost("SetLanguage")]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            // Default to en-US if culture is null or invalid
            if (string.IsNullOrEmpty(culture) || !new[] { "en-US", "ar-EG" }.Contains(culture))
            {
                culture = "en-US";
            }

            // Set culture cookie
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            // Default to home or current page if returnUrl is null
            returnUrl = string.IsNullOrEmpty(returnUrl) ? "/" : returnUrl;

            return LocalRedirect(returnUrl);
        }
    }
}