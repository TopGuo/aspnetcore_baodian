using System;
using System.ComponentModel;
using TB.AspNetCore.Domain.Enums;

namespace TB.AspNetCore.Domain.Models.Api
{
    public class ApiAccountModel
    {

        /// <summary>
        /// 真实姓名
        /// </summary>
        [Description("真实姓名")]
        public string FullName { get; set; }


        /// <summary>
        /// 昵称
        /// </summary>
        [Description("昵称")]
        public string NickName { get; set; }
        /// <summary>
        /// 账户状态
        /// </summary>
        [Description("账户状态")]
        public AccountStatus AccountStatus { get; set; }

        [Description("用户头像")]
        public string HeadPicture { get; set; }

        /// <summary>
        /// 账户创建时间
        /// </summary>
        [Description("账户创建时间")]
        public DateTime CreateTime { get; set; }

        [Description("手机号/登录名")]
        public string Mobile { get; set; }


        /// <summary>
        /// 最后一次登录时间
        /// </summary>
        [Description("最后一次登录时间")]
        public DateTime? LastLoginTime { get; set; }

        /// <summary>
        /// 是否有未支付订单
        /// </summary>
        [Description("是否有未支付订单")]
        public bool HasOrders { get; set; }
        /// <summary>
        /// 可用现金
        /// </summary>
        [Description("充值金额/钱包余额")]
        public decimal RechargeAmount { get; set; }
        /// <summary>
        /// 可取现金额
        /// </summary>
        [Description("收益金额")]
        public decimal IncomeAmount { get; set; }
        /// <summary>
        /// 总金额/钱包历史总额
        /// </summary>
        [Description("总金额/钱包历史总额")]
        public decimal TotalAmount { get { return this.RechargeAmount + this.IncomeAmount; } }

        [Description("token,用户是否登录的唯一标识")]
        public string Token { get; set; }

        [Description("当前认证类型")]
        public ApproveModel ApproveModel { get; set; }

        /// <summary>
        /// 人工认证失败原因
        /// </summary>
        [Description("人工认证失败原因")]
        public string Reason { get; set; }

        [Description("是否有新消息")]
        public bool HasNewInfo { get; set; }
    }
}
