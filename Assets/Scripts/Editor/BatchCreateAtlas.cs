using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;

public class BatchCreateAtlas : EditorWindow
{
    private string sourceRootFolder = "Assets/Images"; // 输入图片根文件夹的路径
    private string outputRootFolder = "Assets/AssetBundle/Images/Atlas";   // 输出图集的根文件夹路径

    [MenuItem("Tools/Batch Create Atlas")]
    public static void ShowWindow()
    {
        GetWindow(typeof(BatchCreateAtlas), false, "Batch Create Atlas");
    }
    // 清空图集缓存，打图集前必须清一下缓存，否则各种异常
    private static void ClearAtlasCache()
    {
        const string atlasCacheDir = "Library/AtlasCache/";
        var fullPath = System.IO.Path.GetFullPath(atlasCacheDir);
        if (!System.IO.Directory.Exists(fullPath))
        {
            return;
        }
        var files = System.IO.Directory.GetFiles(fullPath, "*.*", System.IO.SearchOption.AllDirectories);
        foreach (var filename in files)
        {
            System.IO.File.Delete(filename);
        }
    }
    private void OnGUI()
    {
        GUILayout.Label("Batch Create Atlas", EditorStyles.boldLabel);

        sourceRootFolder = EditorGUILayout.TextField("Source Root Folder", sourceRootFolder);
        outputRootFolder = EditorGUILayout.TextField("Output Root Folder", outputRootFolder);

        if (GUILayout.Button("Create Atlas"))
        {
            ClearAtlasCache();
            CreateAtlases();
        }
    }

    private void CreateAtlases()
    {
        string[] sourceFolders = AssetDatabase.GetSubFolders(sourceRootFolder);
        if (sourceFolders.Length == 0)
        {
            Debug.LogWarning("No source folders found.");
            return;
        }

        foreach (string sourceFolder in sourceFolders)
        {
            string folderName = System.IO.Path.GetFileName(sourceFolder);

            // 获取源文件夹下的所有图片
            string[] imagePaths = AssetDatabase.FindAssets("t:texture2D", new[] { sourceFolder });
            if (imagePaths.Length == 0)
            {
                Debug.LogWarning($"No images found in the source folder: {folderName}");
                continue;
            }

            // 创建 SpriteAtlas
            SpriteAtlas atlas = new SpriteAtlas();
            atlas.SetPackingSettings(new SpriteAtlasPackingSettings
            {
                blockOffset = 1,
                padding = 2,
                enableRotation = false,
                enableTightPacking = false
            });
            atlas.SetTextureSettings(new SpriteAtlasTextureSettings
            {
                sRGB = true,
                filterMode = FilterMode.Bilinear
            });
            atlas.SetPlatformSettings(new TextureImporterPlatformSettings
            {
                maxTextureSize = 2048,
                compressionQuality = 50
            });

            // 添加所有图片到图集
            foreach (string imagePath in imagePaths)
            {
                Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(AssetDatabase.GUIDToAssetPath(imagePath));
                atlas.Add(new Object[] { texture });
            }

            // 创建输出文件夹
            string outputFolder = $"{outputRootFolder}/{folderName}";
            if (!AssetDatabase.IsValidFolder(outputFolder))
            {
                AssetDatabase.CreateFolder(outputRootFolder, folderName);
            }

            // 保存图集
            string atlasPath = $"{outputFolder}/{folderName}.spriteatlas";
            AssetDatabase.CreateAsset(atlas, atlasPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"Atlas created at: {atlasPath}");
        }
    }
}
