using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TaskAuthenticationAuthorization.Restrictions
{
	public class SpecialBuyerTypeHandler : AuthorizationHandler<BuyerTypeRequirement>
	{
		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, BuyerTypeRequirement requirement)
		{
			var user = context.User;
			if (user.IsInRole("admin"))
			{
				context.Succeed(requirement);
			}
			Claim claim = context.User.FindFirst("RestrictionForBuyerType_are_Golden_or_Wholesale");
			if (claim != null)
			{
				if (requirement.BuyerTypes.Any(bt => bt.ToString() == claim?.Value))
				{
					context.Succeed(requirement);
				}
			}
			return Task.CompletedTask;
		}
	}
}
