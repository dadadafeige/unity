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
        string uiFolder = "Assets/AssetBundle/UI"; // �滻Ϊ��� UI �ļ���·��

        List<UIImageInfo> imageInfoList = new List<UIImageInfo>();

        // ��ȡ���� UI Ԥ����
        string[] uiPrefabPaths = AssetDatabase.FindAssets("t:Prefab", new[] { uiFolder });
        foreach (var prefabPath in uiPrefabPaths)
        {
            string prefabAssetPath = AssetDatabase.GUIDToAssetPath(prefabPath);
            GameObject uiPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabAssetPath);

            // ���� UI Ԥ�����е� Image ���
            Image[] images = uiPrefab.GetComponentsInChildren<Image>(true);
            foreach (var image in images)
            {
                // ��ȡ Image ���õ� Sprite
                Sprite sprite = image.sprite;
                string temp = AssetDatabase.GetAssetPath(sprite);
                bool isInSpecificFolder = temp.StartsWith("Assets/Images/");
                if (isInSpecificFolder)
                {
                    Debug.Log(temp);
                    // ������Ϣ���б�
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

        // ����Ϣ���浽 JSON �ļ�
        string jsonPath = "Assets/Resources/ImageReferences.json"; // �滻Ϊ���뱣��� JSON �ļ�·��
        System.IO.File.WriteAllText(jsonPath, string.Empty);
        string jsonContent = JsonConvert.SerializeObject(imageInfoList, Formatting.Indented);
        System.IO.File.WriteAllText(jsonPath, jsonContent);

        Debug.Log("Image references collected and saved to " + jsonPath);
    }
}


