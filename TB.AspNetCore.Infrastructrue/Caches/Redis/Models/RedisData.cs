using System;
using System.Collections.Generic;
using System.Text;

namespace TB.AspNetCore.Infrastructrue.Caches.Redis.Models
{
    /// <summary>
    /// 缓存数据模型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RedisData<T>
    {
        /// <summary>
        /// 值
        /// </summary>
        public T Value { get; set; }
    }
}
