using TB.AspNetCore.Infrastructrue.Contexts;

namespace TB.AspNetCore.Application.Services
{
    /// <summary>
    /// 传入指定上下文
    /// </summary>
    public class BaseService: Repository<BaoDianContext>
    {
        
    }
}
