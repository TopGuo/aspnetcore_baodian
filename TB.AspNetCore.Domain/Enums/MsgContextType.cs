using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TB.AspNetCore.Domain.Enums
{
    public enum MsgContextType
    {
        /// <summary>
        /// 红包墙
        /// </summary>
        [Description("红包墙")]
        Rpw=1,
        /// <summary>
        /// 热点
        /// </summary>
        [Description("热点")]
        hot =2
    }
}
