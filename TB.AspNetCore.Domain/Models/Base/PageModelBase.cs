using System;
using System.Collections.Generic;
using System.Text;

namespace TB.AspNetCore.Domain.Models.Base
{
    public class PageModelBase
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
