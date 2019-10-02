using System.Threading;
using System.Threading.Tasks;

namespace CustomTaskScheduler
{
    public static class TaskExtensions
    {
        public static CancellationToken GetCancellationToken(this Task task) => new TaskCanceledException(task).CancellationToken;
    }
}
