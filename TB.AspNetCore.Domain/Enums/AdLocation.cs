using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TB.AspNetCore.Domain.Enums
{
    public enum AdLocation
    {
        /// <summary>
        /// 首页
        /// </summary>
        [Description("首页")]
        Home = 1,
        /// <summary>
        /// 支付成功
        /// </summary>
        [Description("支付成功")]
        PaySuccessful,
        /// <summary>
        /// 登录页
        /// </summary>
        [Description("登录页")]
        Login,
        /// <summary>
        /// 公告
        /// </summary>
        [Description("公告")]
        GongGao,
        /// <summary>
        /// 启动页广告
        /// </summary>
        [Description("启动页")]
        SplashPage,
        /// <summary>
        /// banner广告
        /// </summary>
        [Description("banner广告")]
        Banner

    }
}
