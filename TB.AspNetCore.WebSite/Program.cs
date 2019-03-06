using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using TB.AspNetCore.Infrastructrue.AppBuilder;

namespace TB.AspNetCore.WebSite
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //新增默认配置
            Infrastructrue.AppBuilder.HostBuilder.ProgramMain(() =>
            {
                CreateWebHostBuilder(args).Build().Run();
            });
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .CreateDefaultBuilder()
            .UseUrls("http://127.0.0.1:50002")
                .UseStartup<Startup>();
    }
}
