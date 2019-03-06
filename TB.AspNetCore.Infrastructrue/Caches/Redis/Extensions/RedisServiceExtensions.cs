using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace TB.AspNetCore.Infrastructrue.Caches.Redis.Extensions
{
    public static class RedisServiceExtensions
    {
        #region 初始化参数
        private static readonly int _DefulatTime = 600; //默认有效期
        private static RedisOptions config;
        private static ConnectionMultiplexer connection;
        private static IDatabase _db;
        private static ISubscriber _sub;
        #endregion
        public static IServiceCollection AddRedisCacheService(
            this IServiceCollection serviceCollection,
            Func<RedisOptions, RedisOptions> redisOptions = null
            )
        {
            var _redisOptions = new RedisOptions();
            _redisOptions = redisOptions?.Invoke(_redisOptions) ?? _redisOptions;
            config = _redisOptions;
            connection = ConnectionMultiplexer.Connect(GetSystemOptions());
            _db = connection.GetDatabase(config.RedisIndex);
            _sub = connection.GetSubscriber();
            return serviceCollection;
        }
        
        #region 系统配置
        /// <summary>
        /// 获取系统配置
        /// </summary>
        /// <returns></returns>
        private static ConfigurationOptions GetSystemOptions()
        {
            var options = new ConfigurationOptions
            {
                AbortOnConnectFail = config.AbortOnConnectFail,
                AllowAdmin = config.AllowAdmin,
                ConnectRetry = config.ConnectRetry,//10,
                ConnectTimeout = config.ConnectTimeout,
                KeepAlive = config.KeepAlive,
                SyncTimeout = config.SyncTimeout,
                EndPoints = { config.RedisHost },
                ServiceName = config.RedisName,
            };
            if (!string.IsNullOrWhiteSpace(config.RedisPass))
            {
                options.Password = config.RedisPass;
            }
            return options;
        }
        #endregion

        //============
        #region 获取缓存
        /// <summary>
        /// 读取缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>数据类型/NULL</returns>
        public static object Get(string key)
        {
            return Get<object>(key);
        }
        /// <summary>
        /// 读取缓存
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <returns>数据类型/NULL</returns>
        public static T Get<T>(string key)
        {
            var value = _db.StringGet(key);
            return (value.IsNull ? default(T) : JsonTo<T>(value).Value);
        }
        #endregion

        #region 异步获取缓存
        /// <summary>
        /// 异步读取缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>object/NULL</returns>
        public static async Task<object> GetAsync(string key)
        {
            return await GetAsync<object>(key);
        }
        /// <summary>
        /// 异步读取缓存
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <returns>数据类型/NULL</returns>
        public static async Task<T> GetAsync<T>(string key)
        {
            var value = await _db.StringGetAsync(key);
            return (value.IsNull ? default(T) : JsonTo<T>(value).Value);
        }
        #endregion

        #region 同步转异步添加[I/O密集]
        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="data">数据</param>
        /// <param name="never">是否永久保存[true:是,false:保存10分钟]</param>
        public static bool Insert(string key, object data, bool never = false)
        {
            return InsertAsync(key, data, never).Result;
        }
        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="data">数据</param>
        /// <param name="never">是否永久保存[true:是,false:保存10分钟]</param>
        /// <returns>添加结果</returns>
        public static bool Insert<T>(string key, T data, bool never = false)
        {
            return InsertAsync<T>(key, data, never).Result;
        }
        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="data">数据</param>
        /// <param name="time">保存时间[单位:秒]</param>
        /// <returns>添加结果</returns>
        public static bool Insert(string key, object data, int time)
        {
            return InsertAsync(key, data, time).Result;
        }
        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="data">数据</param>
        /// <param name="time">保存时间[单位:秒]</param>
        /// <returns>添加结果</returns>
        public static bool Insert<T>(string key, T data, int time)
        {
            return InsertAsync<T>(key, data, time).Result;
        }
        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="data">数据</param>
        /// <param name="cachetime">缓存时间</param>
        /// <returns>添加结果</returns>
        public static bool Insert(string key, object data, DateTime cachetime)
        {
            return InsertAsync(key, data, cachetime).Result;
        }
        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="data">数据</param>
        /// <param name="cachetime">缓存时间</param>
        /// <returns>添加结果</returns>
        public static bool Insert<T>(string key, T data, DateTime cachetime)
        {
            return InsertAsync<T>(key, data, cachetime).Result;
        }
        #endregion

        #region 异步添加
        /// <summary>
        /// 添加缓存[异步]
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="data">数据</param>
        /// <param name="never">是否永久保存[true:是,false:保存10分钟]</param>
        /// <returns>添加结果</returns>
        public static async Task<bool> InsertAsync(string key, object data, bool never = false)
        {
            return await _db.StringSetAsync(key, ToJson(data), (never ? null : new TimeSpan?(TimeSpan.FromSeconds(_DefulatTime))));
        }
        /// <summary>
        /// 添加缓存[异步]
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="data">数据</param>
        /// <param name="never">是否永久保存[true:是,false:保存10分钟]</param>
        /// <returns>添加结果</returns>
        public static async Task<bool> InsertAsync<T>(string key, T data, bool never = false)
        {
            return await _db.StringSetAsync(key, ToJson<T>(data), (never ? null : new TimeSpan?(TimeSpan.FromSeconds(_DefulatTime))));
        }
        /// <summary>
        /// 添加缓存[异步]
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="data">数据</param>
        /// <param name="time">保存时间[单位:秒]</param>
        /// <returns>添加结果</returns>
        public static async Task<bool> InsertAsync(string key, object data, int time)
        {
            return await _db.StringSetAsync(key, ToJson(data), new TimeSpan?(TimeSpan.FromSeconds(time)));
        }
        /// <summary>
        /// 添加缓存[异步]
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="data">数据</param>
        /// <param name="time">保存时间[单位:秒]</param>
        /// <returns>添加结果</returns>
        public static async Task<bool> InsertAsync<T>(string key, T data, int time)
        {
            return await _db.StringSetAsync(key, ToJson<T>(data), new TimeSpan?(TimeSpan.FromSeconds(time)));
        }
        /// <summary>
        /// 添加缓存[异步]
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="data">数据</param>
        /// <param name="cachetime">缓存时间</param>
        /// <returns>添加结果</returns>
        public static async Task<bool> InsertAsync(string key, object data, DateTime cachetime)
        {
            return await _db.StringSetAsync(key, ToJson(data), new TimeSpan?(cachetime - DateTime.Now));
        }
        /// <summary>
        /// 添加缓存[异步]
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="data">数据</param>
        /// <param name="cachetime">缓存时间</param>
        /// <returns>添加结果</returns>
        public static async Task<bool> InsertAsync<T>(string key, T data, DateTime cachetime)
        {
            return await _db.StringSetAsync(key, ToJson<T>(data), new TimeSpan?(cachetime - DateTime.Now));
        }
        #endregion

        #region 验证缓存
        /// <summary>
        /// 验证缓存是否存在
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>验证结果</returns>
        public static bool Exists(string key)
        {
            return _db.KeyExists(key);
        }
        #endregion

        #region 异步验证缓存
        /// <summary>
        /// 验证缓存是否存在
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>验证结果</returns>
        public static async Task<bool> ExistsAsync(string key)
        {
            return await _db.KeyExistsAsync(key);
        }
        #endregion

        #region 移除缓存
        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>移除结果</returns>
        public static bool Remove(string key)
        {
            return _db.KeyDelete(key);
        }
        #endregion

        #region 异步移除缓存
        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>移除结果</returns>
        public static async Task<bool> RemoveAsync(string key)
        {
            return await _db.KeyDeleteAsync(key);
        }
        #endregion

        #region 队列发布
        /// <summary>
        /// 队列发布
        /// </summary>
        /// <param name="Key">通道名</param>
        /// <param name="data">数据</param>
        /// <returns>是否有消费者接收</returns>
        public static bool Publish(Models.RedisChannels Key, object data)
        {
            return _sub.Publish(Key.ToString(), ToJson(data)) > 0 ? true : false;
        }
        #endregion

        #region 队列接收
        /// <summary>
        /// 注册通道并执行对应方法
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="serviceCollection"></param>
        /// <param name="Key">通道名</param>
        /// <param name="doSub">方法</param>
        public static IServiceCollection Subscribe<T>(this IServiceCollection serviceCollection,Models.RedisChannels Key, DoSub doSub) where T : class
        {
            Task.Run(() =>
            {
                var _subscribe = connection.GetSubscriber();
                _subscribe.Subscribe(Key.ToString(), delegate (RedisChannel channel, RedisValue message)
                {
                    T t = Recieve<T>(message);
                    doSub(t);
                });
            });
            return serviceCollection;
        }
        #endregion

        #region 退订队列通道
        /// <summary>
        /// 退订队列通道
        /// </summary>
        /// <param name="Key">通道名</param>
        public static void UnSubscribe(Models.RedisChannels Key)
        {
            _sub.Unsubscribe(Key.ToString());
        }
        #endregion

        #region 数据转换
        /// <summary>
        /// JSON转换配置文件
        /// </summary>
        private static JsonSerializerSettings _jsoncfg = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = Formatting.None
        };
        /// <summary>
        /// 封装模型转换为字符串进行存储
        /// </summary>
        /// <param name="value">值</param>
        /// <returns></returns>
        private static string ToJson(object value)
        {
            return ToJson<object>(value);
        }
        /// <summary>
        /// 封装模型转换为字符串进行存储
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="value">值</param>
        /// <returns></returns>
        private static string ToJson<T>(T value)
        {
            return JsonConvert.SerializeObject(new Models.RedisData<T>
            {
                Value = value
            }, _jsoncfg);
        }
        /// <summary>
        /// 缓存字符串转为封装模型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        private static Models.RedisData<T> JsonTo<T>(string value)
        {
            return JsonConvert.DeserializeObject<Models.RedisData<T>>(value, _jsoncfg);
        }

        private static T Recieve<T>(string cachevalue)
        {
            T result = default(T);
            bool flag = !string.IsNullOrWhiteSpace(cachevalue);
            if (flag)
            {
                var cacheObject = JsonConvert.DeserializeObject<Models.RedisData<T>>(cachevalue, _jsoncfg);
                result = cacheObject.Value;
            }
            return result;
        }
        #endregion

        #region 方法委托
        /// <summary>
        /// 委托执行方法
        /// </summary>
        /// <param name="d"></param>
        public delegate void DoSub(object d);
        #endregion
    }
}
