using System.ComponentModel;

namespace TB.AspNetCore.Domain.Enums
{
    public enum InformationType
    {
        /// <summary>
        /// 公司介绍
        /// </summary>
        [Description("公司介绍")]
        About = 1,
        /// <summary>
        /// 使用指南(列表)
        /// </summary>
        [Description("使用指南(列表)")]
        UseGuide = 2
    }
}
