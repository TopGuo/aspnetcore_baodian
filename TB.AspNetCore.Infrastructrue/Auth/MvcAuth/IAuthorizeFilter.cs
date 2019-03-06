using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace TB.AspNetCore.Infrastructrue.Auth.MvcAuth
{
    public interface IAuthorizeFilter
    {
        Task OnAuthorizedAsync(AuthorizationFilterContext context, string policy);
    }
}
