using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.FeatureManagement;
using System;
using System.Linq;

namespace CustomFeatureFilter
{
    [FilterAlias("Claims")]
    public class ClaimsFeatureFilter : IFeatureFilter
    {
        private readonly IHttpContextAccessor _httpContextAccesor;

        public ClaimsFeatureFilter(IHttpContextAccessor httpContextAccesor)
        {
            _httpContextAccesor = httpContextAccesor ?? throw new ArgumentNullException(nameof(httpContextAccesor));
        }

        public bool Evaluate(FeatureFilterEvaluationContext context)
        {
            var settings = context.Parameters.Get<ClaimsFilterSettings>();

            var user = _httpContextAccesor.HttpContext.User;

            return settings.RequiredClaims
                .All(claimType => user.HasClaim(claim => claim.Type == claimType));
        }
    }
        
}
