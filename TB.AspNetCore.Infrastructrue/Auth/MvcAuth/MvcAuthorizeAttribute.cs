using Microsoft.AspNetCore.Authorization;

namespace TB.AspNetCore.Infrastructrue.Auth.MvcAuth
{
    public class MvcAuthorizeAttribute : AuthorizeAttribute
    {
        public string LoginPath
        {
            get;
            set;
        }

        public string AccessDeniedPath
        {
            get;
            set;
        }

        public MvcAuthorizeAttribute()
            : base()
        {
        }
    }
}
