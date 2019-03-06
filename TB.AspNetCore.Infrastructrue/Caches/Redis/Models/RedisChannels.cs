using System;
using System.Collections.Generic;
using System.Text;

namespace TB.AspNetCore.Infrastructrue.Caches.Redis.Models
{
    /// <summary>
    /// 通道名称
    /// </summary>
    public enum RedisChannels
    {
        /// <summary>
        /// 会员注册
        /// </summary>
        MemberRegister,
        /// <summary>
        /// 测试发布订阅
        /// </summary>
        TestPubSub


    }
}
