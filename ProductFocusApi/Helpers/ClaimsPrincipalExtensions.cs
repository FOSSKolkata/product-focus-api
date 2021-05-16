using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

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
