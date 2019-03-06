using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TB.AspNetCore.Domain.Enums
{
    /// <summary>
    /// 消息状态
    /// </summary>
    public enum MsgStatus
    {
        /// <summary>
        /// 未审核
        /// </summary>
        [Description("未审核")]
        NoReviewed =0,
        /// <summary>
        /// 审核通过
        /// </summary>
        [Description("审核通过")]
        ReViewed =1,
        /// <summary>
        /// 禁用
        /// </summary>
        [Description("禁用")]
        Disabled=2,

        /// <summary>
        /// 未观看
        /// </summary>
        [Description("未观看")]
        NoLooked=3,

        /// <summary>
        /// 已观看
        /// </summary>
        [Description("已观看")]
        Looked =4,

        /// <summary>
        /// 已分享
        /// </summary>
        [Description("已分享")]
        Shared =5,
    }
}
