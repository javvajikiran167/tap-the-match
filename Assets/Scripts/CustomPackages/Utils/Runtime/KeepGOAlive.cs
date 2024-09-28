using UnityEngine;

namespace Custom.Utils
{
    public class KeepGOAlive : MonoBehaviour
    {
        void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }
}