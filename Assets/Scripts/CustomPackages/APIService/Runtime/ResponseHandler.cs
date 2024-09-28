using Custom.Utils;
using System;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;

namespace Custom.Services.Network.RestAPI
{
    public class ResponseHandler
    {

        [Serializable]
        public class FailureResponse
        {
            public string message;
        }

        private readonly UnityWebRequest _unityWebRequest;
        private readonly IFailureHandler _failureHandler;

        public ResponseHandler(UnityWebRequest unityWebRequest, IFailureHandler apiFailureHandler)
        {
            _unityWebRequest = unityWebRequest;
            _failureHandler = apiFailureHandler;
        }

        public void HandleResponse<ResObj>(IAPICtrl<ResObj> apiCtrl, IAPIRetry apiRetry, string endPoint, string requestObj)
        {
            LogUtils.Log("HandleResponse of: ", _unityWebRequest.uri.AbsoluteUri);
            LogUtils.Log("ResponseCode: ", _unityWebRequest.responseCode.ToString());
            LogUtils.Log("Response Result: ", _unityWebRequest.result.ToString());

            switch (_unityWebRequest.result)
            {
                case UnityWebRequest.Result.InProgress:
                    break;
                case UnityWebRequest.Result.Success:
                    HandleSuccess<ResObj>(apiCtrl.OnSuccess);
                    break;
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.ProtocolError:
                case UnityWebRequest.Result.DataProcessingError:
                default:
                    if (apiRetry != null)
                    {
                        HandleRetry(apiCtrl, apiRetry, endPoint, requestObj);
                    }
                    else
                    {
                        string extraInfo = "RequestObj: " + requestObj + ", responseCode: " + _unityWebRequest.responseCode.ToString() + ", API response: " + _unityWebRequest.downloadHandler.text;
                        HandleFailure(apiCtrl, endPoint, extraInfo);
                    }
                    break;
            }
        }

        private void HandleRetry<ResObj>(IAPICtrl<ResObj> apiCtrl, IAPIRetry apiRetry, string endPoint, string requestObj)
        {
            string extraInfo = "Retry Count: " + apiRetry.GetRetryCount() + ", requestObj: " + requestObj + ", responseCode: " + _unityWebRequest.responseCode.ToString() + ", API response: " + _unityWebRequest.downloadHandler.text;

            LogUtils.LogError("API FAILED: url: ", endPoint, "extraInfo:  " + extraInfo);
            if (ShouldRetry(apiRetry))
            {
                try
                {
                    apiRetry.Retry();
                }
                catch (Exception ex)
                {
                    LogUtils.Log("Couldn't perform rety operation due to : " + ex.ToString());
                    HandleFailure(apiCtrl, endPoint, extraInfo);
                }
            }
            else
            {
                HandleFailure(apiCtrl, endPoint, extraInfo);
            }
        }

        private void HandleSuccess<R>(Action<R> onSuccess)
        {
            string text = _unityWebRequest.downloadHandler.text;
            LogUtils.Log("on api success: ", text);
            onSuccess?.Invoke(JsonUtility.FromJson<R>(text));
        }

        private void HandleFailure<ResObj>(IAPICtrl<ResObj> apiCtrl, string endPoint, string extraInfo)
        {
            _failureHandler.handleFailureEvent(_unityWebRequest.downloadHandler.text, endPoint, extraInfo);

            string errorMsg = GetAPIFailureErrorMessage();
            if (apiCtrl.GetOnFailureOverrideMethod() != null)
            {
                apiCtrl.GetOnFailureOverrideMethod().Invoke(_unityWebRequest.responseCode, errorMsg);
            }
            else
            {
                _failureHandler.onAnyFailure(errorMsg);
            }
        }

        private bool ShouldRetry(IAPIRetry apiRetry)
        {
            return apiRetry.GetRetryCount() < apiRetry.GetMAXRetryCount() &&
                (_unityWebRequest.responseCode == (long)HttpStatusCode.BadGateway ||
                _unityWebRequest.responseCode == 0);
        }

        private string GetAPIFailureErrorMessage()
        {
            string errorMsg = string.Empty;
            if (_unityWebRequest.downloadHandler != null)
            {
                string responseBody = _unityWebRequest.downloadHandler.text;
                try
                {
                    //LogUtils.LogError("Failure response msg: " + responseBody);
                    errorMsg = JsonUtility.FromJson<FailureResponse>(responseBody).message;
                }
                catch (Exception)
                {
                }
            }

            if (string.IsNullOrEmpty(errorMsg))
            {
                if (_unityWebRequest.responseCode == 0)
                {
                    errorMsg = APIErrorMsgs.NO_INTERNET;
                }
                else if (_unityWebRequest.responseCode == (long)HttpStatusCode.NotFound)
                {
                    errorMsg = APIErrorMsgs.NOT_FOUND;
                }
                else if (_unityWebRequest.responseCode == (long)HttpStatusCode.BadRequest)
                {
                    errorMsg = APIErrorMsgs.BAD_REQUEST;
                }
                else if (_unityWebRequest.responseCode == (long)HttpStatusCode.RequestTimeout)
                {
                    errorMsg = APIErrorMsgs.REQUEST_TIME_OUT;
                }
                else if (_unityWebRequest.responseCode == (long)HttpStatusCode.Unauthorized)
                {
                    errorMsg = APIErrorMsgs.UNAUTHORIZED;
                }
                else if (_unityWebRequest.responseCode == (long)HttpStatusCode.Forbidden)
                {
                    errorMsg = APIErrorMsgs.FORBIDDEN;
                }
                else if (_unityWebRequest.responseCode == (long)HttpStatusCode.InternalServerError)
                {
                    errorMsg = APIErrorMsgs.INTERNAL_SERVER_ERROR;
                }
                else if (_unityWebRequest.responseCode == (long)HttpStatusCode.ServiceUnavailable)
                {
                    errorMsg = APIErrorMsgs.SERVICE_UNAVAILABLE;
                }
                else if (_unityWebRequest.responseCode == (long)HttpStatusCode.BadGateway)
                {
                    errorMsg = APIErrorMsgs.BAD_GATEWAY;
                }
                else
                {
                    errorMsg = APIErrorMsgs.GENERIC_FAILURE_MSG;
                }
            }

            return errorMsg;
        }
    }
}