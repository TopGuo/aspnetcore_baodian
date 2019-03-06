using System;
using TB.AspNetCore.Domain.Enums;

namespace TB.AspNetCore.Domain.Models.Api
{
    public class AdvertisingModel
    {
        /// <summary>
        /// 唯一id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string AdName { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? BeginTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 图片s
        /// </summary>
        public string AdPic { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string AdDesc { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public AdLocation AdLocation { get; set; }
    }
}
