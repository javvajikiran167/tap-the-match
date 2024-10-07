using System;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine;
using System.Text;
using Custom.Utils;

namespace Custom.Services.Network.RestAPI
{
    public class APIService
    {
        IBaseRequirements _baseRequirements;
        private const string CONTENT_TYPE_JSON = "application/json";
        UnityWebRequest request;
        Coroutine _requestCorotuine;
        public APIService(IBaseRequirements baseRequirements)
        {
            _baseRequirements = baseRequirements;
        }

        public void MakeAPI<ResObj>(IAPIHeaders apiData, IAPICtrl<ResObj> apiCtrl)
        {
            Debug.Log("Making API: " + apiData.GetEndPoint());
            _requestCorotuine = CoroutineUtils.Instance.StartCoroutine(Request(apiCtrl, null, apiData));
        }

        public void MakeAPI<ResObj>(IAPIHeaders apiData, IAPICtrl<ResObj> apiCtrl, IAPIRetry apiRetry)
        {
            Debug.Log("Making API: " + apiData.GetEndPoint());
            _requestCorotuine = CoroutineUtils.Instance.StartCoroutine(Request(apiCtrl, apiRetry, apiData));
        }

        public void AbortIfInProgress()
        {
            try
            {
                if (_requestCorotuine != null)
                    CoroutineUtils.Instance.StopCoroutine(_requestCorotuine);

                LogUtils.Log("Abort API called");
                if (request != null)
                {
                    request.Abort();
                    request.Dispose();
                    LogUtils.Log("Abort API completed");
                }
            }
            catch (Exception)
            {
                LogUtils.Log("Abort API failed");
            }
        }

        private IEnumerator Request<ResObj>(IAPICtrl<ResObj> apiCtrl, IAPIRetry apiRetry, IAPIHeaders apiHeaders)
        {
            string uri;
            switch (apiHeaders.GetMethodType())
            {
                case UnityWebRequest.kHttpVerbGET:
                    uri = APIUtils.GenerateParamsUrl(_baseRequirements.GetBaseUrl(), apiHeaders.GetEndPoint(), (Dictionary<string, string>)apiCtrl.GetRequest());
                    request = UnityWebRequest.Get(uri);
                    SetHeaders(apiHeaders, request);
                    yield return request.SendWebRequest();
                    var requestString = string.Join(Environment.NewLine, (Dictionary<string, string>)apiCtrl.GetRequest());
                    HandleRespose(apiCtrl, apiRetry, request, apiHeaders.GetEndPoint(), requestString);
                    break;
                case UnityWebRequest.kHttpVerbPOST:
                case UnityWebRequest.kHttpVerbDELETE:
                    uri = _baseRequirements.GetBaseUrl() + apiHeaders.GetEndPoint();
                    request = new UnityWebRequest(uri);
                    request.method = apiHeaders.GetMethodType();
                    request.downloadHandler = new DownloadHandlerBuffer();
                    request.disposeDownloadHandlerOnDispose = true;
                    LogUtils.Log("Request for " + apiHeaders.GetMethodType() + " : " + JsonUtility.ToJson(apiCtrl.GetRequest()));
                    SetHeaders(apiHeaders, request);
                    AttachUploadHandler(request, apiCtrl.GetRequest());
                    yield return request.SendWebRequest();
                    HandleRespose(apiCtrl, apiRetry, request, apiHeaders.GetEndPoint(), JsonUtility.ToJson(apiCtrl.GetRequest()));
                    break;
            }

            request.Dispose();
        }

        private void AttachUploadHandler(UnityWebRequest request, object requestObj)
        {
            if (requestObj != null)
            {
                var reqData = JsonUtility.ToJson(requestObj);
                request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(reqData));
                request.disposeUploadHandlerOnDispose = true;
            }
        }

        private void SetHeaders(IAPIHeaders apiHeaders, UnityWebRequest request)
        {
            if (apiHeaders.GetAccessType() == AccessType.Auth)
                request.SetRequestHeader("X-ACCESS-TOKEN", _baseRequirements.GetAccessToken());

            request.SetRequestHeader("Content-Type", CONTENT_TYPE_JSON);
            request.timeout = _baseRequirements.GetAPITimeOutInSeconds();

            Dictionary<string, string> customHeaders = apiHeaders.GetCustomHeaders();
            if (customHeaders != null)
            {
                foreach (var item in customHeaders)
                {
                    request.SetRequestHeader(item.Key, item.Value);
                }
            }
        }

        private void HandleRespose<ResObj>(IAPICtrl<ResObj> apiCtrl, IAPIRetry apiRetry, UnityWebRequest request, string endPoint, string requestObj)
        {
            ResponseHandler responseHandler = new ResponseHandler(request, _baseRequirements.GetFailureHandlers());
            responseHandler.HandleResponse(apiCtrl, apiRetry, endPoint, requestObj);
        }
    }
}