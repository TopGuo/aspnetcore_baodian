using System;
using TB.AspNetCore.Domain.Enums;

namespace TB.AspNetCore.Domain.Models.Web
{
    public class AdManagerViewSearch :Base.PageModelBase
    {
        public string Id { get; set; }
        public string AdName { get; set; }
        public DateTime? BeginTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string AdPic { get; set; }
        public AdLocation AdLocation { get; set; }
        public string AdLocationName { get; set; }
        public string AdDesc { get; set; }

        /// <summary>
        /// 广告启用禁用 0禁用 1启用  添加广告默认1启用
        /// </summary>
        public bool IsEnable { get; set; }

        /// <summary>
        /// 条转链接
        /// </summary>
        public string AdLink { get; set; }
    }
}
