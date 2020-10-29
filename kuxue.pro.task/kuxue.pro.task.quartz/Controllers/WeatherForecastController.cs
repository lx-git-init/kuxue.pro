using kuxue.pro.task.quartz.Jobs;
using kuxue.pro.task.quartz.Listeners;
using Microsoft.AspNetCore.Mvc;
using Quartz;
using System.Threading.Tasks;

namespace kuxue.pro.task.quartz.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ISchedulerFactory _schedulerFactory;

        public WeatherForecastController(ISchedulerFactory schedulerFactory)
        {
            _schedulerFactory = schedulerFactory;
        }

        [HttpGet]
        public async Task Get()
        {
            // 1. 通过调度工厂获取调度器
            IScheduler scheduler = await _schedulerFactory.GetScheduler();

            // 1.1 添加监听器
            scheduler.ListenerManager.AddJobListener(new MyJobListener());

            // 2. 开启调度器
            await scheduler.Start();
            // 3. 创建一个Job
            IJobDetail job = JobBuilder.Create<MyJob>()
                .UsingJobData("key1", 99)
                .WithIdentity("job", "group").Build();
            // 4. 创建一个触发器Trigger
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("trigger", "group")
                .UsingJobData("key2", "value2")
                .WithCronSchedule("0/1 * * * * ?").Build();
            // 5. 将作业和触发器绑定到调度器
            await scheduler.ScheduleJob(job, trigger);
        }
    }
}