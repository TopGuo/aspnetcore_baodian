using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TB.AspNetCore.Domain.Enums
{
    /// <summary>
    /// 上传文件类别枚举
    /// </summary>
    public enum FileType
    {
        [Description("头像")]
        Head = 1,

        [Description("身份证正面照片")]
        IdCardFace = 2,

        [Description("身份证反面照片")]
        IdCardBack = 3,

        [Description("意见反馈")]
        Feedbacks = 4,

        [Description("其他")]
        Other = 5,
    }
}
