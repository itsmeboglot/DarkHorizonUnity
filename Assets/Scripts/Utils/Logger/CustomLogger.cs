using UnityEngine;
using Utils.Extensions;

namespace Utils.Logger
{
    public static class CustomLogger
    {
        public static void Log(LogSource source, string logText, MessageType messageType = MessageType.Default)
        {
            Debug.Log($"[{GetColorfulSource(source)}]: {GetColorfulMessageByType(messageType, logText)}");
        }

        private static string GetColorfulSource(LogSource source)
        {
            return source switch
            {
                LogSource.MetaMask => source.ToString().WithColor(Color.magenta),
                LogSource.Server => source.ToString().WithColor(Color.cyan),
                LogSource.Unity => source.ToString().WithColor(Color.green),
                _ => "Undefined"
            };
        }
        
        private static string GetColorfulMessageByType(MessageType type, string message)
        {
            return type switch
            {
                MessageType.Warning => message.WithColor(Color.yellow),
                MessageType.Error => message.WithColor(Color.red),
                _ => message
            };
        }
    }
}
