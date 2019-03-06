using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TB.AspNetCore.Domain.Config;
using TB.AspNetCore.Domain.Entitys;
using TB.AspNetCore.Domain.Enums;
using TB.AspNetCore.Domain.Models.Web;
using TB.AspNetCore.Domain.Repositorys;
using TB.AspNetCore.Infrastructrue.Auth.MvcAuth;
using TB.AspNetCore.Infrastructrue.Extensions;

namespace TB.AspNetCore.Application.Services
{
    public class TaskService : BaseService, ITaskService
    {
        private IJobCenter jobCenter = ServiceCollectionExtension.Get<IJobCenter>();
        private MvcIdentity identity = (ServiceCollectionExtension.HttpContext.User.Identity as MvcIdentity);

        public async Task<ResponsResult> AddTaskAsync(TaskScheduleModel model)
        {
            ResponsResult result = new ResponsResult();

            if (model == null)
            {
                return result.SetError("Model 不能为空！");
            }
            if (string.IsNullOrEmpty(model.JobName))
            {
                return result.SetError("JobName 不能为空！");
            }
            if (string.IsNullOrEmpty(model.JobGroup))
            {
                return result.SetError("JobGroup 不能为空！");
            }
            if (model.StarRunTime == null)
            {
                model.StarRunTime = DateTime.Now;
            }
            if (model.EndRunTime == null)
            {
                model.EndRunTime = DateTime.MaxValue.AddDays(-1);
            }
            var info = await jobCenter.AddScheduleJobAsync(model);
            if (info.Status != 200)
            {
                return result.SetError(info.Message);
            }
            base.Add(new TaskSchedule
            {
                Id = Guid.NewGuid().ToString("N"),
                CreateAuthr = identity.Name,
                CronExpress = model.CronExpress,
                EndRunTime = model.EndRunTime,
                StarRunTime = model.StarRunTime,
                JobGroup = model.JobGroup,
                JobName = model.JobName,
                RunStatus = model.RunStatus

            }, true);
            return result;
        }

        public async Task<ResponsResult> DelTaskAsync(TaskScheduleModel model)
        {
            ResponsResult result = new ResponsResult();
            if (model == null)
            {
                return result.SetError("Model 不能为空！");
            }
            if (string.IsNullOrEmpty(model.JobName))
            {
                return result.SetError("JobName 不能为空！");
            }
            if (string.IsNullOrEmpty(model.JobGroup))
            {
                return result.SetError("JobGroup 不能为空！");
            }
            //search
            var taskInfo = base.Single<TaskSchedule>(t => t.Id.Equals(model.Id));
            var info = await jobCenter.StopScheduleJobAsync(model.JobGroup, model.JobName);
            if (!info.Status.Equals(200))
            {
                return result.SetError(info.Message);
            }
            //del
            base.Delete(taskInfo, true);
            return result;
        }

        public ResponsResult GetTaskList()
        {
            ResponsResult result = new ResponsResult();
            var query = base.Where<TaskSchedule>(t => !t.RunStatus.Equals(TaskJobStatus.JobHasDel)).Select(t => new
            {
                t.Id,
                t.JobGroup,
                t.JobName,
                t.RunStatus,
                t.StarRunTime,
                t.UpdateTime,
                t.CronExpress,
                t.EndRunTime,
                t.CreateTime
            }).ToList();
            List<TaskScheduleModel> _list = new List<TaskScheduleModel>();
            query.ForEach(t =>
            {
                _list.Add(new TaskScheduleModel()
                {
                    Id = t.Id,
                    JobGroup = t.JobGroup,
                    JobName = t.JobName,
                    _RunStatus = ((TaskJobStatus)t.RunStatus).GetString(),
                    StarRunTime = t.StarRunTime,
                    UpdateTime = t.UpdateTime,
                    CronExpress = t.CronExpress,
                    EndRunTime = t.EndRunTime,
                    CreateTime = t.CreateTime
                });
            });
            result.Data = _list;
            return result;
        }

        public async Task<ResponsResult> ModifyTaskAsync(TaskScheduleModel model)
        {
            ResponsResult result = new ResponsResult();

            if (model == null)
            {
                return result.SetError("Model 不能为空！");
            }
            if (string.IsNullOrEmpty(model.JobName))
            {
                return result.SetError("JobName 不能为空！");
            }
            if (string.IsNullOrEmpty(model.JobGroup))
            {
                return result.SetError("JobGroup 不能为空！");
            }
            if (model.StarRunTime == null)
            {
                model.StarRunTime = DateTime.Now;
            }
            if (model.EndRunTime == null)
            {
                model.EndRunTime = DateTime.MaxValue.AddDays(-1);
            }
            //modify
            var taskInfo = base.Single<TaskSchedule>(t => t.Id.Equals(model.Id));

            if (taskInfo == null)
            {
                return result.SetError("无此任务！");
            }
            //cron
            if (!taskInfo.CronExpress.Equals(model.CronExpress))
            {
                taskInfo.CronExpress = model.CronExpress;
                var stopInfo = await jobCenter.StopScheduleJobAsync(model.JobGroup, model.JobName);
                if (!stopInfo.Status.Equals(200))
                {
                    return result.SetError(stopInfo.Message);
                }

                var info = await jobCenter.AddScheduleJobAsync(model);
                if (!stopInfo.Status.Equals(200))
                {
                    return result.SetError(info.Message);
                }
            }


            //状态
            if (!taskInfo.RunStatus.Equals(model.RunStatus))
            {
                taskInfo.RunStatus = model.RunStatus;
                if (model.RunStatus != (int)TaskJobStatus.PauseJob)
                {
                    var stopInfo = await jobCenter.StopScheduleJobAsync(model.JobGroup, model.JobName);
                    if (!stopInfo.Status.Equals(200))
                    {
                        return result.SetError(stopInfo.Message);
                    }
                    var info = await jobCenter.AddScheduleJobAsync(model);
                    if (!stopInfo.Status.Equals(200))
                    {
                        return result.SetError(info.Message);
                    }
                }
                else
                {
                    var stopInfo = await jobCenter.StopScheduleJobAsync(model.JobGroup, model.JobName);
                    if (!stopInfo.Status.Equals(200))
                    {
                        return result.SetError(stopInfo.Message);
                    }
                }
            }
            //
            taskInfo.CreateAuthr = identity.Name;
            taskInfo.UpdateTime = DateTime.Now;
            taskInfo.EndRunTime = model.EndRunTime;
            taskInfo.StarRunTime = model.StarRunTime;
            taskInfo.JobGroup = model.JobGroup;
            taskInfo.JobName = model.JobName;

            base.Update(taskInfo, true);
            return result;
        }

        public ResponsResult PauseTask()
        {
            throw new NotImplementedException();
        }

        public ResponsResult StartTask()
        {
            throw new NotImplementedException();
        }
    }
}
