using System.Collections.Generic;

namespace Custom.Services.Network.RestAPI
{
    public interface IAPIHeaders
    {
        public AccessType GetAccessType();
        public string GetEndPoint();
        public string GetMethodType();
        public Dictionary<string, string> GetCustomHeaders();
    }
}