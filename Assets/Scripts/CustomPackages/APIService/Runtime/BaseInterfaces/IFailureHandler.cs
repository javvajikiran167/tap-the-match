namespace Custom.Services.Network.RestAPI
{
    public interface IFailureHandler
    {
        void handleDataProcessingError(string displayMsg);
        void handleProtocalError(string displayMsg);
        void handleConnectionError(string displayMsg);
        void onAnyFailure(string displayMsg);
        void onRequestTimeOut(string displayMsg);
        void onNoInternet(string displayMsg);
        void handleFailureEvent(string apiResponse, string endPoint, string extraInfo);
    }
}