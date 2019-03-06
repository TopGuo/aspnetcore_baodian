using System;

namespace TB.AspNetCore.Domain.Models.Web
{
    public class SmsSearchModel : Base.PageModelBase
    {
        public string IP { get; set; }
        public string Mobile { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Source { get; set; }
        public string Url { get; set; }

        public int? AccountId { get; set; }
    }
}
