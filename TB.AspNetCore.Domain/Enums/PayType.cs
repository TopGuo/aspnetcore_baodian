using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TB.AspNetCore.Domain.Enums
{
    public enum PayType
    {
        /// <summary>
        /// 未支付
        /// </summary>
        [Description("未支付")]
        None = 0,
        /// <summary>
        /// 微信app支付
        /// </summary>
        [Description("微信app支付")]
        WeChat = 1,

        /// <summary>
        /// 支付宝
        /// </summary>
        [Description("支付宝app支付")]        
        Alipay = 2,

        /// <summary>
        /// 系统内支付
        /// </summary>
        [Description("系统内支付")]
        System = 3,
    }
}
