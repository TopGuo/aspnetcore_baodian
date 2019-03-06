using System;
using System.Collections.Generic;

namespace TB.AspNetCore.Domain.Entitys
{
    public partial class BudgetInfo
    {
        public int Id { get; set; }
        public string OrderId { get; set; }
        public int Type { get; set; }
        public string AccountId { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
