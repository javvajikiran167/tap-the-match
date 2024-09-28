namespace Custom.Utils
{
    public class LogUtils
    {

        public static void Log(params string[] logMsgs)
        {
            string logMsg = string.Join(" | ", logMsgs);
            UnityEngine.Debug.Log(logMsg);
        }

        public static void LogWarning(params string[] logMsgs)
        {
            string logMsg = string.Join(" | ", logMsgs);
            UnityEngine.Debug.LogWarning(logMsg);
        }

        public static void LogError(params string[] logMsgs)
        {
            string logMsg = string.Join(" | ", logMsgs);
            UnityEngine.Debug.LogError(logMsg);
        }

        public static void LogException(params string[] logMsgs)
        {
            string logMsg = string.Join(" | ", logMsgs);
            UnityEngine.Debug.LogException(new System.Exception(logMsg));
        }

        public static void LogObject<T>(T obj)
        {
            UnityEngine.Debug.Log(Dumper.Dump(obj));
        }
    }
}