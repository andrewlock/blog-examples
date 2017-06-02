using Microsoft.Extensions.Logging;

namespace RegisteringForDispose.Controllers
{
    public class TransientCreatedByContainer : Disposable
    {
        public TransientCreatedByContainer()
        {
        }
    }
}
