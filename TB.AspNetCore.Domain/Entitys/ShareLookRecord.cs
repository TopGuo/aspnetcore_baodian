using System;
using System.Collections.Generic;

namespace TB.AspNetCore.Domain.Entitys
{
    public partial class ShareLookRecord
    {
        public long Id { get; set; }
        public string AccountId { get; set; }
        public int Type { get; set; }
        public long Cid { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
