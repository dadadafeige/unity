using UnityEngine;
using UnityEditor;
using System.IO;

public class AudioCompressor : EditorWindow
{
    private int sampleRate = 44100;
    private AudioCompressionFormat compressionFormat = AudioCompressionFormat.Vorbis;
    private AudioClipLoadType loadType = AudioClipLoadType.CompressedInMemory;
    private string targetFolder = "Assets/AssetBundle/Sound/";  // 默认文件夹路径

    [MenuItem("Tools/Batch Audio Compressor")]
    public static void ShowWindow()
    {
        GetWindow<AudioCompressor>("Batch Audio Compressor");
    }

    void OnGUI()
    {
        GUILayout.Label("Batch Audio Compressor", EditorStyles.boldLabel);

        sampleRate = EditorGUILayout.IntSlider("Sample Rate", sampleRate, 8000, 48000);
        compressionFormat = (AudioCompressionFormat)EditorGUILayout.EnumPopup("Compression Format", compressionFormat);
        loadType = (AudioClipLoadType)EditorGUILayout.EnumPopup("Load Type", loadType);
        targetFolder = EditorGUILayout.TextField("Target Folder", targetFolder);

        if (GUILayout.Button("Compress Audio Files"))
        {
            CompressAllAudioFiles(targetFolder);
        }
    }

    private void CompressAllAudioFiles(string folder)
    {
        string[] audioPaths = Directory.GetFiles(folder, "*.mp3", SearchOption.AllDirectories);
        foreach (string path in audioPaths)
        {
            string assetPath = path.Replace(Application.dataPath, "").Replace('\\', '/');
            AudioImporter importer = AssetImporter.GetAtPath(assetPath) as AudioImporter;

            if (importer != null)
            {
                importer.defaultSampleSettings = new AudioImporterSampleSettings
                {
                    compressionFormat = compressionFormat,
                    loadType = loadType,
                    quality = 0.5f,  // 质量设置，0.0到1.0之间
                    sampleRateSetting = AudioSampleRateSetting.OverrideSampleRate,
                    sampleRateOverride = (uint)sampleRate
                };

                AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
                Debug.Log($"Compressed audio at {assetPath}");
            }
        }

        Debug.Log("All audio files in specified folder compressed successfully.");
    }
}
