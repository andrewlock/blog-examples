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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClaimsFeatureFilter(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public bool Evaluate(FeatureFilterEvaluationContext context)
        {
            var settings = context.Parameters.Get<ClaimsFilterSettings>();

            var user = _httpContextAccessor.HttpContext.User;

            return settings.RequiredClaims
                .All(claimType => user.HasClaim(claim => claim.Type == claimType));
        }
    }
}