using System;
using System.Collections.Generic;
using System.Text;

namespace TB.AspNetCore.Domain.Config
{
    public interface IConfigGeter
    {
        TConfig Get<TConfig>(string key);
        TConfig Get<TConfig>();
        String this[string key] { get; }
    }
}
