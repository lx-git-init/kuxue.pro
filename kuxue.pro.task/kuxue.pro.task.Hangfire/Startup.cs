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
                TransactionIsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, // ������뼶��Ĭ�϶�ȡ���ύ
                QueuePollInterval = TimeSpan.FromSeconds(15), // ��ҵ������ѯ�����Ĭ��ֵ15��
                JobExpirationCheckInterval = TimeSpan.FromHours(1), // ��ҵ���ڼ������������ڼ�¼����Ĭ��ֵΪ1Сʱ
                CountersAggregateInterval = TimeSpan.FromMinutes(5), // �ۺϼ������ļ����Ĭ��Ϊ5����
                PrepareSchemaIfNecessary = true, // �������Ϊtrue���򴴽����ݿ��Ĭ��Ϊtrue
                DashboardJobListLimit = 50000, // �Ǳ����ҵ�б����ƣ�Ĭ��ֵΪ50000
                TransactionTimeout = TimeSpan.FromMinutes(1), // ���׳�ʱ��Ĭ��1����
                TablesPrefix = "Hangfire", // ���ݿ��б��ǰ׺��Ĭ��Ϊnone
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