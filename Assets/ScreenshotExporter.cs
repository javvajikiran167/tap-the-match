using UnityEngine;
using System.IO;

public class ScreenshotExporter : MonoBehaviour
{
    public enum DeviceType { iPhone, iPad }
    public enum Orientation { Portrait, Landscape }

    [Header("Screenshot Settings")]
    public DeviceType selectedDevice = DeviceType.iPhone;
    public Orientation selectedOrientation = Orientation.Portrait;
    public KeyCode screenshotKey = KeyCode.F12;
    public string filePrefix = "screenshot";
    public string saveFolderPath = "D:/UnityScreenshots";

    [Header("Cameras")]
    public Camera mainCamera;      // Game camera
    public Camera particleCamera;  // Particle FX camera

    private int screenshotCount = 0;

    void Update()
    {
        if (Input.GetKeyDown(screenshotKey))
        {
            Vector2Int res = GetResolution(selectedDevice, selectedOrientation);
            TakeScreenshot(res.x, res.y);
        }
    }

    Vector2Int GetResolution(DeviceType device, Orientation orientation)
    {
        return device switch
        {
            DeviceType.iPhone => orientation == Orientation.Portrait ? new Vector2Int(1290, 2796) : new Vector2Int(2796, 1290),
            DeviceType.iPad => orientation == Orientation.Portrait ? new Vector2Int(2064, 2752) : new Vector2Int(2752, 2064),
            _ => new Vector2Int(1920, 1080)
        };
    }

    void TakeScreenshot(int width, int height)
    {
        if (!Directory.Exists(saveFolderPath))
            Directory.CreateDirectory(saveFolderPath);

        string filename = $"{filePrefix}_{selectedDevice}_{selectedOrientation}_{screenshotCount}_{width}x{height}.png";
        string path = Path.Combine(saveFolderPath, filename);

        StartCoroutine(CaptureScreenshot(path, width, height));
        screenshotCount++;
    }

    System.Collections.IEnumerator CaptureScreenshot(string path, int width, int height)
    {
        yield return new WaitForEndOfFrame();

        RenderTexture rt = new RenderTexture(width, height, 24);
        Texture2D screenshot = new Texture2D(width, height, TextureFormat.RGB24, false);

        // Store old targets
        RenderTexture oldMainTarget = mainCamera.targetTexture;
        RenderTexture oldParticleTarget = particleCamera.targetTexture;

        // Set shared render texture
        mainCamera.targetTexture = rt;
        particleCamera.targetTexture = rt;

        // Render both cameras in order
        mainCamera.Render();
        particleCamera.Render();

        // Read and save
        RenderTexture.active = rt;
        screenshot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        screenshot.Apply();

        byte[] bytes = screenshot.EncodeToPNG();
        File.WriteAllBytes(path, bytes);

        // Restore
        mainCamera.targetTexture = oldMainTarget;
        particleCamera.targetTexture = oldParticleTarget;
        RenderTexture.active = null;
        Destroy(rt);

        Debug.Log($"📸 Screenshot saved to: {path}");
    }
}
