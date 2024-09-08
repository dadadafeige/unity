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

    [MenuItem("Tools/���幤��")]
    public static void ShowWindow()
    {
        if (window == null)
            window = GetWindow(typeof(ReplaceTheFont)) as ReplaceTheFont;
        window.titleContent = new GUIContent("���幤��");
        window.Show();
    }
    void OnGUI()
    {
        targetFont = (TMP_FontAsset)EditorGUILayout.ObjectField("�滻����:", targetFont, typeof(TMP_FontAsset), true);
        curFont = (TMP_FontAsset)EditorGUILayout.ObjectField("���滻����(��ָ��ȫ���滻)", curFont, typeof(TMP_FontAsset), true);
        if (GUILayout.Button("�滻������Text������"))
        {
            //Ѱ��Hierarchy��������е�Text
            var tArray = Resources.FindObjectsOfTypeAll(typeof(TextMeshProUGUI));
            for (int i = 0; i < tArray.Length; i++)
            {
                TextMeshProUGUI t = tArray[i] as TextMeshProUGUI;
                //��¼����
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
                //�����Ѹı�
                EditorUtility.SetDirty(t);
            }
            Debug.Log("���");
        }
        if (GUILayout.Button("�滻Ԥ������Text������"))
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
                        //��¼����
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
                        //�����Ѹı�
                        EditorUtility.SetDirty(item);
                    }
                }
            }
            AssetDatabase.SaveAssets();
            Debug.Log("���");
        }
    }

    /// <summary>
    /// ���AssetĿ¼������Ԥ�������
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
