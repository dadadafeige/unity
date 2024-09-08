using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.UI;
using TMPro;

public class ReplaceTheFont : EditorWindow
{
    private static ReplaceTheFont window = null;
    private static List<string> prefafbPathList = new List<string>();
    private static TMP_FontAsset targetFont;
    private static TMP_FontAsset curFont;

    [MenuItem("Tools/字体工具")]
    public static void ShowWindow()
    {
        if (window == null)
            window = GetWindow(typeof(ReplaceTheFont)) as ReplaceTheFont;
        window.titleContent = new GUIContent("字体工具");
        window.Show();
    }
    void OnGUI()
    {
        targetFont = (TMP_FontAsset)EditorGUILayout.ObjectField("替换字体:", targetFont, typeof(TMP_FontAsset), true);
        curFont = (TMP_FontAsset)EditorGUILayout.ObjectField("被替换字体(不指定全部替换)", curFont, typeof(TMP_FontAsset), true);
        if (GUILayout.Button("替换场景中Text的字体"))
        {
            //寻找Hierarchy面板下所有的Text
            var tArray = Resources.FindObjectsOfTypeAll(typeof(TextMeshProUGUI));
            for (int i = 0; i < tArray.Length; i++)
            {
                TextMeshProUGUI t = tArray[i] as TextMeshProUGUI;
                //记录对象
                Undo.RecordObject(t, t.gameObject.name);
                if (curFont != null)
                {
                    if (t.font.name == curFont.name)
                    {
                        t.font = targetFont;
                    }
                }
                else
                {
                    t.font = targetFont;
                }
                //设置已改变
                EditorUtility.SetDirty(t);
            }
            Debug.Log("完成");
        }
        if (GUILayout.Button("替换预制体中Text的字体"))
        {
            GetFiles(new DirectoryInfo(Application.dataPath), "*.prefab", ref prefafbPathList);
            for (int i = 0; i < prefafbPathList.Count; i++)
            {
                GameObject gameObj = AssetDatabase.LoadAssetAtPath<GameObject>(prefafbPathList[i]);

                TextMeshProUGUI[] t = gameObj.GetComponentsInChildren<TextMeshProUGUI>();
                if (t != null)
                {
                    foreach (Object item in t)
                    {
                        TextMeshProUGUI text = (TextMeshProUGUI)item;
                        //记录对象
                        Undo.RecordObject(text, text.gameObject.name);
                        if (curFont != null)
                        {
                            if (text.font.name == curFont.name)
                            {
                                text.font = targetFont;
                            }
                        }
                        else
                        {
                            text.font = targetFont;
                        }
                        //设置已改变
                        EditorUtility.SetDirty(item);
                    }
                }
            }
            AssetDatabase.SaveAssets();
            Debug.Log("完成");
        }
    }

    /// <summary>
    /// 获得Asset目录下所有预制体对象
    /// </summary>
    /// <param name="directory"></param>
    /// <param name="pattern"></param>
    /// <param name="fileList"></param>
    public static void GetFiles(DirectoryInfo directory, string pattern, ref List<string> fileList)
    {
        if (directory != null && directory.Exists && !string.IsNullOrEmpty(pattern))
        {
            try
            {
                foreach (FileInfo info in directory.GetFiles(pattern))
                {
                    string path = info.FullName.ToString();
                    fileList.Add(path.Substring(path.IndexOf("Assets")));
                }
            }
            catch (System.Exception)
            {
                throw;
            }
            foreach (DirectoryInfo info in directory.GetDirectories())
            {
                GetFiles(info, pattern, ref fileList);
            }
        }
    }
}
