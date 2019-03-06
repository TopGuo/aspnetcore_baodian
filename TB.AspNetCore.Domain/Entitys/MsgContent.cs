using System;
using System.Collections.Generic;

namespace TB.AspNetCore.Domain.Entitys
{
    public partial class MsgContent
    {
        public long Id { get; set; }
        public string AccountId { get; set; }
        public int AreaType { get; set; }
        public int ContextType { get; set; }
        public decimal? TotalPrice { get; set; }
        public int? TotalCounts { get; set; }
        public int? RemainCounts { get; set; }
        public string Content { get; set; }
        public string Pics { get; set; }
        public decimal? Longtude { get; set; }
        public decimal? Latiude { get; set; }
        public int Status { get; set; }
        public DateTime CreateTime { get; set; }
        public bool? IsHasOutLink { get; set; }
        public string OutLink { get; set; }
    }
}
