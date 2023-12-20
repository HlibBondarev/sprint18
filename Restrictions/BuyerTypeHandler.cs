using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System;
using System.Security.Claims;
using System.Linq;

namespace TaskAuthenticationAuthorization.Restrictions
{
    public class BuyerTypeHandler : AuthorizationHandler<BuyerTypeRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, BuyerTypeRequirement requirement)
        {
            var user = context.User;
            if (user.IsInRole("admin"))
            {
                context.Succeed(requirement);
            }
            Claim claim = context.User.FindFirst("RestrictionForBuyerType");
            if (claim != null)
            {
                if(requirement.BuyerTypes.Any(bt=> bt.ToString()== claim?.Value))
                {
                    context.Succeed(requirement);
                }
            }
            return Task.CompletedTask;
        }
    }
}
