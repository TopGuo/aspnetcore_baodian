using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TB.AspNetCore.Domain.Enums
{
    /// <summary>
    /// 订单类型
    /// </summary>
    public enum OrderType
    {
        /// <summary>
        /// 红包充值
        /// </summary>
        [Description("红包充值")]
        RpwRecharge = 1
    }

    /// <summary>
    /// 收支类型
    /// </summary>
    public enum OrderTypeSearch
    {
        /// <summary>
        /// 收入
        /// </summary>
        [Description("收入")]
        Income = 1,
        /// <summary>
        /// 支出
        /// </summary>
        [Description("支出")]
        Outlay = 2
    }
}
