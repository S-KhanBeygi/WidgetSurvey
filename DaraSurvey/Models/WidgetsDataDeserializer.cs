using DaraSurvey.Core.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace DaraSurvey.WidgetServices.Models
{
    public class WidgetsDataDeserializer
    {
        public static EditModelBase Deserialize(string serializedWidget)
        {
            if (string.IsNullOrEmpty(serializedWidget)) return null;

            JToken jToken;

            try
            {
                jToken = (JToken)JsonConvert.DeserializeObject(serializedWidget);
            }
            catch
            {
                throw new Exception("Deserializing failed");
            }

            if (jToken["type"] == null) throw new Exception("type field is required for all items");

            var typeFormat = "DaraSurvey.Widgets.{0}.EditModel";

            var binder = new TypeNameSerializationBinder(typeFormat);

            var typeName = jToken["type"]?.ToString();

            if (typeName == null) throw new Exception("\"type\" field is required");

            var type = binder.BindToType(null, typeName);

            if (type == null) return null;

            return (EditModelBase)jToken.ToObject(type);
        }
    }
}