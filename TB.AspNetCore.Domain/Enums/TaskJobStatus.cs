using System.ComponentModel;

namespace TB.AspNetCore.Domain.Enums
{
    public enum TaskJobStatus
    {
        [Description("初始值")]
        UnKnow=0,
        [Description("执行任务中")]
        DoJob = 1,
        [Description("暂停任务中")]
        PauseJob = 2,
        [Description("开启任务失败")]
        JobFail = 3,
        [Description("任务关闭")]
        JobClose = 4,
        [Description("任务删除")]
        JobHasDel = 5,
    }
}
