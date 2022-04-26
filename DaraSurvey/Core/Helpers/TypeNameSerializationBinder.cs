using Newtonsoft.Json.Serialization;
using System;

namespace DaraSurvey.Core.Helpers
{
    public class TypeNameSerializationBinder : ISerializationBinder
    {
        private readonly string TypeFormat;

        public TypeNameSerializationBinder(string typeForamat)
        {
            TypeFormat = typeForamat;
        }

        // --------------------

        public void BindToName(Type serializedType, out string assemblyName, out string typeName)
        {
            assemblyName = null;
            typeName = serializedType.Name;
        }

        // --------------------

        public Type BindToType(string assemblyName, string typeName)
        {
            string resolvedTypeName = string.Format(TypeFormat, typeName);

            return Type.GetType(resolvedTypeName, false);
        }
    }
}
