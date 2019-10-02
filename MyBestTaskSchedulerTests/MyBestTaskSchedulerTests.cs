using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CustomTaskScheduler;
using NUnit.Framework;

namespace MyBestTaskSchedulerTests
{
    [TestFixture]
    public class MyBestTaskSchedulerTests
    {
        private MyBestTaskScheduler _testCustomScheduler;

        private const int SleepLength = 100;
        private readonly object sync = new object();

        private List<Task> _cancelledTasks;
        private Task _longRunningTask;

        [TestCase(25, 500, 250)]
        [TestCase(35, 700, 400)]
        [TestCase(50, 1000, 550)]
        public void MyBestTaskScheduler_ShouldScheduleAllTasks_AllTasksSuccessfullyCompleted(
            int concurrencyLevel, int tasksCount, int longRunningTask)
        {
            _testCustomScheduler = new MyBestTaskScheduler(concurrencyLevel);
            _testCustomScheduler.RunLongRunningTaskEventHandler += RunLongRunningTaskEventHandler;

            var taskFactory = new TaskFactory(_testCustomScheduler);

            var tasks = new Task[tasksCount];

            for (var i = 0; i < tasks.Length; i++)
            {
                var tco = TaskCreationOptions.None;

                if (i == longRunningTask)
                {
                    tco = TaskCreationOptions.LongRunning;
                }

                tasks[i] = taskFactory.StartNew(() =>
                {
                    Thread.Sleep(SleepLength);
                }, tco);
            }

            Task.WaitAll(tasks);
            Assert.IsTrue(tasks.All(t => t.IsCompleted));
            Assert.AreEqual(_longRunningTask, tasks.First(t => t.CreationOptions.HasFlag(TaskCreationOptions.LongRunning)));
        }

        [TestCase(25, 400)]
        [TestCase(35, 700)]
        [TestCase(50, 1000)]
        public void MyBestTaskScheduler_AllTasksShouldFail_WhenTheyThrowException(
            int concurrencyLevel, int tasksCount)
        {
            _testCustomScheduler = new MyBestTaskScheduler(concurrencyLevel);

            var taskFactory = new TaskFactory(_testCustomScheduler);

            var tasks = new Task[tasksCount];
            var continuationTasks = new Task<bool>[tasksCount];

            for (var i = 0; i < tasks.Length; i++)
            {
                tasks[i] = taskFactory.StartNew(() => throw new Exception());
                continuationTasks[i] = tasks[i].ContinueWith(t => t.IsFaulted, TaskContinuationOptions.OnlyOnFaulted);
            }

            Task.WaitAll(continuationTasks);
            Assert.IsTrue(continuationTasks.All(t => t.Result));
        }

        [TestCase(10, 500)]
        [TestCase(15, 700)]
        [TestCase(20, 1000)]
        public void MyBestTaskScheduler_ShouldDequeueTasks_WhichWereCancelled(int concurrencyLevel, int tasksCount)
        {
            _testCustomScheduler = new MyBestTaskScheduler(concurrencyLevel);
            _testCustomScheduler.TryDequeueEventHandler += TryDequeueEventHandler;

            _cancelledTasks = new List<Task>();

            var taskFactory = new TaskFactory(_testCustomScheduler);

            var tasks = new Task[tasksCount];

            var source = new CancellationTokenSource();

            for (var i = 0; i < tasks.Length; i++)
            {
                tasks[i] = taskFactory.StartNew(() =>
                {
                    Thread.Sleep(SleepLength);
                }, source.Token);
            }

            source.CancelAfter(10);

            Task.Delay(1000).Wait();
            
            Assert.IsNotNull(_cancelledTasks);
            Assert.AreEqual(_testCustomScheduler.CountOfDequeuedTasks, _cancelledTasks.Count);
            Assert.IsTrue(_cancelledTasks.All(ct => ct.Status == TaskStatus.WaitingToRun));
        }

        [TestCase(60)]
        [TestCase(-1)]
        public void MyBestTaskScheduler_ShouldThrowArgumentException_WhenProvidedConcurrencyIsNotAllowed(int concurrencyLevel)
        {
            var ex = Assert.Throws<ArgumentException>(() =>
                _testCustomScheduler = new MyBestTaskScheduler(concurrencyLevel));

            Assert.AreEqual($"Wrong {nameof(concurrencyLevel)}.", ex.Message);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _testCustomScheduler.TryDequeueEventHandler -= TryDequeueEventHandler;
            _testCustomScheduler.RunLongRunningTaskEventHandler -= RunLongRunningTaskEventHandler;
            _testCustomScheduler.Dispose();
        }

        private void TryDequeueEventHandler(object sender, Task task)
        {
            lock (sync)
            {
                _cancelledTasks.Add(task);
            }
        }

        private void RunLongRunningTaskEventHandler(object sender, Task e)
        {
            _longRunningTask = e;
        }
    }
}