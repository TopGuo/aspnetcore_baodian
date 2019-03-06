using Microsoft.AspNetCore.DataProtection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TB.AspNetCore.Domain.DataProtection
{
    public class Base256DataProtector : IDataProtector, IDataProtectionProvider
    {
        public static readonly UTF8Encoding SecureUtf8Encoding = new UTF8Encoding(false, true);

        private Base256DataProtectionProvider _base256Provider;

        /// <summary>
        ///
        /// </summary>
        protected List<DataProtectionKeys> _dataKeys = new List<DataProtectionKeys>();

        private static DataProtectionKeys currentKey;

        private string IV;

        internal string[] Purposes
        {
            get;
        }

        public DataProtectionKeys CurrentKey
        {
            get
            {
                if (currentKey == null || currentKey.IsRevoked)
                {
                    DataProtectionKeys dataProtectionKeys = GetKeys().FirstOrDefault();
                    if (dataProtectionKeys == null || dataProtectionKeys.IsRevoked)
                    {
                        DateTime now;
                        if (dataProtectionKeys == null || _base256Provider.Options.CreateKey)
                        {
                            DataProtectionKeys obj = new DataProtectionKeys
                            {
                                CreationDate = DateTime.Now,
                                ActivationDate = DateTime.Now
                            };
                            now = DateTime.Now;
                            obj.ExpirationDate = now.Add(_base256Provider.Options.Expires);
                            obj.MasterKey = Base256Encoders.CreateConfusionCodes();
                            dataProtectionKeys = obj;
                            File.WriteAllText(Path.Combine(_base256Provider.Options.KeyDirectory.FullName, $"key-{Guid.NewGuid()}.json"), JsonConvert.SerializeObject(dataProtectionKeys));
                            _dataKeys.Insert(0, dataProtectionKeys);
                        }
                        else
                        {
                            DataProtectionKeys dataProtectionKeys2 = new DataProtectionKeys();
                            now = DateTime.Now;
                            dataProtectionKeys2.ExpirationDate = now.AddDays(-1.0);
                            now = DateTime.Now;
                            dataProtectionKeys2.CreationDate = now.AddDays(-1.0);
                            now = DateTime.Now;
                            dataProtectionKeys2.ActivationDate = now.AddDays(-1.0);
                            dataProtectionKeys = dataProtectionKeys2;
                        }
                    }
                    currentKey = dataProtectionKeys;
                }
                return currentKey;
            }
        }

        public Base256DataProtector(Base256DataProtectionProvider base256Provider, string[] originalPurposes, string newPurpose)
        {
            if (string.IsNullOrEmpty(newPurpose))
            {
                throw new ArgumentNullException("newPurpose");
            }
            _base256Provider = base256Provider;
            Purposes = ConcatPurposes(originalPurposes, newPurpose);
            if (CurrentKey != null)
            {
                return;
            }
            throw new Exception("Cannot generate the Key");
        }

        private static string[] ConcatPurposes(string[] originalPurposes, string newPurpose)
        {
            if (originalPurposes != null && originalPurposes.Length != 0)
            {
                string[] array = new string[originalPurposes.Length + 1];
                Array.Copy(originalPurposes, 0, array, 0, originalPurposes.Length);
                array[originalPurposes.Length] = newPurpose;
                return array;
            }
            return new string[1]
            {
                newPurpose
            };
        }

        public IDataProtector CreateProtector(string purpose)
        {
            if (string.IsNullOrEmpty(purpose))
            {
                throw new ArgumentNullException("purpose");
            }
            return new Base256DataProtector(_base256Provider, Purposes, purpose);
        }

        public byte[] Protect(byte[] plaintext)
        {
            if (plaintext != null && plaintext.Length != 0)
            {
                try
                {
                    string iV = GetIV();
                    byte[] bytes = Base256Encoders.SecureUtf8Encoding.GetBytes(iV);
                    byte[] array = new byte[bytes.Length + plaintext.Length];
                    bytes.CopyTo(array, 0);
                    plaintext.CopyTo(array, bytes.Length);
                    return Base256Encoders.EncryptToBase256(array, CurrentKey.MasterKey);
                }
                catch
                {
                    return new byte[0];
                }
            }
            return new byte[0];
        }

        public string Protect(string plaintext, bool toBase64 = false)
        {
            if (string.IsNullOrEmpty(plaintext))
            {
                return string.Empty;
            }
            try
            {
                if (toBase64)
                {
                    return Base256Encoders.EncryptToBase64($"{GetIV()}{plaintext}", CurrentKey.MasterKey);
                }
                return Base256Encoders.EncryptToBase256($"{GetIV()}{plaintext}", CurrentKey.MasterKey);
            }
            catch
            {
                return string.Empty;
            }
        }

        protected string GetIV()
        {
            if (string.IsNullOrEmpty(IV))
            {
                string text = string.Empty;
                for (int i = 0; i < 8; i++)
                {
                    text += CurrentKey.MasterKey[i << 3].ToString();
                }
                IV = text;
            }
            return IV;
        }

        public byte[] Unprotect(byte[] protectedData)
        {
            try
            {
                if (protectedData != null && protectedData.Length != 0)
                {
                    byte[] bytes = Base256Encoders.DecryptFromBase256(protectedData, CurrentKey.MasterKey);
                    string _string = Base256Encoders.SecureUtf8Encoding.GetString(bytes);
                    string iV = GetIV();
                    if (_string.Length > iV.Length && _string.StartsWith(iV))
                    {
                        _string = _string.Substring(iV.Length);
                        return Base256Encoders.SecureUtf8Encoding.GetBytes(_string);
                    }
                    return new byte[0];
                }
                return new byte[0];
            }
            catch
            {
                return new byte[0];
            }
        }

        public string Unprotect(string protectedData, bool fromBase64 = false)
        {
            try
            {
                if (string.IsNullOrEmpty(protectedData))
                {
                    return string.Empty;
                }
                string text;
                if (fromBase64)
                {
                    try
                    {
                        text = Base256Encoders.DecryptFromBase64(protectedData, CurrentKey.MasterKey);
                    }
                    catch
                    {
                        List<DataProtectionKeys> keys = GetKeys();
                        if (keys.Count > 1)
                        {
                            text = Base256Encoders.DecryptFromBase64(protectedData, keys.Skip(1).FirstOrDefault().MasterKey);
                            return text;
                        }
                        return string.Empty;
                    }
                }
                else
                {
                    try
                    {
                        text = Base256Encoders.DecryptFromBase256(protectedData, CurrentKey.MasterKey);
                    }
                    catch
                    {
                        List<DataProtectionKeys> keys2 = GetKeys();
                        if (keys2.Count > 1)
                        {
                            text = Base256Encoders.DecryptFromBase256(protectedData, keys2.Skip(1).FirstOrDefault().MasterKey);
                            return text;
                        }
                        return string.Empty;
                    }
                }
                string iV = GetIV();
                if (text.StartsWith(iV))
                {
                    return text.Substring(iV.Length);
                }
                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        private List<DataProtectionKeys> GetKeys()
        {
            if (_dataKeys.Count == 0)
            {
                FileInfo[] files = _base256Provider.Options.KeyDirectory.GetFiles("key-*.json");
                List<DataProtectionKeys> list = new List<DataProtectionKeys>();
                FileInfo[] array = files;
                for (int i = 0; i < array.Length; i++)
                {
                    using (StreamReader streamReader = array[i].OpenText())
                    {
                        string input = streamReader.ReadToEnd();
                        list.Add(JsonConvert.DeserializeObject<DataProtectionKeys>(input));
                    }
                }
                _dataKeys = (from x in list
                             orderby x.CreationDate descending
                             select x).ToList();
            }
            return _dataKeys;
        }
    }
}
