using System;
using System.Collections.Generic;

namespace TB.AspNetCore.Domain.Entitys
{
    public partial class Informations
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public int Type { get; set; }
        public string Picture { get; set; }
        public string Description { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}
