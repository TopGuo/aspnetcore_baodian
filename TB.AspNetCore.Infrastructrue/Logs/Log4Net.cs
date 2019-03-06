using log4net;
using log4net.Config;
using log4net.Repository;
using System;
using System.Diagnostics;
using System.IO;
using TB.AspNetCore.Domain.Config;
using TB.AspNetCore.Infrastructrue.Config;

namespace TB.AspNetCore.Infrastructrue.Logs
{
    /// <summary>
    /// 日志服务
    /// </summary>
    public class Log4Net
    {
        /// <summary>
        /// 定义日志容器
        /// </summary>
        private static ILoggerRepository _repository = LogManager.CreateRepository(TbConstant.Log4RepositoryKey);
        /// <summary>
        /// 定义日志配置文件
        /// </summary>
        private static FileInfo LogConfig = new FileInfo(ConfigLocator.Instance[TbConstant.Log4netKey]);
        /// <summary>
        /// 定义接口参数
        /// </summary>
        /// <returns></returns>
        private static ILog SetLog()
        {
            var _MethodName = string.Empty;
            try
            {
                StackFrame _Call = new StackFrame(2);
                _MethodName = string.Format("{0}.{1}", _Call.GetMethod().ReflectedType.FullName, _Call.GetMethod().Name);
                var _St = new StackTrace().GetFrames();
                foreach (var _item in _St)
                {
                    if (_item.GetMethod().DeclaringType.ToString().EndsWith("Exception") && _St.Length > 2)
                    {
                        _MethodName = string.Format("{0}.{1}", _item.GetMethod().ReflectedType.FullName, _item.GetMethod().Name);
                        continue;
                    }
                }
            }
            catch { }
            XmlConfigurator.Configure(_repository, LogConfig);
            return LogManager.GetLogger(_repository.Name, _MethodName);
        }

        #region 异常日志
        /// <summary>
        /// 异常日志
        /// </summary>
        /// <param name="msg">错误信息</param>
        public static void Debug(object msg)
        {
            SetLog().Debug(msg);
        }
        /// <summary>
        /// 异常日志
        /// </summary>
        /// <param name="msg">错误信息</param>
        /// <param name="ex">异常信息</param>
        public static void Debug(object msg, Exception ex)
        {
            SetLog().Debug(msg, ex);
        }
        #endregion

        #region 错误日志
        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="msg">错误信息</param>
        public static void Error(object msg)
        {
            SetLog().Error(msg);
        }
        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="msg">错误信息</param>
        /// <param name="ex">异常信息</param>
        public static void Error(object msg, Exception ex)
        {
            SetLog().Error(msg, ex);
        }
        #endregion

        #region 数据日志
        /// <summary>
        /// 数据日志
        /// </summary>
        /// <param name="msg">错误信息</param>
        public static void Info(object msg)
        {
            SetLog().Info(msg);
        }
        /// <summary>
        /// 数据日志
        /// </summary>
        /// <param name="msg">错误信息</param>
        /// <param name="ex">异常信息</param>
        public static void Info(object msg, Exception ex)
        {
            SetLog().Info(msg, ex);
        }
        #endregion

        #region 警告日志
        /// <summary>
        /// 警告日志
        /// </summary>
        /// <param name="msg">错误信息</param>
        public static void Warn(object msg)
        {
            SetLog().Warn(msg);
        }
        /// <summary>
        /// 警告日志
        /// </summary>
        /// <param name="msg">错误信息</param>
        /// <param name="ex">异常信息</param>
        public static void Warn(object msg, Exception ex)
        {
            SetLog().Warn(msg, ex);
        }
        #endregion
    }
}
