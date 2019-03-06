using System;
using TB.AspNetCore.Domain.Enums;

namespace TB.AspNetCore.Domain.Models
{
    public class TokenModel
    {
        public string Id { get; set; }
        public string Mobile { get; set; }
        public SourceType Source { get; set; }
        public string Code { get; set; }
        public DateTime? Time { get; set; }
        /// <summary>
        /// 用户类型,相当于角色使用
        /// </summary>
        public AccountType Type { get; set; }
    }
}
