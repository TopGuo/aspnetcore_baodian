using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using TB.AspNetCore.Domain.Config;
using TB.AspNetCore.Domain.Enums;
using TB.AspNetCore.Domain.Models.Web;
using TB.AspNetCore.Infrastructrue.Contexts;
using TB.AspNetCore.Infrastructrue.Extensions;

namespace TB.AspNetCore.Infrastructrue.Tasks.Quartz.Extensions
{
    public static class JobServiceExtensions
    {
        /// <summary>
        /// 程序启动将任务调度表里所有状态为 执行中 任务启动起来
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static IServiceCollection AddJobService(this IServiceCollection serviceCollection)
        {
            serviceCollection.BuildServiceProvider().RegisterServiceProvider();
            var jobCenter = ServiceCollectionExtension.Get<IJobCenter>();
            var dbContext = ServiceCollectionExtension.New<BaoDianContext>();
            var jobInfoList = dbContext.TaskSchedule
                .Where(t => t.RunStatus.Equals((int)TaskJobStatus.DoJob))
                .Select(t => new TaskScheduleModel
                {
                    Id = t.Id,
                    JobGroup = t.JobGroup,
                    JobName = t.JobName,
                    CronExpress = t.CronExpress,
                    StarRunTime = t.StarRunTime,
                    EndRunTime = t.EndRunTime,
                    NextRunTime = t.NextRunTime,
                    RunStatus = t.RunStatus
                }).ToList();

            jobInfoList.ForEach(async t =>
            {
                await jobCenter.AddScheduleJobAsync(t);
            });
            return serviceCollection;
        }
    }
}
