using System;
using System.Collections.Generic;
using System.Text;

namespace TB.AspNetCore.Domain.Models.Api
{
    public class AddRpwModel
    {
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 提交图片
        /// </summary>
        public List<string> Pics { get; set; }

        /// <summary>
        /// 红包个数
        /// </summary>
        public int? RpwCounts { get; set; }

        /// <summary>
        /// 红包总金额
        /// </summary>
        public decimal? RpwTotalAmount { get; set; }

        /// <summary>
        /// 红包范围
        /// </summary>
        public Enums.AllOrLocal Type { get; set; }

        /// <summary>
        /// 是否添加外链
        /// </summary>
        public bool IsHasOutLink { get; set; } = false;

        /// <summary>
        /// 外链地址
        /// </summary>
        public string OutLinkStr { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        public decimal? Longtude { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        public decimal? Latitude { get; set; }
    }
}
