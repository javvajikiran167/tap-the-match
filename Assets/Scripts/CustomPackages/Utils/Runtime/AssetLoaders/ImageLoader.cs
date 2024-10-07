using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Custom.Utils
{
    public static class ImageLoader
    {
        static Dictionary<string, Sprite> downloadedImages = new Dictionary<string, Sprite>();
        public static void LoadImage(string imageUrl, Image imageComponent, Sprite defaultSprite)
        {
            if (imageComponent == null)
            {
                LogUtils.LogError("LoadImage imageComponent parameters is passed as null");
                return;
            }

            if (string.IsNullOrEmpty(imageUrl))
            {
                LogUtils.LogWarning("LoadImage URL is empty");
                imageComponent.sprite = defaultSprite;
                return;
            }

            if (downloadedImages.ContainsKey(imageUrl))
            {
                imageComponent.sprite = null;
                imageComponent.sprite = downloadedImages[imageUrl];
                return;
            }

            CoroutineUtils.Instance.StartCoroutine(DownloadAndAttachImage(imageUrl, imageComponent, defaultSprite));
        }

        static IEnumerator DownloadAndAttachImage(string imageUrl, Image imageComponent, Sprite defaultSprite)
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(imageUrl);
            yield return www.SendWebRequest();
            if (imageComponent != null)
            {
                if (www.result != UnityWebRequest.Result.Success)
                {
                    imageComponent.sprite = defaultSprite;
                    LogUtils.LogError(www.error);
                }
                else
                {
                    Texture2D texture = DownloadHandlerTexture.GetContent(www);
                    imageComponent.sprite = null;
                    Sprite createdSprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100F, 0, SpriteMeshType.FullRect);
                    imageComponent.sprite = createdSprite;
                    downloadedImages[imageUrl] = createdSprite;
                }
            }
        }
    }
}