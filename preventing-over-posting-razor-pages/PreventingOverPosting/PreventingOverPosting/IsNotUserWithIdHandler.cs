using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace PreventingOverPosting
{
    public class IsNotUserWithIdHandler : AuthorizationHandler<IsNotUserWithIdRequirement, int>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsNotUserWithIdRequirement requirement, int resource)
        {
            if(requirement.DisallowedId != resource)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
