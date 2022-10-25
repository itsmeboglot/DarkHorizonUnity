using System;

namespace Gateways.Connection
{
    [Serializable]
    public struct ServerUris
    {
        public Http http;
        public Socket socket;

        [Serializable]
        public struct Http
        {
            public string identityUri;
            public string playerUri;
        }
        
        [Serializable]
        public struct Socket
        {
            public string lobbySocketUri;
            public string gameSocketUri;
        }
    }
}