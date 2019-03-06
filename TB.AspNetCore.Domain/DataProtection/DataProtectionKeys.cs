using Newtonsoft.Json;
using System;

namespace TB.AspNetCore.Domain.DataProtection
{
    public class DataProtectionKeys
    {
        public DateTime CreationDate
        {
            get;
            set;
        }

        public DateTime ActivationDate
        {
            get;
            set;
        }

        public DateTime ExpirationDate
        {
            get;
            set;
        }

        public string MasterKey
        {
            get;
            set;
        }

        [JsonIgnore]
        public bool IsRevoked
        {
            get
            {
                return ExpirationDate < DateTime.Now;
            }
        }
    }
}
