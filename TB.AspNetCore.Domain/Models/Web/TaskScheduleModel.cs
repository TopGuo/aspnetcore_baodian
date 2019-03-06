using System;

namespace TB.AspNetCore.Domain.Models.Web
{
    public class TaskScheduleModel
    {
        public string Id { get; set; }
        public string JobGroup { get; set; }
        public string JobName { get; set; }
        public string CronExpress { get; set; }
        public DateTime StarRunTime { get; set; }
        public DateTime? EndRunTime { get; set; }
        public DateTime? NextRunTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public DateTime? CreateTime { get; set; }
        
        public int RunStatus { get; set; }
        public string _RunStatus { get; set; }
    }
}
