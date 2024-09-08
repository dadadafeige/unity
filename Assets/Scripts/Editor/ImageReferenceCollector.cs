using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ImageReferenceCollector : MonoBehaviour
{
    [MenuItem("Tools/Collect Image References")]
    public static void CollectImageReferences()
    {
        string uiFolder = "Assets/AssetBundle/UI"; // 替换为你的 UI 文件夹路径

        List<UIImageInfo> imageInfoList = new List<UIImageInfo>();

        // 获取所有 UI 预制体
        string[] uiPrefabPaths = AssetDatabase.FindAssets("t:Prefab", new[] { uiFolder });
        foreach (var prefabPath in uiPrefabPaths)
        {
            string prefabAssetPath = AssetDatabase.GUIDToAssetPath(prefabPath);
            GameObject uiPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabAssetPath);

            // 遍历 UI 预制体中的 Image 组件
            Image[] images = uiPrefab.GetComponentsInChildren<Image>(true);
            foreach (var image in images)
            {
                // 获取 Image 引用的 Sprite
                Sprite sprite = image.sprite;
                string temp = AssetDatabase.GetAssetPath(sprite);
                bool isInSpecificFolder = temp.StartsWith("Assets/Images/");
                if (isInSpecificFolder)
                {
                    Debug.Log(temp);
                    // 保存信息到列表
                    UIImageInfo info = new UIImageInfo
                    {
                        UIName = uiPrefab.name,
                        ComponentName = image.name,
                        SpritePath = temp
                    };
                    imageInfoList.Add(info);
                }
              
            }
        }

        // 将信息保存到 JSON 文件
        string jsonPath = "Assets/Resources/ImageReferences.json"; // 替换为你想保存的 JSON 文件路径
        System.IO.File.WriteAllText(jsonPath, string.Empty);
        string jsonContent = JsonConvert.SerializeObject(imageInfoList, Formatting.Indented);
        System.IO.File.WriteAllText(jsonPath, jsonContent);

        Debug.Log("Image references collected and saved to " + jsonPath);
    }
}


