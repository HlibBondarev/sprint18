using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TaskAuthenticationAuthorization.Restrictions
{
    public class OwnBuyerOrderHandler : AuthorizationHandler<OwnBuyerOrderRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OwnBuyerOrderRequirement requirement)
        {
            var user = context.User;
            if (user.IsInRole("admin"))
            {
                context.Succeed(requirement);
            }
            Claim claim = context.User.FindFirst("OnlyOwnBuyerOrders");
            if (claim != null)
            {
                if (requirement.UserId.ToString() == claim?.Value)
                {
                    context.Succeed(requirement);
                }
            }
            return Task.CompletedTask;
        }
    }
}
