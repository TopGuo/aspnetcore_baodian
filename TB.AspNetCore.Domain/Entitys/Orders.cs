using System;
using System.Collections.Generic;

namespace TB.AspNetCore.Domain.Entitys
{
    public partial class Orders
    {
        public string Id { get; set; }
        public string AccountId { get; set; }
        public string SourceId { get; set; }
        public string OrderCode { get; set; }
        public int OrderType { get; set; }
        public decimal Amount { get; set; }
        public int OrderStatus { get; set; }
        public int PayType { get; set; }
        public string AlipayId { get; set; }
        public decimal? PayAmount { get; set; }
        public decimal? ActualAmount { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}
