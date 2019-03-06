using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TB.AspNetCore.Domain.Models.Api
{
    public class ApiSharedModel
    {
        [Description("邀请链接")] public string Link { get; set; }

        [Description("邀请码")] public string Code { get; set; }

        [Description("被邀请者得到优惠券金额")] public decimal InvitedAmount { get; set; }

        [Description("邀请者得到优惠券金额")] public decimal InviteeAmount { get; set; }

        [Description("邀请标题")] public string Title { get { return "邀请好友奖励说明"; } }

        [Description("内容")] public string Content { get; set; }

        [Description("图标")] public string Picture { get; set; }
    }
}
