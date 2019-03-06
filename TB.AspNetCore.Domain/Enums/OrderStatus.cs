using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TB.AspNetCore.Domain.Enums
{
    /// <summary>
    /// 订单状态
    /// </summary>
    public enum OrderStatus
    {
        /// <summary>
        /// 未支付
        /// </summary>
        [Description("未支付")]
        Pending = 0,
        /// <summary>
        /// 支付成功
        /// </summary>
        [Description("支付成功")]
        Success = 1,
        /// <summary>
        /// 支付失败
        /// </summary>
        [Description("支付失败")]
        Faild = 2,

        /// <summary>
        /// 已取消
        /// </summary>
        [Description("已取消")]
        Cancelled = 3,
    }
}
