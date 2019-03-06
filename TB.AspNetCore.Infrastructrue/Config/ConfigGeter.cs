using Microsoft.Extensions.Configuration;
using System;
using TB.AspNetCore.Domain.Config;

namespace TB.AspNetCore.Infrastructrue.Config
{
    /// <summary>
    /// 配置文件提供者
    /// </summary>
    public class ConfigGeter : IConfigGeter
    {
        private readonly IConfiguration _configuration;

        public ConfigGeter(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public TConfig Get<TConfig>(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(key));
            var section = _configuration.GetSection(key);
            return section.Get<TConfig>();
        }

        public TConfig Get<TConfig>()
        {
            return Get<TConfig>(typeof(TConfig).FullName);
        }

        public string this[string key] => _configuration[key];
    }
}
