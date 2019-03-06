using System;
using System.Collections.Generic;
using System.Text;

namespace TB.AspNetCore.Domain.Config
{
    /// <summary>
    /// 预配置常量
    /// </summary>
    public class TbConstant
    {
        #region 临时文件上传地址

        public const string UPLOAD_TEMP_PATH = "Upload_Temp";


        #endregion
        #region 给客户端颁发key
        /// <summary>
        /// 上传key
        /// </summary>
        public const string UploadKey = "C1B702BD-C1D9-4103-A110-BF1A68179C19";
        /// <summary>
        /// ioskey
        /// </summary>
        public const string iOS_ApiKey = "972F5D56-296B-4BD7-8CB6-49617A441A99";
        /// <summary>
        /// androidkey
        /// </summary>
        public const string Android_ApiKey = "506DDE1D-4642-4859-9226-E26B098FC02B";
        /// <summary>
        /// wechatkey
        /// </summary>
        public const string WeChatApp_ApiKey = "23ACA89E-0169-44A1-8D08-64AD3EC4A551";
        #endregion

        #region 默认头像
        /// <summary>
        /// 默认头像图片相对服务器路径
        /// </summary>
        public const string DefaultHeadPicture = "/images/head.png";
        #endregion

        #region WebSite 站点地址
        /// <summary>
        /// WebSite
        /// </summary>
        public const string WebSiteKey = "WebSite";
        #endregion

        #region 验证码等待时间
        /// <summary>
        /// 验证码等待时间
        /// </summary>
        public const int CanRequestAfterSecond = 120;
        /// <summary>
        /// 验证码有效时长（分钟）
        /// </summary>
        public const int CodeValidMinutes = 2;
        
        #endregion
        /// <summary>
        /// 验证码
        /// </summary>
        public const string WEBSITE_VERIFICATION_CODE = "ValidateCode";
        /// <summary>
        /// 是否展示所有数据标志
        /// </summary>
        public const string ShowAllDataCookie = "ShowAllData";
        /// <summary>
        /// 上次登录路径
        /// </summary>
        public const string LAST_LOGIN_PATH = "LAST_LOGIN_PATH";
        /// <summary>
        /// 公司名称
        /// </summary>
        public const string CompanyName = "星辰无限";
        /// <summary>
        /// 网站授权协议
        /// </summary>
        public const string WEBSITE_AUTHENTICATION_SCHEME = "Web";
        /// <summary>
        /// 日志配置容器名
        /// </summary>
        public const string Log4RepositoryKey = "TBLog";

        /// <summary>
        /// Log4Net 日志配置文件
        /// </summary>
        public const string Log4netKey = "Log4NetConfig";

        /// <summary>
        /// Redis配置options
        /// </summary>
        public const string RedisOptionsKey = "RedisConfig";

        /// <summary>
        /// RedisCon配置option
        /// </summary>
        public const string RedisConOptionsKey = "RedisConConfig";
    }
}
