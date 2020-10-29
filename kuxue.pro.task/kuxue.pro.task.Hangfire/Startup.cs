using System;
using Hangfire;
using Hangfire.MySql;
using kuxue.pro.task.Hangfire.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace kuxue.pro.task.Hangfire
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddHangfire(x => x.UseStorage(new MySqlStorage(Configuration.GetConnectionString("DefaultConnection"), new MySqlStorageOptions
            {
                TransactionIsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, // 事务隔离级别，默认读取已提交
                QueuePollInterval = TimeSpan.FromSeconds(15), // 作业队列轮询间隔，默认值15秒
                JobExpirationCheckInterval = TimeSpan.FromHours(1), // 作业到期检查间隔（管理过期记录），默认值为1小时
                CountersAggregateInterval = TimeSpan.FromMinutes(5), // 聚合计数器的间隔，默认为5分钟
                PrepareSchemaIfNecessary = true, // 如果设置为true，则创建数据库表，默认为true
                DashboardJobListLimit = 50000, // 仪表板作业列表限制，默认值为50000
                TransactionTimeout = TimeSpan.FromMinutes(1), // 交易超时，默认1分钟
                TablesPrefix = "Hangfire", // 数据库中表的前缀，默认为none
            })));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHangfireServer();
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new DashboardAuthorizationFilter() }
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}