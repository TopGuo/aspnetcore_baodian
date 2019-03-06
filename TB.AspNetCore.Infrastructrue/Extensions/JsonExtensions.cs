using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace TB.AspNetCore.Infrastructrue.Extensions
{
    public static class JsonExtensions
    {
        public static JsonSerializerSettings DefaultJsonSettings()
        {
            try
            {
                IOptions<MvcJsonOptions> val = ServiceCollectionExtension.Get<IOptions<MvcJsonOptions>>();
                if (val != null)
                {
                    JsonSerializerSettings val2 = val.Value.SerializerSettings;
                    JsonSerializerSettings val3 = new JsonSerializerSettings();
                    val3.SerializationBinder = val2.SerializationBinder;
                    val3.CheckAdditionalContent = val2.CheckAdditionalContent;
                    val3.ConstructorHandling = val2.ConstructorHandling;
                    val3.Context = val2.Context;
                    val3.ContractResolver = val2.ContractResolver;
                    val3.Converters = val2.Converters;
                    val3.Culture = val2.Culture;
                    val3.DateFormatHandling = val2.DateFormatHandling;
                    val3.DateFormatString = val2.DateFormatString;
                    val3.DateParseHandling = val2.DateParseHandling;
                    val3.DateTimeZoneHandling = val2.DateTimeZoneHandling;
                    val3.DefaultValueHandling = val2.DefaultValueHandling;
                    val3.EqualityComparer = val2.EqualityComparer;
                    val3.Error = val2.Error;
                    val3.FloatFormatHandling = val2.FloatFormatHandling;
                    val3.FloatParseHandling = val2.FloatParseHandling;
                    val3.Formatting = val2.Formatting;
                    val3.MaxDepth = val2.MaxDepth;
                    val3.MetadataPropertyHandling = val2.MetadataPropertyHandling;
                    val3.MissingMemberHandling = val2.MissingMemberHandling;
                    val3.NullValueHandling = val2.NullValueHandling;
                    val3.ObjectCreationHandling = val2.ObjectCreationHandling;
                    val3.PreserveReferencesHandling = val2.PreserveReferencesHandling;
                    val3.ReferenceLoopHandling = val2.ReferenceLoopHandling;
                    val3.ReferenceResolverProvider = val2.ReferenceResolverProvider;
                    val3.StringEscapeHandling = val2.StringEscapeHandling;
                    val3.TraceWriter = val2.TraceWriter;
                    val3.TypeNameHandling = val2.TypeNameHandling;
                    return val3;
                }
                throw new KeyNotFoundException("MvcJsonOptions.SerializerSettings");
            }
            catch
            {
                JsonSerializerSettings val4 = new JsonSerializerSettings();
                val4.NullValueHandling = NullValueHandling.Ignore;
                val4.DateFormatString = "MM/dd/yyyy HH:mm";
                //val4.ContractResolver = new CamelCaseNamingStrategy(true, true); ;
                val4.Formatting = Newtonsoft.Json.Formatting.None;
                val4.MissingMemberHandling = MissingMemberHandling.Ignore;
                val4.MaxDepth = 32;
                val4.TypeNameHandling = TypeNameHandling.None;
                val4.Culture = new CultureInfo("en-us");
                val4.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                return val4;
            }
        }
        public static string GetJson(this object obj)
        {
            return obj.GetJson(delegate
            {
            });
        }

        public static string GetJson(this object obj, Action<JsonSerializerSettings> jsonSettings)
        {
            if (obj == null)
            {
                return "{}";
            }
            JsonSerializerSettings val = DefaultJsonSettings();
            jsonSettings(val);
            return JsonConvert.SerializeObject(obj, val);
        }

        public static object GetModel(this string input, string path = "")
        {
            if (string.IsNullOrEmpty(input))
            {
                return null;
            }
            input = input.FormatJson();
            JsonSerializerSettings val = DefaultJsonSettings();
            object obj = JsonConvert.DeserializeObject(input, val);
            if (!string.IsNullOrEmpty(path))
            {
                JArray val2;
                if ((val2 = (obj as JArray)) != null)
                {
                    return (object)val2.First.SelectToken(path);
                }
                JObject val3;
                if ((val3 = (obj as JObject)) != null)
                {
                    return (object)val3.SelectToken(path);
                }
                throw new MvcException($"model is {obj.GetType().Name}");
            }
            return obj;
        }
        public static T GetModel<T>(this string input, string path = "")
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return default(T);
            }
            input = input.FormatJson();
            JsonSerializerSettings val = DefaultJsonSettings();
            if (string.IsNullOrEmpty(path))
            {
                if (!input.StartsWith("["))
                {
                    return JsonConvert.DeserializeObject<T>(input, val);
                }
                return JsonConvert.DeserializeObject<List<T>>(input, val).FirstOrDefault();
            }
            object obj = JsonConvert.DeserializeObject(input);
            JArray val2;
            if ((val2 = (obj as JArray)) != null)
            {
                return val2.First.SelectToken(path).ToObject<T>();
            }
            JObject val3;
            if ((val3 = (obj as JObject)) != null)
            {
                return val3.SelectToken(path).ToObject<T>();
            }
            throw new MvcException($"jtoken is {obj.GetType().Name}");
        }

        public static List<T> GetModelList<T>(this string input, string path = "")
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return new List<T>();
            }
            input = input.FormatJson();
            JsonSerializerSettings val = DefaultJsonSettings();
            if (string.IsNullOrEmpty(path))
            {
                if (!input.StartsWith("["))
                {
                    return new List<T>
                    {
                        JsonConvert.DeserializeObject<T>(input, val)
                    };
                }
                return JsonConvert.DeserializeObject<List<T>>(input, val);
            }
            List<T> list = (List<T>)new List<T>();
            object obj = JsonConvert.DeserializeObject(input);
            JArray source;
            if ((source = (obj as JArray)) != null)
            {
                ((IEnumerable<JToken>)source).ToList().ForEach((Action<JToken>)delegate (JToken t)
                {
                    ((List<T>)list).Add(t.SelectToken(path).ToObject<T>());
                });
                return (List<T>)list;
            }
            JObject val2;
            if ((val2 = (obj as JObject)) != null)
            {
                ((List<T>)list).Add(val2.SelectToken(path).ToObject<T>());
                return (List<T>)list;
            }
            throw new MvcException($"jtoken is {obj.GetType().Name}");
        }


        public static string GetString(this Enum _enum)
        {
            if (_enum == null)
            {
                return string.Empty;
            }
            Type type = _enum.GetType();
            string name = Enum.GetName(type, _enum);
            if (!string.IsNullOrEmpty(name))
            {
                DescriptionAttribute customAttribute = ((MemberInfo)type.GetField(name)).GetCustomAttribute<DescriptionAttribute>();
                if (customAttribute != null)
                {
                    return customAttribute.Description;
                }
                return name;
            }
            List<FieldInfo> list = (from t in type.GetFields()
                                    where t.FieldType == type
                                    select t).ToList();
            List<string> description = new List<string>();
            list.ForEach(delegate (FieldInfo t)
            {
                if (_enum.HasFlag((Enum)t.GetValue(_enum)))
                {
                    DescriptionAttribute customAttribute2 = ((MemberInfo)t).GetCustomAttribute<DescriptionAttribute>();
                    if (customAttribute2 != null && !string.IsNullOrEmpty(customAttribute2.Description))
                    {
                        description.Add(customAttribute2.Description);
                    }
                    else
                    {
                        description.Add(t.Name);
                    }
                }
            });
            return string.Join(",", description);
        }

        public static List<Tuple<int, string>> GetStrings<T>() where T : struct
        {
            List<Tuple<int, string>> list = new List<Tuple<int, string>>();
            Type type = typeof(T);
            (from t in type.GetFields()
             where t.FieldType == type
             select t).ToList().ForEach((Action<FieldInfo>)delegate (FieldInfo t)
             {
                 DescriptionAttribute customAttribute = CustomAttributeExtensions.GetCustomAttribute<DescriptionAttribute>((MemberInfo)t);
                 if (customAttribute != null && !string.IsNullOrEmpty(customAttribute.Description))
                 {
                     list.Add(new Tuple<int, string>((int)t.GetValue(null), customAttribute.Description));
                 }
                 else
                 {
                     list.Add(new Tuple<int, string>((int)t.GetValue(null), t.Name));
                 }
             });
            return list;
        }

        public static List<Tuple<int, string>> GetStrings(this Type enumType)
        {
            if (!enumType.GetTypeInfo().IsEnum)
            {
                throw new Exception("The type is not Enum");
            }
            List<Tuple<int, string>> list = new List<Tuple<int, string>>();
            (from t in enumType.GetFields()
             where t.FieldType == enumType
             select t).ToList().ForEach(delegate (FieldInfo t)
             {
                 DescriptionAttribute customAttribute = ((MemberInfo)t).GetCustomAttribute<DescriptionAttribute>();
                 if (customAttribute != null && !string.IsNullOrEmpty(customAttribute.Description))
                 {
                     list.Add(new Tuple<int, string>((int)t.GetValue(null), customAttribute.Description));
                 }
                 else
                 {
                     list.Add(new Tuple<int, string>((int)t.GetValue(null), t.Name));
                 }
             });
            return list;
        }

        public static string GetString(this HttpContext context)
        {
            string text = context.GetStringAsync().GetAwaiter().GetResult();
            if (!string.IsNullOrEmpty(text))
            {
                text = text.Trim();
            }
            return text;
        }

        public static async Task<string> GetStringAsync(this HttpContext context)
        {
            HttpRequest request = context.Request;

            //#region todo：memarysteam
            if (!(request.Body is MemoryStream))
            {
                MemoryStream memoryStream = new MemoryStream();
                memoryStream.Position = 0;
                request.Body.CopyTo(memoryStream);
                request.Body = memoryStream;
            }
            //#endregion
            request.Body.Position = 0;
            string text = await new StreamReader(request.Body).ReadToEndAsync();
            if (!string.IsNullOrEmpty(text))
            {
                text = text.Trim();
            }

            return text;
        }

        public static Dictionary<string, StringValues> GetValues(this HttpContext context)
        {
            return context.GetValuesAsync().GetAwaiter().GetResult();
        }

        public static async Task<Dictionary<string, StringValues>> GetValuesAsync(this HttpContext context)
        {
            Dictionary<string, StringValues> values = new Dictionary<string, StringValues>(StringComparer.OrdinalIgnoreCase);
            string text = await context.GetStringAsync();
            if (string.IsNullOrEmpty(text))
            {
                return values;
            }
            object value = JsonConvert.DeserializeObject(text.FormatJson());
            AddToBackingStore(values, value);
            return values;
        }

        private static void AddToBackingStore(Dictionary<string, StringValues> store, object value)
        {
            JProperty val;
            JValue val2;
            if (value is JObject || value is JArray)
            {
                foreach (JToken item in (IEnumerable<JToken>)(value as JToken))
                {
                    AddToBackingStore(store, item);
                }
            }
            else if ((val = (value as JProperty)) != null)
            {
                AddToBackingStore(store, (object)val.Value);
            }
            else if ((val2 = (value as JValue)) != null && val2.Value != null)
            {
                store[val2.Path] = val2.Value.ToString();
            }
        }

        public static string FormatXmlToJson(this string xml)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);
            string text = JsonConvert.SerializeXmlNode(xmlDocument, 0, true);

            Match match = new Regex("(\\{\"#cdata-section\":([^\\}]+)\\})", RegexOptions.IgnoreCase).Match(text);
            StringBuilder stringBuilder = new StringBuilder(text);
            while (match.Success)
            {
                stringBuilder.Replace(match.Result("$1"), match.Result("$2"));
                match = match.NextMatch();
            }
            stringBuilder.Replace("\"@", "\"").Replace("\"#cdata-section\"", "\"__data\"").Replace("\"#text\"", "\"__data\"");
            return stringBuilder.ToString();
        }

        public static string FormatJson(this string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return json;
            }
            json = json.Trim('\n', '\r', '\t', ' ');
            if (!json.StartsWith("<html ", StringComparison.OrdinalIgnoreCase) && !json.StartsWith("<!DOCTYPE", StringComparison.Ordinal))
            {
                if (json.StartsWith("<"))
                {
                    return json.FormatXmlToJson();
                }
                if (!json.StartsWith("[") && !json.StartsWith("{"))
                {
                    return JsonConvert.SerializeObject((object)new Dictionary<string, string>
                    {
                        {
                            "json",
                            json
                        }
                    });
                }
                return json;
            }
            return JsonConvert.SerializeObject((object)new Dictionary<string, string>
            {
                {
                    "html",
                    json
                }
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static bool IsAjaxRequest(this HttpContext context)
        {
            bool flag = false;
            if (!string.IsNullOrEmpty(context.Request.ContentType))
            {
                flag = (context.Request.ContentType.Equals("application/json") || context.Request.ContentType.Equals("text/json"));
            }
            if (!flag)
            {
                StringValues val = context.Request.Headers["X-Requested-With"];
                int num;
                if (val.Count <= 0)
                {
                    val = context.Request.Headers["ASP.NET-Core-Requested-With"];
                    num = ((val.Count > 0) ? 1 : 0);
                }
                else
                {
                    num = 1;
                }
                flag = ((byte)num != 0);
            }
            if (!flag)
            {
                flag = (context.Request.HasFormContentType || (!string.IsNullOrEmpty(context.Request.Method) && !context.Request.Method.Equals("get", StringComparison.OrdinalIgnoreCase)));
            }
            return flag;
        }
    }
}
