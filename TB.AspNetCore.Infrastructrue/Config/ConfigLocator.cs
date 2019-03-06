using System;
using TB.AspNetCore.Domain.Config;

namespace TB.AspNetCore.Infrastructrue.Config
{
    public class ConfigLocator
    {
        private readonly IConfigGeter _currentServiceProvider;
        private static IConfigGeter _serviceProvider;

        public ConfigLocator(IConfigGeter currentServiceProvider)
        {
            _currentServiceProvider = currentServiceProvider;
        }

        public static ConfigLocator Instance => new ConfigLocator(_serviceProvider);

        public static void SetLocatorProvider(IConfigGeter serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public TConfig Get<TConfig>(String key)
        {
            return _currentServiceProvider.Get<TConfig>(key);
        }

        public TConfig Get<TConfig>()
        {
            return _currentServiceProvider.Get<TConfig>(typeof(TConfig).Name);
        }

        public String this[string key] => _currentServiceProvider[key];
    }
}
