using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DaraSurvey.Core.BaseClasses
{
    public class JsonSeralizerSetting
    {
        public static JsonSerializerSettings SerializationSettings
        {
            get
            {
                return new JsonSerializerSettings
                {
                    ContractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() },
                    Converters = { new Newtonsoft.Json.Converters.StringEnumConverter() }
                };
            }
        }
    }
}
