using System;
using System.Collections.Generic;
using TB.AspNetCore.Domain.Enums;

namespace TB.AspNetCore.Domain.Models.Api
{
    public class MsgContentModel
    {
        /// <summary>
        /// 总金额
        /// </summary>
        public decimal? TotalPrice { get; set; }
        /// <summary>
        /// 红包总数
        /// </summary>
        public int? TotalCounts { get; set; }
        /// <summary>
        /// 红包剩余个数
        /// </summary>
        public int? RemainCounts { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 图片s
        /// </summary>
        public List<string> Pics { get; set; }
        /// <summary>
        /// 消息状态
        /// </summary>
        public MsgStatus Status { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 是否有外链
        /// </summary>
        public bool? IsHasOutLink { get; set; }
        /// <summary>
        /// 外链
        /// </summary>
        public string OutLink { get; set; }
    }
}
