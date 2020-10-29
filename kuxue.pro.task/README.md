### 一、Quartz
    1、安装nuget包 Quartz.AspNetCore  
    2、调用步骤
        * 通过调度工厂获取调度器 StdSchedulerFactory.GetScheduler()
        * 开启调度器 scheduler.Start()
        * 创建IJobDetail JobBuilder.Create<>()
        * 创建ITrigger TriggerBuilder.Create()
        * 将作业和触发器绑定到调度器 scheduler.ScheduleJob(job, trigger)
    3、参数传递
        * job传参 IJobDetail.JobDataMap 或 JobBuilder.UseJobData
        * trigger传参 ITrigger.JobDataMap 或 TriggerBuilder.UseJobData
    4、Job执行时需要注意
        * 是否需要使用上一Job的执行结果 [PersistJobDataAfterExecution]
        * trigger设置的时间与Job执行所耗时间冲突时如何处理 [DisallowConcurrentExecution]
    5、调度器上添加监听
        scheduler.ListenerManager.AddJobListener(...);

### 二、Hangfire  
    1、安装Nuget包 Hangfire.AspNetCore
                  Hangfire.MySqlStorage (这里使用的是Mysql数据库)
    2、Startup中进行add()和use()
    3、新建数据库hangfire，运行程序后会自动生成数据表
    4、Hangfire四种基本用法：
        1）Fire-and-forget：任务队列
            BackgroundJob.Enqueue(()=>Console.WriteLine("Fire-and-forget"));
        2）Delayed：延迟任务
            BackgroundJob.Schedule(()=>Console.WriteLine("Delayed"), TimeSpan.FromDays(1));
        3）Recurring：周期任务
            BackgroundJob.AddOrUpdate("job1", ()=>TaskAction("1"), "0/10 * * * * ?");
        4）Containuations：工作流任务
            var job1 = BackgroundJob.Enqueue(()=>Console.WriteLine("Hello, "));
            BackgroundJob.ContinueWith(job1, ()=>Console.WriteLine("World!")); 

### 三、IHostServices + Timer（netcore自带）

