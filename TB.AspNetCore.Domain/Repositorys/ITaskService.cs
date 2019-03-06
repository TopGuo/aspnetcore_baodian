using System.Threading.Tasks;
using TB.AspNetCore.Domain.Config;
using TB.AspNetCore.Domain.Models.Web;

namespace TB.AspNetCore.Domain.Repositorys
{
    public interface ITaskService
    {
        //获取任务列表
        ResponsResult GetTaskList();
        //添加任务
        Task<ResponsResult> AddTaskAsync(TaskScheduleModel model);
        //修改任务
        Task<ResponsResult> ModifyTaskAsync(TaskScheduleModel model);
        //删除任务
        Task<ResponsResult> DelTaskAsync(TaskScheduleModel model);
        //暂停任务
        ResponsResult PauseTask();
        //开启任务
        ResponsResult StartTask();
    }
}
