using System.Text;
using Newtonsoft.Json;
using Whimsy.Shared.Core;

namespace Whimsy.Shared.Serialization.Json
{
    public class JsonServerMessageSerializer : IServerMessageSerializer
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
}