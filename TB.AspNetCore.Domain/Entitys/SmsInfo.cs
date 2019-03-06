using System;
using System.Collections.Generic;

namespace TB.AspNetCore.Domain.Entitys
{
    public partial class SmsInfo
    {
        public long Id { get; set; }
        public string AccountId { get; set; }
        public string Contents { get; set; }
        public bool? IsUsed { get; set; }
        public string Code { get; set; }
        public string Mobile { get; set; }
        public string Ip { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
