using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Custom.Utils
{
    public class InternetChecker : MonoBehaviour
    {
        public static InternetChecker instance;

        public static event Action<bool> NetworkStateChanged;
        private const float timeBetweenChecks = 1.0f;
        private const int timeOutForChecking = 3;
        private string _pingUrl;
        private bool isInternetAvailable;

        const int MAX_FAILS_ALLOWED_IN_STREAK = 3;
        int _currentFailedStreak = 0;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            isInternetAvailable = true;
        }

        public void StartThePing(string pingUrl)
        {
            if (string.IsNullOrEmpty(pingUrl))
            {
                pingUrl = "https://www.google.com";
            }
            _pingUrl = pingUrl;


            Debug.Log("ping URL: " + _pingUrl);       
            //Start out with the assumption that the internet is not available.
            isInternetAvailable = true;
            StartCoroutine(GetRequest(_pingUrl));
        }

        IEnumerator GetRequest(string _pingUrl)
        {
            while (true)
            {
                using (UnityWebRequest webRequest = UnityWebRequest.Get(_pingUrl))
                {
                    webRequest.timeout = timeOutForChecking;
                    // Request and wait for the desired page.
                    yield return webRequest.SendWebRequest();
                    switch (webRequest.result)
                    {
                        case UnityWebRequest.Result.ConnectionError:
                        case UnityWebRequest.Result.DataProcessingError:
                        case UnityWebRequest.Result.ProtocolError:
                            Debug.Log("InternetChecker No internet case - Result: responseCode, error " + webRequest.result + ", " + webRequest.responseCode + ", " + webRequest.error);
                            _currentFailedStreak++;
                            if (_currentFailedStreak >= MAX_FAILS_ALLOWED_IN_STREAK)
                            {
                                InternetIsNotAvailable();
                            }
                            break;
                        case UnityWebRequest.Result.Success:
                            InternetAvailable();
                            _currentFailedStreak = 0;
                            break;
                    }
                }

                yield return new WaitForSeconds(timeBetweenChecks);
            }
        }

        public void InternetIsNotAvailable()
        {
            //Only log when we are going from true to flase.
            if (isInternetAvailable != false)
            {
                Debug.Log("No Internet :(");
                isInternetAvailable = false; //This was changed from true to false.
                NetworkStateChanged?.Invoke(isInternetAvailable);
            }
        }

        private void InternetAvailable()
        {
            //Only log when we are going from false to true.
            if (isInternetAvailable != true)
            {
                Debug.Log("Internet is available! ;)");
                isInternetAvailable = true; //This was changed from false to true
                NetworkStateChanged?.Invoke(isInternetAvailable);
            }
        }
    }
}
