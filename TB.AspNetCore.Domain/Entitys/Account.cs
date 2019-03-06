using System;
using System.Collections.Generic;

namespace TB.AspNetCore.Domain.Entitys
{
    public partial class Account
    {
        public string Id { get; set; }
        public string HeaderPic { get; set; }
        public string NicikName { get; set; }
        public string PassWord { get; set; }
        public string WxNick { get; set; }
        public string RealName { get; set; }
        public string TelPhone { get; set; }
        public string Unionid { get; set; }
        public string OpenId { get; set; }
        public int RewarCounts { get; set; }
        public string Token { get; set; }
        public int AccountStatus { get; set; }
        public int AccountType { get; set; }
        public string ReferId { get; set; }
        public string Email { get; set; }
        public DateTime LastLoginTime { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
