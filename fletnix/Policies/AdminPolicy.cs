using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
/*
namespace fletnix.Policies
{
    public class AdminPolicy : AuthorizationHandler<AdminPolicy>, IAuthorizationRequirement
    {
        protected override HandleRequirementAsync(AuthorizationHandlerContext context, AdminPolicy requirement)
        {
            if (!context.User.HasClaim(c => c.Type == ClaimTypes.Role))
            {
                context.Fail();

            }

            var role = context.User.FindFirst(c => c.Type == ClaimTypes.Role).Value;
            if (role == "admin")
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
        }
    }
}*/