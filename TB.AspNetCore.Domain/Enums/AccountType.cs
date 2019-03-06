using System.ComponentModel;

namespace TB.AspNetCore.Domain.Enums
{
    public enum AccountType
    {
        /// <summary>
        /// 管理员
        /// </summary>
        [Description("超级管理员")]
        Admin = 1,
        /// <summary>
        /// 普通用户
        /// </summary>
        [Description("普通用户")]
        StandardUser = 2,
    }
}
