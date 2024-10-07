using UnityEngine;

namespace Custom.Utils
{
    public class AppSettings : MonoBehaviour
    {
        private void Awake()
        {
            //Debug.unityLogger.logEnabled = isLogEnabled;

            Application.runInBackground = true;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            //Application.targetFrameRate = Screen.currentResolution.refreshRate;
        }

        private void OnDestroy()
        {
            Screen.sleepTimeout = SleepTimeout.SystemSetting;
        }
    }
}
