using System.ComponentModel;

namespace TB.AspNetCore.Domain.Enums
{
    public enum ErrorCode
    {
        /// <summary>
        /// 账户不可用
        /// </summary>
        [Description("账户不可用")]
        AccountDisabled = 10,
        /// <summary>
        /// 已存在
        /// </summary>
        [Description("用户,数据已存在")]
        Existed = 100,

        #region 300
        /// <summary>
        /// 无效的输入
        /// </summary>
        [Description("无效的输入")]
        InvalidData = 300,
        /// <summary>
        /// 签名错误
        /// </summary>
        [Description("签名错误")]
        InvalidSign = 301,
        /// <summary>
        /// 不正确的身份证号码
        /// </summary>
        [Description("不正确的身份证号码")]
        InvalidIdCard = 302,
        /// <summary>
        /// 不正确的手机号码
        /// </summary>
        [Description("不正确的手机号码")]
        InvalidMobile = 303,
        /// <summary>
        /// 不正确的姓名
        /// </summary>
        [Description("不正确的姓名")]
        InvalidFullName = 304,
        /// <summary>
        /// 格式错误
        /// </summary>
        [Description("格式错误")]
        InvalidFormat = 305,
        /// <summary>
        /// 密码不正确
        /// </summary>
        [Description("密码不正确")]
        InvalidPassword = 306,
        /// <summary>
        /// token不正确
        /// </summary>
        [Description("token不正确")]
        InvalidToken = 307,
        /// <summary>
        /// 纬度不正确(-90~90)
        /// </summary>
        [Description("纬度不正确(-90~90)")]
        InvalidLatitude = 308,
        /// <summary>
        /// 经度不正确(-180~180)
        /// </summary>
        [Description("经度不正确(-180~180)")]
        InvalidLongitude = 309,
        /// <summary>
        /// 城市信息不正确
        /// </summary>
        [Description("城市信息不正确")]
        InvalidCity = 310,
        #endregion

        #region 400
        /// <summary>
        /// 请求错误
        /// </summary>
        [Description("请求错误")]
        BadRequest = 400,
        /// <summary>
        /// 未登录
        /// </summary>
        [Description("未登录")]
        Unauthorized = 401,

        /// <summary>
        /// 权限不足,拒绝访问
        /// </summary>
        [Description("权限不足,拒绝访问")]
        Forbidden = 403,

        /// <summary>
        /// 不存在
        /// </summary>
        [Description("数据,页面不存在")]
        NotFound = 404, 
        #endregion

        #region 500
        /// <summary>
        /// 系统错误
        /// </summary>
        [Description("系统错误")]
        SystemError = 500,
        /// <summary>
        /// 操作失败
        /// </summary>
        [Description("操作失败")]
        OperationError = 505,
        /// <summary>
        /// 数据有误
        /// </summary>
        [Description("数据有误")]
        DataError = 506,
        /// <summary>
        /// 操作频繁
        /// </summary>
        [Description("操作频繁")]
        FrequentOperation = 507,
        /// <summary>
        /// 不能为空
        /// </summary>
        [Description("不能为空")]
        CannotEmpty = 508,
        /// <summary>
        /// 已过期
        /// </summary>
        [Description("已过期")]
        Expired = 509, 
        #endregion

        #region 600
        /// <summary>
        /// 请重新登录
        /// </summary>
        [Description("请重新登录")]
        Relogin = 600,

        /// <summary>
        /// 暂未开放
        /// </summary>
        [Description("暂未开放")]
        NoDo = 605,
        #endregion

        #region 业务错误码
        /// <summary>
        /// 消息内容不能为空
        /// </summary>
        [Description("消息内容不能为空")]
        RpwMsgEmpty=10001,
        /// <summary>
        /// 图片不能为空
        /// </summary>
        [Description("图片不能为空")]
        PicsEmpty = 10002,
        /// <summary>
        /// 红包金额不能为空
        /// </summary>
        [Description("红包金额不能为空")]
        AmountEmpty = 10003,
        /// <summary>
        /// 金额不能为负数
        /// </summary>
        [Description("不能为负数")]
        AmountPub = 10004,
        /// <summary>
        /// 总数不能为空
        /// </summary>
        [Description("总数不能为空")]
        TotalAmount = 10005,
        /// <summary>
        /// 单包金额过低
        /// </summary>
        [Description("单包金额过低")]
        SinglePackage = 10005,

        #endregion
    }
}
