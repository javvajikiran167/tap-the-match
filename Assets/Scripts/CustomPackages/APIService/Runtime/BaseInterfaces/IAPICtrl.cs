using System;

namespace Custom.Services.Network.RestAPI
{
    public interface IAPICtrl<T>
    {
        object GetRequest();
        void OnSuccess(T apiResponse);
        Action<long, string> GetOnFailureOverrideMethod();
    }
}
