﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProductFocus.Api.AuthorizationPolicies
{
    public class ScopesRequirement : AuthorizationHandler<ScopesRequirement>, IAuthorizationRequirement
    {
        readonly string[] _acceptedScopes;

        public ScopesRequirement(params string[] acceptedScopes)
        {
            _acceptedScopes = acceptedScopes;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                        ScopesRequirement requirement)
        {
            // If there are no scopes, do not process
            if (!context.User.Claims.Any(x => x.Type == ClaimConstants.Scope)
               && !context.User.Claims.Any(y => y.Type == ClaimConstants.Scp))
            {
                return Task.CompletedTask;
            }

            Claim scopeClaim = context?.User?.FindFirst(ClaimConstants.Scp);

            if (scopeClaim == null)
                scopeClaim = context?.User?.FindFirst(ClaimConstants.Scope);

            if (scopeClaim != null && scopeClaim.Value.Split(' ').Intersect(requirement._acceptedScopes).Any())
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
