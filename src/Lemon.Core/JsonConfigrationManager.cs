using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.IO;

namespace Lemon.Core.Config
{
    internal class JsonConfigrationManager<TConfigNode>
    {
        private IDictionary<string, IDictionary<string, object>> _raw;

        public JsonConfigrationManager(string file)
        {
            var text = File.ReadAllText(file);

            _raw = JsonConvert.DeserializeObject<Dictionary<string, IDictionary<string, object>>>(text, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
        }

        private IDictionary<string, object> InternalGetSetting(string name)
        {
            if (!_raw.Keys.Contains(name))
            {
                return null;
            }

            var setting = new Dictionary<string, object>();

            var values = _raw[name];

            Copy(from: values, to: setting);

            if (values.Keys.Contains("@extend"))
            {
                var extendFrom = values["@extend"] as string;

                var extendSetting = InternalGetSetting(extendFrom);

                Copy(from: extendSetting, to: setting);

                setting.Remove("@extend");
            }

            return setting;
        }

        private static void Copy(IDictionary<string, object> from, IDictionary<string, object> to)
        {
            foreach(var kv in from)
            {
                if(!to.Keys.Contains(kv.Key))
                {
                    to.Add(kv);
                }
            }
        }

        public TConfigNode GetSetting(string name)
        {
            var temp = InternalGetSetting(name);

            var plainText = JsonConvert.SerializeObject(temp);

            return JsonConvert.DeserializeObject<TConfigNode>(plainText);
        }
    }
}
