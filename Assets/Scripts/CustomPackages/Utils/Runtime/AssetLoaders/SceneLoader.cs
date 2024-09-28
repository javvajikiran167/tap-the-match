using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Custom.Utils
{
    public class SceneLoader
    {
        private IEnumerator LoadAssetBundleManifest(string pathToManifest)
        {
            AssetBundle assetBundle = AssetBundle.LoadFromFile(pathToManifest);
            AssetBundleManifest manifest = assetBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");

            yield return null;
        }

        public IEnumerator LoadSceneGivenBundlePathAsync(string pathToBundle, Action<float> loadProgress)
        {
            Debug.Log("PersistentDataPath: " + Application.persistentDataPath);
            Debug.Log(" Path to asset bundle from base app: " + pathToBundle);
            AssetBundleCreateRequest bundleLoadRequest = AssetBundle.LoadFromFileAsync(pathToBundle);
            bundleLoadRequest.allowSceneActivation = false;
            while (!bundleLoadRequest.isDone)
            {
                loadProgress?.Invoke(bundleLoadRequest.progress / 2);
                yield return null;
                if (bundleLoadRequest.progress >= 0.9f)
                    break;
            }
            bundleLoadRequest.allowSceneActivation = true;
            loadProgress?.Invoke(1 / 2);
            yield return bundleLoadRequest;
            AssetBundle myLoadedAssetBundle = bundleLoadRequest.assetBundle;
            if (myLoadedAssetBundle == null)
            {
                Debug.Log("Failed to load AssetBundle!");
                yield break;
            }
            Debug.Log("AssetBundle Load Success");
            CoroutineUtils.instance.StartCoroutine(LoadSceneAsync(myLoadedAssetBundle, loadProgress));
        }

        private IEnumerator LoadSceneAsync(UnityEngine.AssetBundle bundle, Action<float> loadProgress)
        {
            Debug.Log("LoadSceneAsync Called");
            if (bundle.isStreamedSceneAssetBundle)
            {
                string[] scenePaths = bundle.GetAllScenePaths();
                string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePaths[0]);
                AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
                asyncOperation.allowSceneActivation = false;
                while (!asyncOperation.isDone)
                {
                    loadProgress?.Invoke(0.5f + asyncOperation.progress/2);
                    yield return null;
                    if (asyncOperation.progress >= 0.9f)
                        break;
                }
                loadProgress?.Invoke(1);
                asyncOperation.allowSceneActivation = true;
            }
        }

        public IEnumerator LoadSceneAsync(string sceneName, Action<float> loadProgress)
        {
            Debug.Log("LoadSceneAsync Called sceneName: " + sceneName);
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
            asyncOperation.allowSceneActivation = false;
            while (!asyncOperation.isDone)
            {
                loadProgress?.Invoke(asyncOperation.progress);
                yield return null;
                if (asyncOperation.progress >= 0.9f)
                    break;
            }
            loadProgress?.Invoke(1);
            asyncOperation.allowSceneActivation = true;
        }
    }
}