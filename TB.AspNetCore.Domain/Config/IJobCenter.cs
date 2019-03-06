using System.Threading.Tasks;
using TB.AspNetCore.Domain.Models.Web;

namespace TB.AspNetCore.Domain.Config
{
    public interface IJobCenter
    {
        /// <summary>
        /// 添加定时任务
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        Task<ResponsResult> AddScheduleJobAsync(TaskScheduleModel m);

        /// <summary>
        /// 暂停定时任务
        /// </summary>
        /// <param name="jobGroup"></param>
        /// <param name="jobName"></param>
        /// <returns></returns>
        Task<ResponsResult> StopScheduleJobAsync(string jobGroup, string jobName);

        /// <summary>
        /// 恢复定时任务
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        Task<ResponsResult> RunScheduleJobAsync(TaskScheduleModel m);
    }
}
