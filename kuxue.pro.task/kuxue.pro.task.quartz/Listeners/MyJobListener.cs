using Quartz;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace kuxue.pro.task.quartz.Listeners
{
    public class MyJobListener : IJobListener
    {
        public string Name => nameof(MyJobListener);

        public Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = default)
        {
            return Task.Run(() =>
            {
                Console.WriteLine($"{Name}_{nameof(JobExecutionVetoed)}");
            });
        }

        public Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = default)
        {
            return Task.Run(() =>
            {
                Console.WriteLine($"{Name}_{nameof(JobToBeExecuted)}");
            });
        }

        public Task JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException, CancellationToken cancellationToken = default)
        {
            return Task.Run(() =>
            {
                Console.WriteLine($"{Name}_{nameof(JobWasExecuted)}");
            });
        }
    }
}