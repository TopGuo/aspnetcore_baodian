using System;
using System.Collections.Generic;
using System.Text;
using TB.AspNetCore.Domain.Config;
using TB.AspNetCore.Domain.Enums;

namespace TB.AspNetCore.Domain.Repositorys
{
    /// <summary>
    /// 会员服务
    /// </summary>
    public interface IMemberService
    {
        /// <summary>
        /// 邀请注册
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="mobile"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        ResponsResult Register(string accountId, string mobile, string code);

        /// <summary>
        /// 验证码登录/注册
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <param name="code">验证码</param>
        /// <param name="sign">签名</param>
        /// <param name="apiKey">apikey</param>
        /// <param name="source">来源类型</param>
        /// <returns></returns>
        ResponsResult Login(string mobile, string code, string sign, string apiKey, SourceType source);
    }
}
