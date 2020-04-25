using Microsoft.AspNetCore.Authorization;

namespace PreventingOverPosting
{
    public class IsNotUserWithIdRequirement : IAuthorizationRequirement
    {
        public int DisallowedId { get; }
        public IsNotUserWithIdRequirement(int id)
        {
            DisallowedId = id;
        }
    }
}
