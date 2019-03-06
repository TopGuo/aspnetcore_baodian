using System;
using System.Collections.Generic;

namespace TB.AspNetCore.Domain.Entitys
{
    public partial class Advertising
    {
        public string Id { get; set; }
        public string AdName { get; set; }
        public DateTime? BeginTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string AdPic { get; set; }
        public string AdDesc { get; set; }
        public bool? IsEnable { get; set; }
        public string AdLink { get; set; }
        public int AdLocation { get; set; }
    }
}
