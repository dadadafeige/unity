using UnityEngine;
using UnityEditor;
using System.IO;

public class TextureCompressor : EditorWindow
{
    private int maxSize = 1024;
    private bool useCrunchedCompression = true;
    private int compressionQuality = 50;
    private string targetFolder = "Assets/AssetBundle/Images/Texture/";  // 默认文件夹路径
    private bool applyNPOTScaling = false;
    private TextureImporterNPOTScale npotScale = TextureImporterNPOTScale.None;
    [MenuItem("Tools/Batch Texture Compressor")]
    public static void ShowWindow()
    {
        GetWindow<TextureCompressor>("Batch Texture Compressor");
    }

    void OnGUI()
    {
        GUILayout.Label("Batch Texture Compressor", EditorStyles.boldLabel);

        maxSize = EditorGUILayout.IntSlider("Max Size", maxSize, 32, 2048);
        useCrunchedCompression = EditorGUILayout.Toggle("Use Crunch Compression", useCrunchedCompression);
        compressionQuality = EditorGUILayout.IntSlider("Compression Quality", compressionQuality, 0, 100);
        targetFolder = EditorGUILayout.TextField("Target Folder", targetFolder);
        applyNPOTScaling = EditorGUILayout.Toggle("Apply NPOT Scaling", applyNPOTScaling);
        npotScale = (TextureImporterNPOTScale)EditorGUILayout.EnumPopup("Non-Power of 2", npotScale);
        if (GUILayout.Button("Compress Textures"))
        {
            CompressAllTextures(targetFolder);
        }
    }

    private void CompressAllTextures(string folder)
    {
        string[] texturePaths = Directory.GetFiles(folder, "*.*", SearchOption.AllDirectories);
        foreach (string path in texturePaths)
        {
            if (path.EndsWith(".png") || path.EndsWith(".jpg") || path.EndsWith(".tga") || path.EndsWith(".bmp"))
            {
                string assetPath =  path.Replace(Application.dataPath, "").Replace('\\', '/');
                TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;

                if (importer != null)
                {
                    importer.maxTextureSize = maxSize;
                    importer.textureCompression = TextureImporterCompression.Compressed;
                    importer.crunchedCompression = useCrunchedCompression;
                    importer.compressionQuality = compressionQuality;

                    // 检查是否为精灵
                    if (importer.textureType == TextureImporterType.Sprite)
                    {
                        importer.npotScale = TextureImporterNPOTScale.None;
                    }
                    else
                    {
                        importer.npotScale = npotScale;

                    }

                    // 安卓平台设置
                    TextureImporterPlatformSettings androidSettings = new TextureImporterPlatformSettings
                    {
                        name = "Android",
                        format = TextureImporterFormat.ETC2_RGBA8,
                        maxTextureSize = maxSize,
                        compressionQuality = compressionQuality
                    };
                    importer.SetPlatformTextureSettings(androidSettings);

                    // Windows平台设置
                    TextureImporterPlatformSettings windowsSettings = new TextureImporterPlatformSettings
                    {
                        name = "Standalone",
                        format = TextureImporterFormat.DXT5,
                        maxTextureSize = maxSize,
                        compressionQuality = compressionQuality
                    };
                    importer.SetPlatformTextureSettings(windowsSettings);

                    AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
                    Debug.Log($"Compressed texture at {assetPath}");
                }
            }
        }

        Debug.Log("All textures in specified folder compressed successfully.");
    }
}
