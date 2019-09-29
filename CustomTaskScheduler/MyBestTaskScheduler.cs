using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Timer = System.Timers.Timer;

namespace CustomTaskScheduler
{
    public class MyBestTaskScheduler : TaskScheduler, IDisposable
    {
        private readonly BlockingCollection<Task> _taskExecutionQueue;
        private readonly List<Task> _tasksQueue = new List<Task>();
        private readonly List<Thread> _threads = new List<Thread>();
        private readonly Dictionary<int, Task> _tasksCache = new Dictionary<int, Task>();
        private readonly Timer _threadsObservingTimer;
        private readonly int _maximumConcurrencyLevel;
        private readonly object _sync = new object();

        private const int MinimumAmountOfFreeThreads = 5;
        private const int TimerInterval = 100;
        private const int MaximumAllowedConcurrencyLevel = 50;
        private const int MinimumAllowedConcurrencyLevel = 0;

        private EventHandler _notifyOfFreeCapacityEventHandler;
        private bool _disposed;

        public override int MaximumConcurrencyLevel => _maximumConcurrencyLevel;

        public MyBestTaskScheduler(int concurrencyLevel)
        {
            if (concurrencyLevel < MinimumAllowedConcurrencyLevel || concurrencyLevel > MaximumAllowedConcurrencyLevel)
            {
                throw new ArgumentException($"Wrong {nameof(concurrencyLevel)}.");
            }

            _threadsObservingTimer = new Timer(TimerInterval);
            _threadsObservingTimer.Elapsed += ThreadsObservingTimerCallback;
            _threadsObservingTimer.Start();

            _maximumConcurrencyLevel = concurrencyLevel;

            _taskExecutionQueue = new BlockingCollection<Task>(_maximumConcurrencyLevel);

            var initialConcurrencyLevel = _maximumConcurrencyLevel > MinimumAmountOfFreeThreads ? MinimumAmountOfFreeThreads : _maximumConcurrencyLevel;

            _notifyOfFreeCapacityEventHandler += NotifyOfFreeCapacityEventHandler;

            _threads.AddRange(CreateThreads(initialConcurrencyLevel));

            _threads.ForEach(t => t.Start());
        }

        protected override void QueueTask(Task task)
        {
            lock (_sync)
            {
                if (_threads.Count != MaximumConcurrencyLevel)
                {
                    var delta = MaximumConcurrencyLevel - _threads.Count;
                    var extraCount = delta > MinimumAmountOfFreeThreads ? MinimumAmountOfFreeThreads : delta;

                    var newThreads = CreateThreads(extraCount);

                    newThreads.ForEach(t => t.Start());

                    _threads.AddRange(newThreads);
                }

                if (_taskExecutionQueue.TryAdd(task)) return;

                _tasksQueue.Add(task);
            }
        }

        protected override bool TryDequeue(Task task)
        {
            if (!_tasksQueue.Contains(task)) return false;

            lock (_sync)
            {
                if (!_tasksQueue.Contains(task)) return false;
                _tasksQueue.Remove(task);
                return true;
            }
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
            => _threads.Contains(Thread.CurrentThread) && TryExecuteTask(task);

        protected override IEnumerable<Task> GetScheduledTasks() => _taskExecutionQueue.ToArray();

        private void ThreadsObservingTimerCallback(object sender, EventArgs e)
        {
            lock (_sync)
            {
                var unusedThreads = new List<Thread>();

                foreach (var thread in _threads)
                {
                    if (_tasksCache.TryGetValue(thread.ManagedThreadId, out var task)
                        && (task.IsCanceled || task.IsCompleted || task.IsFaulted))
                    {
                        unusedThreads.Add(thread);
                    }
                }

                if (unusedThreads.Count <= MinimumAmountOfFreeThreads) return;

                var dif = unusedThreads.Count - MinimumAmountOfFreeThreads;

                foreach (var thread in unusedThreads.Take(dif))
                {
                    _threads.Remove(thread);
                }
            }
        }

        private void NotifyOfFreeCapacityEventHandler(object sender, EventArgs e)
        {
            if (_tasksQueue.Count == 0) return;

            lock (_sync)
            {
                if (_tasksQueue.Count == 0) return;

                var task = _tasksQueue[0];
                _tasksQueue.RemoveAt(0);

                QueueTask(task);
            }
        }

        private Thread CreateNewThread()
        {
            var thread = new Thread(ThreadBody);
            thread.SetApartmentState(ApartmentState.STA);
            return thread;
        }

        private void ThreadBody()
        {
            foreach (var t in _taskExecutionQueue.GetConsumingEnumerable())
            {
                lock (_sync)
                {
                    _tasksCache[Thread.CurrentThread.ManagedThreadId] = t;
                }

                TryExecuteTask(t);

                if (MaximumConcurrencyLevel > _taskExecutionQueue.Count)
                {
                    _notifyOfFreeCapacityEventHandler?.Invoke(_taskExecutionQueue, EventArgs.Empty);
                }

                lock (_sync)
                {
                    _tasksCache.Remove(Thread.CurrentThread.ManagedThreadId);
                }
            }
        }

        private List<Thread> CreateThreads(int amount)
        {
            var threads = new List<Thread>();

            for (var i = 0; i < amount; i++)
            {
                threads.Add(CreateNewThread());
            }

            return threads;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                if (_threadsObservingTimer != null)
                {
                    _threadsObservingTimer.Stop();
                    _threadsObservingTimer.Elapsed -= ThreadsObservingTimerCallback;
                    _threadsObservingTimer.Dispose();
                }

                _taskExecutionQueue.CompleteAdding();
                _notifyOfFreeCapacityEventHandler -= NotifyOfFreeCapacityEventHandler;

                foreach (var thread in _threads)
                {
                    thread.Join();
                }

                _taskExecutionQueue.Dispose();
            }

            _disposed = true;
        }
    }
}