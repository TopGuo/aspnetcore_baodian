using TB.AspNetCore.Domain.Models.Base;

namespace TB.AspNetCore.Domain.Models.Api
{
    public class SystemSettingModel : SettingsBase
    {
        public override string Name => "SystemSetting";
        /// <summary>
        /// 注册赠送优惠券的金额
        /// </summary>
        public decimal CouponForRegist { get; set; } = 10;
        /// <summary>
        /// 客服电话
        /// </summary>
        public string ConsumerHotline { get; set; } = "0311-12345678";
        /// <summary>
        /// 邀请者得优惠券金额
        /// </summary>
        public decimal InviteePackage { get; set; } = 10;
        /// <summary>
        /// 被邀请者得到优惠券的金额
        /// </summary>
        public decimal InvitedPackage { get; set; } = 10;
        /// <summary>
        /// 用户头像保存地址
        /// </summary>
        public string UserHeadImagePath { get; set; } = "/account/";
        /// <summary>
        /// 网址地址
        /// </summary>
        public string WebSite { get; set; } = "http://xingchen.52expo.top";

        /// <summary>
        /// Api地址
        /// </summary>
        public string ApiSite { get; set; } = "http://img.52expo.top";

        /// <summary>
        /// 网址地址 测试
        /// </summary>
        public string WebSiteTest { get; set; } = "http://beta_web.icbqida.com/";
        /// <summary>
        /// 举报图片地址
        /// </summary>
        public string ReportImagePath { get; set; } = "/Report/";
        public string WeChatAppCallBack { get; set; } = "http://api.icbqida.com/Payment/WeChatAppCallBack";

        public string WeChatWebCallBack { get; set; } = "http://api.icbqida.com/Payment/WeChatWebCallBack";

        public string AlipayCallBack { get; set; } = "http://api.icbqida.com/Payment/AlipayCallBack";

        public string AdImagePath { get; set; } = "/Ad/";
    }
}
