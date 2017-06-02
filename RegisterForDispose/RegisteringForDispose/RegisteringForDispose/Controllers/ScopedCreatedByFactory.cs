using Microsoft.Extensions.Logging;

namespace RegisteringForDispose.Controllers
{
    public class ScopedCreatedByFactory : Disposable
    {
        public ScopedCreatedByFactory()
        {
        }
    }
}
