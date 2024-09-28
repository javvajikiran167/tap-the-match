namespace Custom.Services.Network.RestAPI
{
    public static class APIErrorMsgs
    {
        public const string NOT_FOUND = "Couldn't find the URL. Our developers have already been notified and they are on it";
        public const string BAD_REQUEST = "Bad Request. Our developers have already been notified and they are on it";
        public const string REQUEST_TIME_OUT = "Request is timed out, please check your internet connection and try again";
        public const string UNAUTHORIZED = "Unauthorized, please try closing the app and login again";
        public const string FORBIDDEN = "Forbidden, you are not allowed to perform this action";
        public const string INTERNAL_SERVER_ERROR = "An internal error has occurred, our developers have already been notified and they are on it";
        public const string SERVICE_UNAVAILABLE = "ServiceUnavailable, Please try again in some time";
        public const string NO_INTERNET = "Please check your internet connection and try again";
        public const string BAD_GATEWAY = "BadGateway. We're working on getting it fixed as soon as we can";
        public const string GENERIC_FAILURE_MSG = "Something went wrong. We're working on getting it fixed as soon as we can";
    }
}
