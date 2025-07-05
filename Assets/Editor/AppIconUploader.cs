#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build;         // NamedBuildTarget
using UnityEditor.iOS;           // iOSPlatformIconKind
using UnityEditor.Android;       // AndroidPlatformIconKind
using UnityEngine;

public static class ModernAppIconUploader
{
    // Change these to match your layout
    private const string IOS_ICON_DIR = "Assets/Assets.xcassets/AppIcon.appiconset";
    private const string ANDROID_ICON_DIR = "Assets/AndroidIcons";

    [MenuItem("Tools/App Icons/Apply All Icons (All Kinds)")]
    public static void ApplyAllIcons()
    {
        // ---------- iOS : every icon kind -----------------------------------
        var iosKinds = new PlatformIconKind[]
        {
            iOSPlatformIconKind.Application,
            iOSPlatformIconKind.Spotlight,
            iOSPlatformIconKind.Settings,
            iOSPlatformIconKind.Notification,
            iOSPlatformIconKind.Marketing      // 1024-px App Store icon
        };

        foreach (var kind in iosKinds)
            ApplyIcons(NamedBuildTarget.iOS, kind, IOS_ICON_DIR);

        // ---------- Android : legacy + adaptive -----------------------------
        ApplyIcons(NamedBuildTarget.Android, AndroidPlatformIconKind.Legacy, ANDROID_ICON_DIR);
        ApplyAdaptiveIcons();   // foreground + background layers

        Debug.Log("<color=green>✅ All iOS & Android icon kinds applied</color>");
    }

    // ---------- single-layer kinds (iOS & Android legacy) ------------------
    private static void ApplyIcons(NamedBuildTarget platform, PlatformIconKind kind, string folder)
    {
        var slots = PlayerSettings.GetPlatformIcons(platform, kind);           // :contentReference[oaicite:0]{index=0}
        foreach (var slot in slots)
        {
            var tex = FindTextureBySize(folder, slot.width);
            if (tex) slot.SetTexture(tex, 0);                                  // layer 0
        }
        PlayerSettings.SetPlatformIcons(platform, kind, slots);                // :contentReference[oaicite:1]{index=1}
    }

    // ---------- Android adaptive (2-layer) ---------------------------------
#if UNITY_2022_1_OR_NEWER
    private static void ApplyAdaptiveIcons()
    {
        var platform = NamedBuildTarget.Android;
        var kind = AndroidPlatformIconKind.Adaptive;
        var slots = PlayerSettings.GetPlatformIcons(platform, kind);

        foreach (var slot in slots)
        {
            var bg = FindTexture($"bg_{slot.width}", ANDROID_ICON_DIR);
            var fg = FindTexture($"fg_{slot.width}", ANDROID_ICON_DIR);
            if (bg && fg) slot.SetTextures(new[] { bg, fg });
        }
        PlayerSettings.SetPlatformIcons(platform, kind, slots);
    }
#endif

    // ---------- helpers ----------------------------------------------------
    private static Texture2D FindTextureBySize(string folder, int size)
    {
        foreach (var guid in AssetDatabase.FindAssets("t:Texture2D", new[] { folder }))
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var tex = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
            if (tex && tex.width == size) { EnsureSprite(path); return tex; }
        }
        Debug.LogWarning($"[Icons] No {size}px texture in “{folder}”.");
        return null;
    }

    private static Texture2D FindTexture(string keyword, string folder)
    {
        var guids = AssetDatabase.FindAssets($"{keyword} t:Texture2D", new[] { folder });
        if (guids.Length == 0) return null;
        var path = AssetDatabase.GUIDToAssetPath(guids[0]);
        EnsureSprite(path);
        return AssetDatabase.LoadAssetAtPath<Texture2D>(path);
    }

    private static void EnsureSprite(string path)
    {
        var imp = (TextureImporter)AssetImporter.GetAtPath(path);
        if (imp.textureType != TextureImporterType.Sprite || imp.mipmapEnabled)
        {
            imp.textureType = TextureImporterType.Sprite;
            imp.mipmapEnabled = false;
            imp.SaveAndReimport();
        }
    }
}
#endif
