using System.Threading;

namespace HealthCheckAsyncStartupTasks
{
    public class StartupTaskContext
    {
        private int _outstandingTaskCount = 0;

        public void RegisterTask()
        {
            Interlocked.Increment(ref _outstandingTaskCount);
        }

        public void MarkTaskAsComplete()
        {
            Interlocked.Decrement(ref _outstandingTaskCount);
        }

        public bool IsComplete => _outstandingTaskCount == 0;
    }
}