using System;
using System.Collections.Generic;
using System.Text;
using TB.AspNetCore.Domain.Models.Base;

namespace TB.AspNetCore.Domain.Models.Api
{
    public class AndroidVersion : SettingsBase
    {
        public override string Name => "AndroidVersion";

        /// <summary>
        /// APP名称
        /// </summary>
        public string AppName { get; set; } = "xxAPP";
        /// <summary>
        /// APP版本号
        /// </summary>
        public string AppVersion { get; set; } = "1.0";
        /// <summary>
        /// APP下载地址
        /// </summary>
        public string AppDownLink { get; set; } = "http://test";
        /// <summary>
        /// 是否必须更新
        /// </summary>
        public bool Upgrade { get; set; } = true;
        /// <summary>
        /// APP更新描述
        /// </summary>
        public string AppDescription { get; set; } = "测试";
    }

    public class IosVersion : SettingsBase
    {
        public override string Name => "IosVersion";

        /// <summary>
        /// APP名称
        /// </summary>
        public string AppName { get; set; } = "xxAPP";
        /// <summary>
        /// APP版本号
        /// </summary>
        public string AppVersion { get; set; } = "1.0";
        /// <summary>
        /// APP下载地址
        /// </summary>
        public string AppDownLink { get; set; } = "http://test";
        /// <summary>
        /// 是否必须更新
        /// </summary>
        public bool Upgrade { get; set; } = true;
        /// <summary>
        /// APP更新描述
        /// </summary>
        public string AppDescription { get; set; } = "测试";
    }
}
