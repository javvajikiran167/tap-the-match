namespace Custom.Services.Network.RestAPI
{
    public interface IAPIRetry
    {
        void Retry();
        void IncrementRetryCount();
        int GetRetryCount();
        int GetMAXRetryCount();
    }
}
