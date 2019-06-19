using Microsoft.AspNetCore.Http;
using Microsoft.FeatureManagement;

namespace CustomFeatureFilter
{
    public class SessionSessionManager : ISessionManager
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public SessionSessionManager(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public void Set(string featureName, bool enabled)
        {
            var session = _contextAccessor.HttpContext.Session;
            var sessionKey = $"feature_{featureName}";
            session.Set(sessionKey, new[] {enabled ? (byte) 1 : (byte) 0});
        }

        public bool TryGet(string featureName, out bool enabled)
        {
            var session = _contextAccessor.HttpContext.Session;
            if (session.TryGetValue($"feature_{featureName}", out var enabledBytes))
            {
                enabled = enabledBytes[0] == 1;
                return true;
            }

            enabled = false;
            return false;
        }
    }
}