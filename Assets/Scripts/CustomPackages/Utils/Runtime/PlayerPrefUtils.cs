using UnityEngine;

namespace Custom.Utils
{
    public class PlayerPrefUtils
    {
        public static void SetLong(string key, long value)
        {
            PlayerPrefs.SetString(key, value.ToString());
        }

        public static long GetLong(string key)
        {
            return long.Parse(PlayerPrefs.GetString(key, "0"));
        }

        public static void SetObject<T>(string key, T value)
        {
            string data = JsonUtility.ToJson(value);
            PlayerPrefs.SetString(key, data);
        }

        public static T GetObject<T>(string key, T defaultVal)
        {
            string value = PlayerPrefs.GetString(key, "");
            if (value.Length <= 0)
            {
                return defaultVal;
            }

            return JsonUtility.FromJson<T>(value);
        }
    }
}