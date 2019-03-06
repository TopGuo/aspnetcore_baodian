using Quartz;
using Quartz.Impl;
using System;
using System.Threading.Tasks;
using TB.AspNetCore.Domain.Config;
using TB.AspNetCore.Domain.Models.Web;
using TB.AspNetCore.Infrastructrue.Extensions;
using TB.AspNetCore.Infrastructrue.Logs;
using TB.AspNetCore.Infrastructrue.Tasks.Quartz.Model;

namespace TB.AspNetCore.Infrastructrue.Tasks.Quartz
{
    /// <summary>
    /// 任务调度中心
    /// </summary>
    public class JobCenter:IJobCenter
    {
        
        public static IScheduler scheduler = null;
        public static async Task<IScheduler> GetSchedulerAsync()
        {
            if (scheduler != null)
            {
                return scheduler;
            }
            else
            {
                ISchedulerFactory schedf = new StdSchedulerFactory();
                IScheduler sched = await schedf.GetScheduler();
                return sched;
            }
        }
        /// <summary>
        /// 添加任务计划//或者进程终止后的开启
        /// </summary>
        /// <returns></returns>
        public async Task<ResponsResult> AddScheduleJobAsync(TaskScheduleModel m)
        {
            ResponsResult result = new ResponsResult();
            try
            {
                if (m != null)
                {
                    DateTimeOffset starRunTime = DateBuilder.NextGivenSecondDate(m.StarRunTime, 1);
                    DateTimeOffset endRunTime = DateBuilder.NextGivenSecondDate(m.EndRunTime, 1);
                    scheduler = await GetSchedulerAsync();
                    IJobDetail job = JobBuilder.Create<HttpJob>()
                      .WithIdentity(m.JobName, m.JobGroup)
                      .Build();
                    ICronTrigger trigger = (ICronTrigger)TriggerBuilder.Create()
                                                 .StartAt(starRunTime)
                                                 .EndAt(endRunTime)
                                                 .WithIdentity(m.JobName, m.JobGroup)
                                                 .WithCronSchedule(m.CronExpress)
                                                 .Build();
                    await scheduler.ScheduleJob(job, trigger);
                    await scheduler.Start();
                    result.Data = m;
                    return result;
                }
                return result.SetError("传入实体为空");
            }
            catch (Exception ex)
            {
                Log4Net.Error($"[JobCenter_AddScheduleJobAsync]_{ex}");
                return result.SetError(ex.Message);
            }
        }

        /// <summary>
        /// 暂停指定任务计划
        /// </summary>
        /// <returns></returns>
        public async Task<ResponsResult> StopScheduleJobAsync(string jobGroup, string jobName)
        {
            ResponsResult result = new ResponsResult();
            try
            {
                scheduler = await GetSchedulerAsync();
                //使任务暂停
                await scheduler.PauseJob(new JobKey(jobName, jobGroup));                
                var status = new StatusViewModel()
                {
                    Status = 0,
                    Msg = "暂停任务计划成功",
                };
                result.Data = status.GetJson();
                return result;
            }
            catch (Exception ex)
            {
                Log4Net.Error($"[JobCenter_StopScheduleJobAsync]_{ex}");
                var status = new StatusViewModel()
                {
                    Status = -1,
                    Msg = "暂停任务计划失败",
                };
                result.Data = status.GetJson();
                return result;
            }
        }
        /// <summary>
        /// 恢复指定的任务计划**恢复的是暂停后的任务计划，如果是程序奔溃后 或者是进程杀死后的恢复，此方法无效
        /// </summary>
        /// <returns></returns>
        public async Task<ResponsResult> RunScheduleJobAsync(TaskScheduleModel sm)
        {
            ResponsResult result = new ResponsResult();
            try
            {
                #region 开任务
                //scheduler = await GetSchedulerAsync();
                //DateTimeOffset starRunTime = DateBuilder.NextGivenSecondDate(sm.StarRunTime, 1);
                //DateTimeOffset endRunTime = DateBuilder.NextGivenSecondDate(sm.EndRunTime, 1);
                //IJobDetail job = JobBuilder.Create<HttpJob>()
                //  .WithIdentity(sm.JobName, sm.JobGroup)
                //  .Build();
                //ICronTrigger trigger = (ICronTrigger)TriggerBuilder.Create()
                //                             .StartAt(starRunTime)
                //                             .EndAt(endRunTime)
                //                             .WithIdentity(sm.JobName, sm.JobGroup)
                //                             .WithCronSchedule(sm.CronExpress)
                //                             .Build();
                //await scheduler.ScheduleJob(job, trigger);
                //await scheduler.Start();
                #endregion
                scheduler = await GetSchedulerAsync();
                //resumejob 恢复
                await scheduler.ResumeJob(new JobKey(sm.JobName, sm.JobGroup));

                var status = new StatusViewModel()
                {
                    Status = 0,
                    Msg = "恢复任务计划成功",
                };
                result.Data = status.GetJson();
                return result;
            }
            catch (Exception ex)
            {
                Log4Net.Error($"[JobCenter_RunScheduleJobAsync]_{ex}");
                var status = new StatusViewModel()
                {
                    Status = -1,
                    Msg = "恢复任务计划失败",
                };
                result.Data = status.GetJson();
                return result;
            }
        }
    }
}
