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
    public class UnitTest1
    {
        private MyBestTaskScheduler _testCustomScheduler;

        private const int SleepLength = 100;
        private List<Task> _cancelledTasks;
        private readonly object sync = new object();

        [TestCase(25, 5000)]
        [TestCase(35, 7000)]
        [TestCase(50, 10000)]
        public void MyBestTaskScheduler_ShouldScheduleAllTasks_AllTasksSuccessfullyCompleted(int concurrencyLevel, int tasksCount)
        {
            _testCustomScheduler = new MyBestTaskScheduler(concurrencyLevel);

            var taskFactory = new TaskFactory(_testCustomScheduler);

            var tasks = new Task[tasksCount];

            for (var i = 0; i < tasks.Length; i++)
            {
                tasks[i] = taskFactory.StartNew(() =>
                {
                    Thread.Sleep(SleepLength);
                });
            }

            Task.WaitAll(tasks);

            Assert.IsTrue(tasks.All(t => t.IsCompleted));
        }

        [TestCase(10, 5000, 100)]
        [TestCase(15, 7000, 200)]
        [TestCase(20, 10000, 300)]
        public void MyBestTaskScheduler_DequeueTasksOnCancellation(int concurrencyLevel, int tasksCount, int cancelAfter)
        {
            _testCustomScheduler = new MyBestTaskScheduler(concurrencyLevel);
            _testCustomScheduler.TryDequeueEventHandler += TryDequeueEventHandler;

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

            source.CancelAfter(cancelAfter);
            foreach (var task in tasks)
            {
                Task.Run(() => task.Wait()).Wait();
            }

            Assert.IsNotNull(_cancelledTasks);
            Assert.IsTrue(_cancelledTasks.All(ct => ct.IsCanceled));
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
            if (_testCustomScheduler == null)
            {
                return;
            }

            _testCustomScheduler.TryDequeueEventHandler -= TryDequeueEventHandler;
            _testCustomScheduler.Dispose();
        }

        private void TryDequeueEventHandler(object sender, Task task)
        {
            if (_cancelledTasks == null)
            {
                _cancelledTasks = new List<Task>();
            }

            lock (sync)
            {

                _cancelledTasks.Add(task);
            }
        }
    }
}