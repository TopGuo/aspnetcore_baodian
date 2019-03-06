using System;
using System.Collections.Generic;
using System.Text;

namespace TB.AspNetCore.Infrastructrue.Caches.Redis
{
    public class RedisOptions
    {
        /// <summary>
        /// 数据库地址
        /// </summary>
        public string RedisHost { get; set; }
        /// <summary>
        /// 数据库用户名
        /// </summary>
        public string RedisName { get; set; }
        /// <summary>
        /// 数据库密码
        /// </summary>
        public string RedisPass { get; set; }

        /// <summary>
        /// 库
        /// </summary>
        public int RedisIndex { get; set; }

        /// <summary>
        /// 异步连接等待时间
        /// </summary>
        public int ConnectTimeout { get; set; } = 600;

        /// <summary>
        /// 同步连接等待时间
        /// </summary>
        public int SyncTimeout { get; set; } = 600;

        /// <summary>
        /// 最大连接数
        /// </summary>
        public int KeepAlive { get; set; } = 30;

        /// <summary>
        /// 连接重试次数
        /// </summary>
        public int ConnectRetry { get; set; } = 10;

        /// <summary>
        /// 获取或设置是否应显式通知连接/配置超时通过TimeoutException
        /// </summary>
        public bool AbortOnConnectFail { get; set; } = true;

        /// <summary>
        /// 是否允许管理员操作
        /// </summary>
        public bool AllowAdmin { get; set; } = true;
    }
}
