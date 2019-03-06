using Newtonsoft.Json;

namespace TB.AspNetCore.Domain.Models.Base
{
    public abstract class SettingsBase
    {
        [JsonIgnore]
        public abstract string Name { get; }
        public override string ToString()
        {
            return Name;
        }
    }
}
