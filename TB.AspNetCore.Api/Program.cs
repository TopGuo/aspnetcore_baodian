using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TB.AspNetCore.Infrastructrue.AppBuilder;

namespace TB.AspNetCore.Api
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
            .UseUrls("http://127.0.0.1:50009")
                .UseStartup<Startup>();
    }
}
