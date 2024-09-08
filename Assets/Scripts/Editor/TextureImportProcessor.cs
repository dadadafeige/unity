using UnityEditor;
using UnityEngine;

public class TextureImportProcessor : AssetPostprocessor
{
    // ��������ǰ����
    void OnPreprocessTexture()
    {
        if (IsNonPowerOfTwo(assetImporter as TextureImporter))
        {
            // ���õ�������ʱ�Ĵ����߼�
            SetTextureImporterSettings(assetImporter as TextureImporter);
        }
    }

    // ��������Ƿ�Ϊ�Ƕ�����
    bool IsNonPowerOfTwo(TextureImporter importer)
    {
        TextureImporterSettings settings = new TextureImporterSettings();
        importer.ReadTextureSettings(settings);

        return !Mathf.IsPowerOfTwo(settings.maxTextureSize);
    }

    // �������������Ĵ���ѡ��
    void SetTextureImporterSettings(TextureImporter importer)
    {
        // ��ȡ���������
        TextureImporterSettings settings = new TextureImporterSettings();
        importer.ReadTextureSettings(settings);

        // �ڴ�������Ĵ����߼������罫 NPOT ����� WrapMode ����Ϊ Clamp
        settings.wrapMode = TextureWrapMode.Clamp;



        // ���µ���������
        importer.SetTextureSettings(settings);
    }
}
