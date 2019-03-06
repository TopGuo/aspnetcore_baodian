
using Microsoft.AspNetCore.Hosting;
using System;
using TB.AspNetCore.Infrastructrue.Config;
using TB.AspNetCore.Infrastructrue.Extensions;

namespace TB.AspNetCore.Infrastructrue.AppBuilder
{
    public static class HostBuilder
    {

        public static void ProgramMain(Action main)
        {
            main();
        }

        /// <summary>
        /// 加入自定义默认配置
        /// </summary>
        /// <param name="webHostBuilder"></param>
        /// <returns></returns>
        public static IWebHostBuilder CreateDefaultBuilder(this IWebHostBuilder webHostBuilder) =>
            webHostBuilder.ConfigureAppConfiguration(
                (ctx, config) => ConfigLocator.SetLocatorProvider(new ConfigGeter(config.Build())))
                .ConfigureServices((ctx, services) =>
                {
                    services
                    .AddRegisterContainer();
                });
    }
}
