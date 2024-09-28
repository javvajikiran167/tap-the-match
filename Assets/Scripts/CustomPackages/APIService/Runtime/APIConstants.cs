using UnityEngine.Networking;

namespace Custom.Services.Network.RestAPI
{
    public enum AccessType
    {
        Auth,
        Public
    }

    public struct APIRequestMethods
    {
        public const string Get = UnityWebRequest.kHttpVerbGET;
        public const string Post = UnityWebRequest.kHttpVerbPOST;
        public const string Delete = UnityWebRequest.kHttpVerbDELETE;
    }
}