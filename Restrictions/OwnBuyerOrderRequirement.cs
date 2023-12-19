using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using TaskAuthenticationAuthorization.Models;

namespace TaskAuthenticationAuthorization.Restrictions
{
    public class OwnBuyerOrderRequirement : IAuthorizationRequirement
    {
        protected internal int UserId { get; set; }
        public OwnBuyerOrderRequirement(int userId)
        {
            UserId = userId;
        }
    }
}
