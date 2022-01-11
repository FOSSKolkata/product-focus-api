using System.Security.Claims;

namespace ProductFocusApi.Helpers
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserId(this ClaimsPrincipal principal)
        {
            var loggedInUserId = principal.FindFirstValue(ClaimTypes.NameIdentifier);


            return loggedInUserId;
        }
    }
}
