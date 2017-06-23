using System;
using Microsoft.Extensions.Logging;

namespace RegisteringForDispose.Controllers
{
    public abstract class Disposable : IDisposable
    {
        public Disposable()
        {
            Console.WriteLine("+ {0} was created", this.GetType().Name);
        }

        public void Dispose()
        {
            Console.WriteLine("- {0} was disposed!", this.GetType().Name);
        }
    }
}
