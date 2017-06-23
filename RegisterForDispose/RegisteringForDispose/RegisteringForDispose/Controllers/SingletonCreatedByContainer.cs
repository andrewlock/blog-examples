using Microsoft.Extensions.Logging;

namespace RegisteringForDispose.Controllers
{
    public class SingletonCreatedByContainer : Disposable
    {
        public SingletonCreatedByContainer()
        {
        }
    }
}