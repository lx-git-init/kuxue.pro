using Quartz;
using System;
using System.Threading.Tasks;

namespace kuxue.pro.task.quartz.Jobs
{
    [PersistJobDataAfterExecution]
    //[DisallowConcurrentExecution] // 等待上一个job完成才能执行下一个job
    public class MyJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            //  获取传入的参数
            var jobData = context.JobDetail.JobDataMap;
            var triggerData = context.Trigger.JobDataMap;
            var data = context.MergedJobDataMap;

            jobData["key1"] = jobData.GetInt("key1") + 1; // PersistJobDataAfterExecution可延续上次返回的值

            Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}____{jobData["key1"]}");
            return Task.CompletedTask;
        }
    }
}