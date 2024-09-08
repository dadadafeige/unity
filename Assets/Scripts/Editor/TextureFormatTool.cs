using UnityEditor;
using UnityEngine;

public class TextureFormatTool : EditorWindow
{
    [MenuItem("Tools/Set Texture Format")]
    public static void OpenWindow()
    {
        TextureFormatTool window = GetWindow<TextureFormatTool>("Texture Format Tool");
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("Set Default Texture Format", EditorStyles.boldLabel);

        if (GUILayout.Button("Set Default Format"))
        {
            SetDefaultTextureFormat();
            Debug.Log("Default Texture Format set successfully.");
        }
    }

    private static void SetDefaultTextureFormat()
    {
        string[] guids = AssetDatabase.FindAssets("t:texture");
        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            TextureImporter textureImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;

            if (textureImporter != null)
            {
                textureImporter.textureCompression = TextureImporterCompression.Compressed;
                textureImporter.compressionQuality = 50; // Adjust compression quality as needed
                textureImporter.sRGBTexture = true;
                textureImporter.mipmapEnabled = false;
                textureImporter.textureType = TextureImporterType.Sprite;
                textureImporter.spriteImportMode = SpriteImportMode.Single;
                textureImporter.wrapMode = TextureWrapMode.Clamp;


                // Set other properties as needed

                AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
            }
        }
    }
}
