using System;
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
            _testCustomScheduler?.Dispose();
        }
    }
}