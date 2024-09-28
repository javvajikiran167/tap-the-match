using UnityEngine;

namespace Custom.Utils
{
    public static class ScreenUtils
    {
        public static void LockToAnyLandscape()
        {
            LogUtils.Log("ScreenUtils: LockToAnyLandscape called");
            Screen.orientation = ScreenOrientation.AutoRotation;
            Screen.autorotateToLandscapeLeft = true;
            Screen.autorotateToLandscapeRight = true;
            Screen.autorotateToPortrait = false;
            Screen.autorotateToPortraitUpsideDown = false;
        }

        public static void LockToPotrait()
        {
            LogUtils.Log("ScreenUtils: LockToPotrait called");
            Screen.orientation = ScreenOrientation.Portrait;
            Screen.autorotateToLandscapeLeft = false;
            Screen.autorotateToLandscapeRight = false;
            Screen.autorotateToPortrait = false;
            Screen.autorotateToPortraitUpsideDown = false;
        }

        public static void LockToAutoRotation()
        {
            LogUtils.Log("ScreenUtils: LockToAutoRotation called");

            Screen.orientation = ScreenOrientation.AutoRotation;
            Screen.autorotateToLandscapeLeft = true;
            Screen.autorotateToLandscapeRight = true;
            Screen.autorotateToPortrait = true;
            Screen.autorotateToPortraitUpsideDown = true;
        }
    }
}
