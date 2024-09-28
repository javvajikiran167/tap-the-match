namespace Custom.Services.Network.RestAPI
{
    public interface IBaseRequirements
    {
        string GetBaseUrl();
        int GetAPITimeOutInSeconds();
        string GetAccessToken();
        IFailureHandler GetFailureHandlers();
    }
}