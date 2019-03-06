using System.Threading.Tasks;
using Quartz;
using TB.AspNetCore.Infrastructrue.Logs;

namespace TB.AspNetCore.Infrastructrue.Tasks.Quartz
{
    public class HttpJob : IJob
    {
        
        /// <summary>
        /// 通过group和name判断是要执行哪个任务  具体（任务执行逻辑）业务逻辑写后面
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Execute(IJobExecutionContext context)
        {
            await Task.Run(() =>
            {
                var name = context.JobDetail.Key.Name;
                var group = context.JobDetail.Key.Group;
                if (group=="xx1"&&name=="xx2")
                {
                    //do something
                }

                if (group == "xx2" && name == "xx3")
                {
                    //do something also
                }
                //组：z，名称c
                if (group.Equals("z")&&name.Equals("c"))
                {
                    Log4Net.Info($"你好棒");
                }
                Log4Net.Info($"执行任务_Name:{name}_Grop:{group}");
            });
        }
    }
}
