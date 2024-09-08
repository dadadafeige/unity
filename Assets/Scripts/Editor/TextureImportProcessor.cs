using UnityEditor;
using UnityEngine;

public class TextureImportProcessor : AssetPostprocessor
{
    // 在纹理导入前调用
    void OnPreprocessTexture()
    {
        if (IsNonPowerOfTwo(assetImporter as TextureImporter))
        {
            // 设置导入纹理时的处理逻辑
            SetTextureImporterSettings(assetImporter as TextureImporter);
        }
    }

    // 检查纹理是否为非二次幂
    bool IsNonPowerOfTwo(TextureImporter importer)
    {
        TextureImporterSettings settings = new TextureImporterSettings();
        importer.ReadTextureSettings(settings);

        return !Mathf.IsPowerOfTwo(settings.maxTextureSize);
    }

    // 设置纹理导入器的处理选项
    void SetTextureImporterSettings(TextureImporter importer)
    {
        // 获取纹理的设置
        TextureImporterSettings settings = new TextureImporterSettings();
        importer.ReadTextureSettings(settings);

        // 在此添加您的处理逻辑，例如将 NPOT 纹理的 WrapMode 设置为 Clamp
        settings.wrapMode = TextureWrapMode.Clamp;



        // 更新导入器设置
        importer.SetTextureSettings(settings);
    }
}
