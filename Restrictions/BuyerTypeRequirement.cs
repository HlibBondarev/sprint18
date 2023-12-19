using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using TaskAuthenticationAuthorization.Models;

namespace TaskAuthenticationAuthorization.Restrictions
{
    public class BuyerTypeRequirement: IAuthorizationRequirement
    {
        protected internal List<BuyerType> BuyerTypes { get; set; }
        public BuyerTypeRequirement(List<BuyerType> buyerTypes)
        {
            BuyerTypes = buyerTypes; 
        }
    }
}
