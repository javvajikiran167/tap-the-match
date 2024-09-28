using System;
using UnityEngine;

namespace Custom.Utils
{
    public class AppPauseChecker : MonoBehaviour
    {
        public static event Action<bool> AppPauseStateChanged;

        void OnApplicationPause(bool pauseStatus)
        {
            AppPauseStateChanged?.Invoke(pauseStatus);
        }
    }
}
