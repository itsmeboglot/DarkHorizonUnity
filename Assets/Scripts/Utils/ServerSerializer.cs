using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Whimsy.Shared.Core;

namespace Utils
{
    public class ServerSerializer : IServerMessageSerializer
    {
        #region Consts

        private static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,
            SerializationBinder = new NetCoreSerializationBinder(),
            Formatting = Formatting.Indented,
            Error = (sender, eventArgs) =>
            {
                var errorContext = eventArgs.ErrorContext;
                if (errorContext.Error is JsonSerializationException ex)
                    eventArgs.ErrorContext.Handled = true;
            }
        };

        #endregion

        #region Implementations

        public byte[] Serialize(object response)
        {
            var json = JsonConvert.SerializeObject(response, JsonSerializerSettings);
            var bytes = Encoding.UTF8.GetBytes(json);

            return bytes;
        }

        public T Deserialize<T>(byte[] data)
        {
            var json = Encoding.UTF8.GetString(data);
            var request = JsonConvert.DeserializeObject<T>(json, JsonSerializerSettings);

            return request;
        }

        #endregion
    }
    
    internal class NetCoreSerializationBinder : DefaultSerializationBinder
    {
        #region Consts

        private static readonly Regex Regex = new Regex(
            @"System\.Private\.CoreLib(, Version=[\d\.]+)?(, Culture=[\w-]+)(, PublicKeyToken=[\w\d]+)?");

        private static readonly Dictionary<Type, (string assembly, string type)> Cache =
            new Dictionary<Type, (string, string)>();

        #endregion

        #region Implementations

        public override void BindToName(Type serializedType, out string assemblyName, out string typeName)
        {
            base.BindToName(serializedType, out assemblyName, out typeName);

            if (Cache.TryGetValue(serializedType, out var name))
            {
                assemblyName = name.assembly;
                typeName = name.type;
            }
            else
            {
                if (assemblyName.Contains("System.Private.CoreLib"))
                    assemblyName = Regex.Replace(assemblyName, "mscorlib");

                if (typeName.Contains("System.Private.CoreLib"))
                    typeName = Regex.Replace(typeName, "mscorlib");

                if(!Cache.ContainsKey(serializedType))
                    Cache.Add(serializedType, (assemblyName, typeName));
            }
        }

        #endregion
    }
}